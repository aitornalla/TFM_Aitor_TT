using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Player
{
	[RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CharacterInputController2D))]
	public sealed class CharacterController2D : MonoBehaviour
	{
		[SerializeField]
		private float _jumpForce = 525.0f;              // Amount of force added when the player jumps
		[SerializeField]
		private float _slideForce = 125.0f;             // Amount of force added when the player slides
		[SerializeField]
		private float _glideSpeed = -0.5f;              // Target gliding speed
        [SerializeField]
		private float _runSpeed = 40.0f;				// Player run speed
		[SerializeField] [Range(0, .3f)]
		private float _movementSmoothing = 0.05f;		// How much to smooth out the movement
		[SerializeField]
		private bool _airControl = false;				// Whether or not a player can steer while jumping
		[SerializeField]
		private LayerMask _groundLayer;					// A mask determining what is ground to the character

        [Header("Checkers")]
        #region Checkers
        [SerializeField]
		private Transform _groundCheck;					// A position marking where to check if the player is grounded
		[SerializeField]
		private Transform _ceilingCheck;                // A position marking where to check for ceilings
		[SerializeField]
		private Transform _attackCheck;                 // A position marking where to check for attack
        #endregion

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
		private CapsuleCollider2D _glideCapCollider2D;  // Gliding CapsuleCollider2D component of the gameObject
		[SerializeField]
		private CapsuleCollider2D _attackCapCollider2D; // Attack CapsuleCollider2D component of the gameObject
		[SerializeField]
		private SpriteRenderer _spriteRenderer;         // SpriteRenderer component of the gameObject
        #endregion

        #region Variables
        private Vector3 _velocity = Vector3.zero;       // Velocity Vector3 of the gameObject
		private Collider2D[] _collider2DArrary;         // GameObject Collider2D array
        #endregion

        #region Flags
        private bool _grounded;							// Whether or not the player is grounded
        private bool _facingRight = true;               // For determining which way the player is currently facing
		private bool _wasSliding = false;               // Was the player sliding in the previous frame?
		private bool _hasDoubleJumped = false;          // Flag for double jumping
		private bool _wasGliding = false;               // Is the player gliding?
		private bool _isAttacking = false;
        #endregion

        [Header("Events")]
		#region Events
		public BoolEvent OnGroundedEvent;
		public BoolEvent OnSlideEvent;
		public UnityEvent OnDoubleJumpEvent;
		public BoolEvent OnGlideEvent;
		public UnityEvent OnAttackEndEvent;
        #endregion

        #region Properties
        public bool IsGrounded { get { return _grounded; } }
        public bool IsFacingRight { get { return _facingRight; } }
        public bool WasSliding { get { return _wasSliding; } }
        public bool HasDoubleJumped { get { return _hasDoubleJumped; } }
		public bool WasGliding { get { return _wasGliding; } }
		public bool IsAttacking { get { return _isAttacking; } }
        #endregion

        #region Awake
        private void Awake()
		{
            // Initialize events
			if (OnGroundedEvent == null)
				OnGroundedEvent = new BoolEvent();

			if (OnSlideEvent == null)
				OnSlideEvent = new BoolEvent();

			if (OnDoubleJumpEvent == null)
				OnDoubleJumpEvent = new UnityEvent();

			if (OnGlideEvent == null)
				OnGlideEvent = new BoolEvent();

			if (OnAttackEndEvent == null)
				OnAttackEndEvent = new UnityEvent();

			// Get gameObject Collider2D references
			_collider2DArrary = gameObject.GetComponents<Collider2D>();
		}
		#endregion

		private void Update()
		{
            
		}

		#region FixedUpdate
		private void FixedUpdate()
		{
            // First set grounded flag to false
            _grounded = false;

			// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
			// This can be done using layers instead but Sample Assets will not overwrite your project settings.
			Collider2D[] colliders = Physics2D.OverlapCircleAll(_groundCheck.position, k_GroundedRadius, _groundLayer);

			for (int i = 0; i < colliders.Length; i++)
			{
				if (colliders[i].gameObject != gameObject)
				{
					_grounded = true;
					_hasDoubleJumped = false;
					_wasGliding = false;
				}
			}
            // Trigger grounded event for animator state changes (includes double jump and glide setting)
			OnGroundedEvent.Invoke(_grounded);
		}
        #endregion

        #region Move
        public void Move(float move, bool jump, bool slide, bool glide, bool attack)
		{
            #region Grounded
            if (_grounded)
			{
                // If player is attacking control of the player is disabled
				if (_isAttacking)
					return;

                if (attack && !_isAttacking)
                {
					// Check if player was sliding to put back main settings
					if (_wasSliding)
					{
						// Set sliding flag to false
						_wasSliding = false;
						// Trigger slide event for animator state changes
						OnSlideEvent.Invoke(false);
					}
					// Manage gameObject Collider2Ds
					ManageCollider2Ds(_attackCapCollider2D);
                    // Set "is attacking" flag to true
					_isAttacking = true;

					return;
				}

                #region Slide
                if (slide)
				{
					if (!_wasSliding)
					{
						// Set sliding flag to true
						_wasSliding = true;
						// Trigger slide event for animator state changes
						OnSlideEvent.Invoke(true);
                        // Manage gameObject Collider2Ds
						ManageCollider2Ds(_slideCapCollider2D);

                        if (Mathf.Abs(_rigidbody2D.velocity.x) > 0.01f)
                        {
							// Set slide force direction
							float l_slideForceDirection = _facingRight ? 1.0f : -1.0f;
							// Apply slide force to rigid body
							_rigidbody2D.AddForce(new Vector2(l_slideForceDirection * _slideForce, 0.0f));
						}
					}
				}
                #endregion
                #region Not slide
                else
                {
					if (_wasSliding)
					{
						// Set sliding flag to false
						_wasSliding = false;
						// Trigger slide event for animator state changes
						OnSlideEvent.Invoke(false);
					}
					// Manage gameObject Collider2Ds
					ManageCollider2Ds(_capsuleCollider2D);
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
                #endregion
                #region Player jump
                if (jump)
				{
					// Check if player was sliding to put back main settings
					if (_wasSliding)
					{
						// Set sliding flag to false
						_wasSliding = false;
						// Trigger slide event for animator state changes
						OnSlideEvent.Invoke(false);
						// Manage gameObject Collider2Ds
						ManageCollider2Ds(_capsuleCollider2D);
					}
					// Add a vertical force to the player
					_rigidbody2D.AddForce(new Vector2(0.0f, _jumpForce));
				}
				#endregion
			}
            #endregion
            #region Not grounded
            else
            {
				if (_airControl)
                {

                }

                #region Double jump
                if (jump && !_hasDoubleJumped && !_wasGliding)
                {
                    // Put double jump flag to true
					_hasDoubleJumped = true;
					// Trigger double jump event for animator state changes
					OnDoubleJumpEvent.Invoke();
                    // Zero out y velocity before applying jump force
					_rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0.0f);
					// Add a vertical force to the player
					_rigidbody2D.AddForce(new Vector2(0.0f, _jumpForce));
				}
                #endregion

                #region Glide
                if (glide)
                {
                    if (!_wasGliding)
                    {
						// Zero out y velocity before applying glide
						_rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0.0f);
						// Manage gameObject Collider2Ds
						ManageCollider2Ds(_glideCapCollider2D);
					}
					// Put glide flag to true
					_wasGliding = true;
					// Trigger glide event for animator state changes
					OnGlideEvent.Invoke(true);
                    // Apply smooth transition to player glide velocity
					_rigidbody2D.velocity = Vector3.SmoothDamp(_rigidbody2D.velocity, new Vector3(_rigidbody2D.velocity.x, _glideSpeed, 0.0f), ref _velocity, _movementSmoothing);
				}
                else
                {
					// Put glide flag to false
					_wasGliding = false;
					// Manage gameObject Collider2Ds
					ManageCollider2Ds(_capsuleCollider2D);
					// Trigger glide event for animator state changes
					OnGlideEvent.Invoke(false);
				}
                #endregion
            }
            #endregion
        }
        #endregion

        /// <summary>
        ///     Changes facing right flag and reoffsets gameObject colliders
        /// </summary>
        private void FlipFacingDirection()
		{
			// Switch the way the player is labelled as facing.
			_facingRight = !_facingRight;
			// Flip player sprite in X axis
			_spriteRenderer.flipX = !_spriteRenderer.flipX;
            // ReOffsets gameObject Collider2Ds
			ReOffsetCollider2Ds();
		}

        /// <summary>
        ///     Enables one Collider2D and disables the other gameObject Collider2Ds
        /// </summary>
        /// <param name="col"><see cref="Collider2D"/> to enable</param>
        private void ManageCollider2Ds(Collider2D col)
        {
            for (int i = 0; i < _collider2DArrary.Length; i++)
            {
				_collider2DArrary[i].enabled = col.Equals(_collider2DArrary[i]) ? true : false;
            }
        }

        /// <summary>
        ///     ReOffsets gameObject Collider2Ds when changing facing direction
        /// </summary>
        private void ReOffsetCollider2Ds()
        {
			Vector2 l_offset = Vector2.zero;

			for (int i = 0; i < _collider2DArrary.Length; i++)
			{
				l_offset = _collider2DArrary[i].offset;

				_collider2DArrary[i].offset = new Vector2(-l_offset.x, l_offset.y);
			}
		}

        /// <summary>
        ///     Gets called when attack animation reaches a keyframe when the attack should damage enemies
        /// </summary>
        public void OnAttackAnimationEvent()
        {
			
        }

        /// <summary>
        ///     Gets called when attack animation ends, sets "is attacking" flag to false
        /// </summary>
        public void OnAttackEndAnimationEvent()
        {
			// Set "is attacking" flag to false
			_isAttacking = false;
            // Triggers attack end event for character input control states
			OnAttackEndEvent.Invoke();
		}
	}

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool>
    {

    }
}
