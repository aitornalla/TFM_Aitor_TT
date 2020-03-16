using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Player
{
	[RequireComponent(typeof(Rigidbody2D))]
	public sealed class CharacterController2D : MonoBehaviour
	{
		[SerializeField]
		private float _jumpForce = 525.0f;              // Amount of force added when the player jumps
		[SerializeField]
		private float _slideForce = 125.0f;             // Amount of force added when the player slides
		[SerializeField]
		private float _runSpeed = 40.0f;				// Player run speed
		[SerializeField] [Range(0, .3f)]
		private float _movementSmoothing = 0.05f;		// How much to smooth out the movement
		[SerializeField]
		private bool _airControl = false;				// Whether or not a player can steer while jumping
		[SerializeField]
		private LayerMask _groundLayer;					// A mask determining what is ground to the character
		[SerializeField]
		private Transform _groundCheck;					// A position marking where to check if the player is grounded
		[SerializeField]
		private Transform _ceilingCheck;				// A position marking where to check for ceilings

		#region Constants
		private const float k_GroundedRadius = 0.2f; 	// Radius of the overlap circle to determine if grounded
		private const float k_CeilingRadius = 0.2f;		// Radius of the overlap circle to determine if the player can stand up
		#endregion

		[Header("Components")]
		#region Components
		[SerializeField]
		private Rigidbody2D _rigidbody2D;               // Rigidbody2D component of the gameObject
		[SerializeField]
		private CapsuleCollider2D _capsuleCollider2D;   // Not-sliding CapsuleCollider2D component of the gameObject
		[SerializeField]
		private CapsuleCollider2D _slideCapCollider2D;  // Sliding CapsuleCollider2D component of the gameObject
		[SerializeField]
		private SpriteRenderer _spriteRenderer;			// SpriteRenderer component of the gameObject
		#endregion

		private Vector3 _velocity = Vector3.zero;		// Velocity Vector3 of the gameObject

		private bool _grounded;							// Whether or not the player is grounded
        private bool _facingRight = true;               // For determining which way the player is currently facing
		private bool _wasSliding = false;               // Was the player sliding in the previous frame?

		[Header("Events")]
		#region Events
		public UnityEvent OnLandEvent;
		public BoolEvent OnSlideEvent;
		#endregion

		#region Awake
		private void Awake()
		{
			if (OnLandEvent == null)
				OnLandEvent = new UnityEvent();

			if (OnSlideEvent == null)
				OnSlideEvent = new BoolEvent();
		}
		#endregion

		private void Update()
		{

		}

		#region FixedUpdate
		private void FixedUpdate()
		{
			bool l_wasGrounded = _grounded;

			_grounded = false;

			// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
			// This can be done using layers instead but Sample Assets will not overwrite your project settings.
			Collider2D[] colliders = Physics2D.OverlapCircleAll(_groundCheck.position, k_GroundedRadius, _groundLayer);

			for (int i = 0; i < colliders.Length; i++)
			{
				if (colliders[i].gameObject != gameObject)
				{
					_grounded = true;

					if (!l_wasGrounded)
						OnLandEvent.Invoke();
				}
			}
		}
		#endregion

		public void Move(float move, bool jump, bool slide)
		{
			// Only control the player if grounded or airControl is turned on
			if (_grounded) // || _airControl)
			{
				if (slide)
				{
					if (!_wasSliding)
					{
						// Set sliding flag to true
						_wasSliding = true;
						// Trigger slide event
						OnSlideEvent.Invoke(true);
						// Disable main CapsuldeCollider2D component
						if (_capsuleCollider2D != null)
							_capsuleCollider2D.enabled = false;
						// Enable slide CapsuleCollider2D component
						if (_slideCapCollider2D != null)
							_slideCapCollider2D.enabled = true;

                        if (Mathf.Abs(_rigidbody2D.velocity.x) > 0.01f)
                        {
							// Set slide force direction
							float l_slideForceDirection = _facingRight ? 1.0f : -1.0f;
							// Apply slide force to rigid body
							_rigidbody2D.AddForce(new Vector2(l_slideForceDirection * _slideForce, 0.0f));
						}
					}
					//else if (_wasSliding && Mathf.Abs(_rigidbody2D.velocity.x) < 0.01f)
					//{
					//    // Set sliding flag to false
					//    _wasSliding = false;
					//    // Trigger slide event
					//    OnSlideEvent.Invoke(false);
					//    // Enable main CapsuldeCollider2D component
					//    if (_capsuleCollider2D != null)
					//        _capsuleCollider2D.enabled = true;
					//    // Disable slide CapsuleCollider2D component
					//    if (_slideCapCollider2D != null)
					//        _slideCapCollider2D.enabled = false;
					//}
				}
				else
				{
					if (_wasSliding)
					{
						// Set sliding flag to false
						_wasSliding = false;
						// Trigger slide event
						OnSlideEvent.Invoke(false);
						// Enable main CapsuldeCollider2D component
						if (_capsuleCollider2D != null)
							_capsuleCollider2D.enabled = true;
						// Disable slide CapsuleCollider2D component
						if (_slideCapCollider2D != null)
							_slideCapCollider2D.enabled = false;
					}

					// Apply run speed and fixed delta time to move parameter
					move *= _runSpeed * Time.fixedDeltaTime;
					// Move the character by finding the target velocity
					Vector3 l_targetVelocity = new Vector2(move * 10.0f, _rigidbody2D.velocity.y);
					// And then smoothing it out and applying it to the character
					_rigidbody2D.velocity = Vector3.SmoothDamp(_rigidbody2D.velocity, l_targetVelocity, ref _velocity, _movementSmoothing);

					// If the input is moving the player right and the player is facing left...
					if (move > 0.0f && !_facingRight)
					{
						// ... flip the player.
						FlipFacingDirection();
					}
					// Otherwise if the input is moving the player left and the player is facing right...
					else if (move < 0.0f && _facingRight)
					{
						// ... flip the player.
						FlipFacingDirection();
					}
				}

				#region Player jump
				if (jump)
				{
					// Check if player was sliding to put back main settings
					if (_wasSliding)
					{
						// Set sliding flag to false
						_wasSliding = false;
						// Trigger slide event
						OnSlideEvent.Invoke(false);
						// Enable main CapsuldeCollider2D component
						if (_capsuleCollider2D != null)
							_capsuleCollider2D.enabled = true;
						// Disable slide CapsuleCollider2D component
						if (_slideCapCollider2D != null)
							_slideCapCollider2D.enabled = false;
					}

					// Set grounded to false
					_grounded = false;
					// Add a vertical force to the player
					_rigidbody2D.AddForce(new Vector2(0.0f, _jumpForce));
				}
				#endregion
			}
			else
            {
				
			}
		}

		private void FlipFacingDirection()
		{
			// Switch the way the player is labelled as facing.
			_facingRight = !_facingRight;
			// Flip player sprite in X axis
			_spriteRenderer.flipX = !_spriteRenderer.flipX;
			// Change not-sliding CapsuleCollider2D X offset so it matches the sprite when flipped
			Vector2 l_capsuleCollider2DOffset = _capsuleCollider2D.offset;
			_capsuleCollider2D.offset = new Vector2(-l_capsuleCollider2DOffset.x, l_capsuleCollider2DOffset.y);
			// Change sliding CapsuleCollider2D X offset so it matches the sprite when flipped
			Vector2 l_slideCapCollider2DOffset = _slideCapCollider2D.offset;
			_slideCapCollider2D.offset = new Vector2(-l_slideCapCollider2DOffset.x, l_slideCapCollider2DOffset.y);
		}
	}

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool>
    {

    }
}

/*
 * // Apply run speed and fixed delta time to move parameter
			move *= _runSpeed * Time.fixedDeltaTime;

			// Only control the player if grounded or airControl is turned on
			if (_grounded) // || _airControl)
			{
                if (slide)
                {
                    if (!_wasSliding && Mathf.Abs(_rigidbody2D.velocity.x) > 0.01f)
                    {
                        // Set sliding flag to true
                        _wasSliding = true;
                        // Trigger slide event
                        OnSlideEvent.Invoke(true);
                        // Disable main CapsuldeCollider2D component
                        if (_capsuleCollider2D != null)
                            _capsuleCollider2D.enabled = false;
                        // Enable slide CapsuleCollider2D component
                        if (_slideCapCollider2D != null)
                            _slideCapCollider2D.enabled = true;
                        // Set slide force direction
						float l_slideForceDirection = _facingRight ? 1.0f : -1.0f;
                        // Apply slide force to rigid body
                        _rigidbody2D.AddForce(new Vector2(l_slideForceDirection * _slideForce, 0.0f));
                    }
                    //else if (_wasSliding && Mathf.Abs(_rigidbody2D.velocity.x) < 0.01f)
                    //{
                    //    // Set sliding flag to false
                    //    _wasSliding = false;
                    //    // Trigger slide event
                    //    OnSlideEvent.Invoke(false);
                    //    // Enable main CapsuldeCollider2D component
                    //    if (_capsuleCollider2D != null)
                    //        _capsuleCollider2D.enabled = true;
                    //    // Disable slide CapsuleCollider2D component
                    //    if (_slideCapCollider2D != null)
                    //        _slideCapCollider2D.enabled = false;
                    //}
                }
                else
                {
                    if (_wasSliding)
                    {
                        // Set sliding flag to false
                        _wasSliding = false;
                        // Trigger slide event
                        OnSlideEvent.Invoke(false);
						// Enable main CapsuldeCollider2D component
						if (_capsuleCollider2D != null)
                            _capsuleCollider2D.enabled = true;
						// Disable slide CapsuleCollider2D component
						if (_slideCapCollider2D != null)
                            _slideCapCollider2D.enabled = false;
                    }

					// Move the character by finding the target velocity
					Vector3 l_targetVelocity = new Vector2(move * 10.0f, _rigidbody2D.velocity.y);
					// And then smoothing it out and applying it to the character
					_rigidbody2D.velocity = Vector3.SmoothDamp(_rigidbody2D.velocity, l_targetVelocity, ref _velocity, _movementSmoothing);

					// If the input is moving the player right and the player is facing left...
					if (move > 0.0f && !_facingRight)
					{
						// ... flip the player.
						FlipFacingDirection();
					}
					// Otherwise if the input is moving the player left and the player is facing right...
					else if (move < 0.0f && _facingRight)
					{
						// ... flip the player.
						FlipFacingDirection();
					}
				}
			}
            #region Player jump
            if (_grounded && jump)
			{
                // Check if player was sliding to put back main settings
				if (_wasSliding)
				{
					// Set sliding flag to false
					_wasSliding = false;
					// Trigger slide event
					OnSlideEvent.Invoke(false);
					// Enable main CapsuldeCollider2D component
					if (_capsuleCollider2D != null)
						_capsuleCollider2D.enabled = true;
					// Disable slide CapsuleCollider2D component
					if (_slideCapCollider2D != null)
						_slideCapCollider2D.enabled = false;
				}

				// Set grounded to false
				_grounded = false;
				// Add a vertical force to the player
				_rigidbody2D.AddForce(new Vector2(0.0f, _jumpForce));
			}
            #endregion
 */
