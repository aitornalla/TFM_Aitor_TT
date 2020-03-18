using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Player
{
    [RequireComponent(typeof(CharacterComponents))]
	[RequireComponent(typeof(CharacterEvents))]
	[RequireComponent(typeof(CharacterFlags))]
	[RequireComponent(typeof(CharacterHealth))]
	[RequireComponent(typeof(CharacterInputController2D))]
	[RequireComponent(typeof(CharacterParams))]
	[RequireComponent(typeof(Rigidbody2D))]
	public sealed class CharacterController2D : MonoBehaviour
	{
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
		private const float k_CeilingRadius = 0.2f;     // Radius of the overlap circle to determine if the player can stand up
        #endregion

        #region Character Components
        [SerializeField]
		private CharacterComponents _characterComponents;
        #endregion

        #region Variables
        private Vector3 _velocity = Vector3.zero;       // Velocity Vector3 of the gameObject
		private Collider2D[] _collider2DArrary;         // GameObject Collider2D array
        #endregion

        #region Awake
        private void Awake()
		{
            // Events initialized in CharacterEvents component

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
            _characterComponents.CharacterFlags.IsGrounded = false;

			// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
			// This can be done using layers instead but Sample Assets will not overwrite your project settings.
			Collider2D[] colliders =
                Physics2D.
                OverlapCircleAll(_groundCheck.position, k_GroundedRadius, _characterComponents.CharacterParams.GroundLayer);

			for (int i = 0; i < colliders.Length; i++)
			{
				if (colliders[i].gameObject != gameObject)
				{
					_characterComponents.CharacterFlags.IsGrounded = true;
					_characterComponents.CharacterFlags.HasDoubleJumped = false;
					_characterComponents.CharacterFlags.WasGliding = false;
				}
			}
			// Trigger grounded event for animator state changes (includes double jump and glide setting)
			_characterComponents.CharacterEvents.OnGroundedEvent.Invoke(_characterComponents.CharacterFlags.IsGrounded);
		}
        #endregion

        #region Move
        public void Move(float move, bool jump, bool slide, bool glide, bool attack)
		{
            #region Grounded
            if (_characterComponents.CharacterFlags.IsGrounded)
			{
                // If player is attacking control of the player is disabled
				if (_characterComponents.CharacterFlags.IsAttacking)
					return;

                if (attack && !_characterComponents.CharacterFlags.IsAttacking)
                {
					// Check if player was sliding to put back main settings
					if (_characterComponents.CharacterFlags.WasSliding)
					{
						// Set sliding flag to false
						_characterComponents.CharacterFlags.WasSliding = false;
						// Trigger slide event for animator state changes
						_characterComponents.CharacterEvents.OnSlideEvent.Invoke(false);
					}
					// Manage gameObject Collider2Ds
					ManageCollider2Ds(_characterComponents.AttackCapsuleCollider2D);
					// Set "is attacking" flag to true
					_characterComponents.CharacterFlags.IsAttacking = true;

					return;
				}

                #region Slide
                if (slide)
				{
					if (!_characterComponents.CharacterFlags.WasSliding)
					{
						// Set sliding flag to true
						_characterComponents.CharacterFlags.WasSliding = true;
						// Trigger slide event for animator state changes
						_characterComponents.CharacterEvents.OnSlideEvent.Invoke(true);
                        // Manage gameObject Collider2Ds
						ManageCollider2Ds(_characterComponents.SlideCapsuleCollider2D);

                        if (Mathf.Abs(_characterComponents.Rigidbody2D.velocity.x) > 0.01f)
                        {
							// Set slide force direction
							float l_slideForceDirection = _characterComponents.CharacterFlags.IsFacingRight ? 1.0f : -1.0f;
							// Apply slide force to rigid body
							_characterComponents
                                .Rigidbody2D
                                .AddForce(new Vector2(l_slideForceDirection * _characterComponents.CharacterParams.SlideForce, 0.0f));
						}
					}
				}
                #endregion
                #region Not slide
                else
                {
					if (_characterComponents.CharacterFlags.WasSliding)
					{
						// Set sliding flag to false
						_characterComponents.CharacterFlags.WasSliding = false;
						// Trigger slide event for animator state changes
						_characterComponents.CharacterEvents.OnSlideEvent.Invoke(false);
					}
					// Manage gameObject Collider2Ds
					ManageCollider2Ds(_characterComponents.MainCapsuleCollider2D);
					// Apply run speed and fixed delta time to move parameter
					move *= _characterComponents.CharacterParams.RunSpeed * Time.fixedDeltaTime;
					// Move the character by finding the target velocity
					Vector3 l_targetVelocity = new Vector2(move * 10.0f, _characterComponents.Rigidbody2D.velocity.y);
					// And then smoothing it out and applying it to the character
					_characterComponents.Rigidbody2D.velocity =
                        Vector3
                        .SmoothDamp(_characterComponents.Rigidbody2D.velocity,
                            l_targetVelocity,
                            ref _velocity,
                            _characterComponents.CharacterParams.MovementSmoothing);
					// If the input is moving the player right and the player is facing left...
					if (move > 0.0f && !_characterComponents.CharacterFlags.IsFacingRight)
					{
						// ... flip the player.
						FlipFacingDirection();
					}
					// Otherwise if the input is moving the player left and the player is facing right...
					else if (move < 0.0f && _characterComponents.CharacterFlags.IsFacingRight)
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
					if (_characterComponents.CharacterFlags.WasSliding)
					{
						// Set sliding flag to false
						_characterComponents.CharacterFlags.WasSliding = false;
						// Trigger slide event for animator state changes
						_characterComponents.CharacterEvents.OnSlideEvent.Invoke(false);
						// Manage gameObject Collider2Ds
						ManageCollider2Ds(_characterComponents.MainCapsuleCollider2D);
					}
					// Add a vertical force to the player
					_characterComponents.Rigidbody2D.AddForce(new Vector2(0.0f, _characterComponents.CharacterParams.JumpForce));
				}
				#endregion
			}
            #endregion
            #region Not grounded
            else
            {
				if (_characterComponents.CharacterParams.AirControl)
                {

                }

                #region Double jump
                if (jump && !_characterComponents.CharacterFlags.HasDoubleJumped && !_characterComponents.CharacterFlags.WasGliding)
                {
					// Put double jump flag to true
					_characterComponents.CharacterFlags.HasDoubleJumped = true;
					// Trigger double jump event for animator state changes
					_characterComponents.CharacterEvents.OnDoubleJumpEvent.Invoke();
					// Zero out y velocity before applying jump force
					_characterComponents.Rigidbody2D.velocity = new Vector2(_characterComponents.Rigidbody2D.velocity.x, 0.0f);
					// Add a vertical force to the player
					_characterComponents.Rigidbody2D.AddForce(new Vector2(0.0f, _characterComponents.CharacterParams.JumpForce));
				}
                #endregion

                #region Glide
                if (glide)
                {
                    if (!_characterComponents.CharacterFlags.WasGliding)
                    {
						// Zero out y velocity before applying glide
						_characterComponents.Rigidbody2D.velocity = new Vector2(_characterComponents.Rigidbody2D.velocity.x, 0.0f);
						// Manage gameObject Collider2Ds
						ManageCollider2Ds(_characterComponents.GlideCapsuleCollider2D);
					}
					// Put glide flag to true
					_characterComponents.CharacterFlags.WasGliding = true;
					// Trigger glide event for animator state changes
					_characterComponents.CharacterEvents.OnGlideEvent.Invoke(true);
					// Apply smooth transition to player glide velocity
					_characterComponents.Rigidbody2D.velocity =
                        Vector3
                        .SmoothDamp(_characterComponents.Rigidbody2D.velocity,
                            new Vector3(_characterComponents.Rigidbody2D.velocity.x, _characterComponents.CharacterParams.GlideSpeed, 0.0f),
                            ref _velocity,
                            _characterComponents.CharacterParams.MovementSmoothing);
				}
                else
                {
					// Put glide flag to false
					_characterComponents.CharacterFlags.WasGliding = false;
					// Manage gameObject Collider2Ds
					ManageCollider2Ds(_characterComponents.MainCapsuleCollider2D);
					// Trigger glide event for animator state changes
					_characterComponents.CharacterEvents.OnGlideEvent.Invoke(false);
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
			_characterComponents.CharacterFlags.IsFacingRight = !_characterComponents.CharacterFlags.IsFacingRight;
			// Flip player sprite in X axis
			_characterComponents.SpriteRenderer.flipX = !_characterComponents.SpriteRenderer.flipX;
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
			throw new NotImplementedException("Implement enemy check and damage");
        }

        /// <summary>
        ///     Gets called when attack animation ends, sets "is attacking" flag to false
        /// </summary>
        public void OnAttackEndAnimationEvent()
        {
			// Set "is attacking" flag to false
			_characterComponents.CharacterFlags.IsAttacking = false;
			// Triggers attack end event for character input control states
			_characterComponents.CharacterEvents.OnAttackEndEvent.Invoke();
		}
	}
}
