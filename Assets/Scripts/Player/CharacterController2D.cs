using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Player
{
	[RequireComponent(typeof(Rigidbody2D))]
	public sealed class CharacterController2D : MonoBehaviour
	{
		[SerializeField]
		private float _jumpForce = 400f;				// Amount of force added when the player jumps
		[SerializeField]
		private float _runSpeed = 40.0f;				// Player run speed
		[SerializeField] [Range(0, 1)]
		private float _crouchSpeed = 0.36f;				// Amount of maxSpeed applied to crouching movement. 1 = 100%
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
		[SerializeField]
		private Collider2D _crouchDisableCollider;		// A collider that will be disabled when crouching

		#region Constants
		private const float k_GroundedRadius = 0.2f; 	// Radius of the overlap circle to determine if grounded
		private const float k_CeilingRadius = 0.2f;		// Radius of the overlap circle to determine if the player can stand up
		#endregion

		[Header("Components")]
		#region Components
		[SerializeField]
		private Rigidbody2D _rigidbody2D;				// Rigidbody2D component of the gameObject
		[SerializeField]
		private BoxCollider2D _boxCollider2D;			// Upper BoxCollider2D component of the gameObject
		[SerializeField]
		private CircleCollider2D _circleCollider2D;		// Lower CircleCollider2D component of the gameObject
		[SerializeField]
		private SpriteRenderer _spriteRenderer;			// SpriteRenderer component of the gameObject
		#endregion

		private Vector3 _velocity = Vector3.zero;		// Velocity Vector3 of the gameObject

		private bool _grounded;							// Whether or not the player is grounded
		private bool _facingRight = true;				// For determining which way the player is currently facing
		private bool _wasCrouching = false;


		[Header("Events")]
		#region Events
		public UnityEvent OnLandEvent;
		[System.Serializable]
		public class BoolEvent : UnityEvent<bool> { }
		public BoolEvent OnCrouchEvent;
		#endregion

		#region Awake
		private void Awake()
		{
			if (OnLandEvent == null)
				OnLandEvent = new UnityEvent();

			if (OnCrouchEvent == null)
				OnCrouchEvent = new BoolEvent();
		}
		#endregion

		private void Update()
		{
			if (Input.GetKeyDown ("f"))
				FlipFacingDirection ();
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

		public void Move(float move, bool crouch, bool jump)
		{
			move = move * _runSpeed * Time.fixedDeltaTime;

			// If crouching, check to see if the character can stand up
			if (!crouch)
			{
				// If the character has a ceiling preventing them from standing up, keep them crouching
				if (Physics2D.OverlapCircle(_ceilingCheck.position, k_CeilingRadius, _groundLayer))
				{
					crouch = true;
				}
			}

			//only control the player if grounded or airControl is turned on
			if (_grounded || _airControl)
			{

				// If crouching
				if (crouch)
				{
					if (!_wasCrouching)
					{
						_wasCrouching = true;
						OnCrouchEvent.Invoke(true);
					}

					// Reduce the speed by the crouchSpeed multiplier
					move *= _crouchSpeed;

					// Disable one of the colliders when crouching
					if (_crouchDisableCollider != null)
						_crouchDisableCollider.enabled = false;
				} else
				{
					// Enable the collider when not crouching
					if (_crouchDisableCollider != null)
						_crouchDisableCollider.enabled = true;

					if (_wasCrouching)
					{
						_wasCrouching = false;
						OnCrouchEvent.Invoke(false);
					}
				}

				// Move the character by finding the target velocity
				Vector3 l_targetVelocity = new Vector2(move * 10f, _rigidbody2D.velocity.y);
				// And then smoothing it out and applying it to the character
				_rigidbody2D.velocity = Vector3.SmoothDamp(_rigidbody2D.velocity, l_targetVelocity, ref _velocity, _movementSmoothing);

				// If the input is moving the player right and the player is facing left...
				if (move > 0 && !_facingRight)
				{
					// ... flip the player.
					FlipFacingDirection();
				}
				// Otherwise if the input is moving the player left and the player is facing right...
				else if (move < 0 && _facingRight)
				{
					// ... flip the player.
					FlipFacingDirection();
				}
			}
			// If the player should jump...
			if (_grounded && jump)
			{
				// Add a vertical force to the player.
				_grounded = false;
				_rigidbody2D.AddForce(new Vector2(0f, _jumpForce));
			}
		}

		private void FlipFacingDirection()
		{
			// Switch the way the player is labelled as facing.
			_facingRight = !_facingRight;
			// Flip player sprite in X axis
			_spriteRenderer.flipX = !_spriteRenderer.flipX;
			// Change BoxCollider2D X offset so it matches the sprite when flipped
			Vector2 l_boxCollider2DOffset = _boxCollider2D.offset;
			_boxCollider2D.offset = new Vector2 (-l_boxCollider2DOffset.x, l_boxCollider2DOffset.y);
			// Change CircleCollider2D X offset so it matches the sprite when flipped
			Vector2 l_circleCollider2DOffset = _circleCollider2D.offset;
			_circleCollider2D.offset = new Vector2 (-l_circleCollider2DOffset.x, l_circleCollider2DOffset.y);
		}
	}
}
