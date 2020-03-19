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
		#region Character Components
        [SerializeField]
		private CharacterComponents _characterComponents;
        #endregion

        #region Variables
        private Vector3 _velocity = Vector3.zero;       // Velocity Vector3 of the gameObject
		private Collider2D[] _collider2DArrary;         // GameObject Collider2D array
        #endregion

        #region Character State
        private ICharacterStateMachine _characterStateMachine = null;
        #endregion

        #region Awake
        private void Awake()
		{
            // Events initialized in CharacterEvents component

			// Get gameObject Collider2D references
			_collider2DArrary = gameObject.GetComponents<Collider2D>();

            // Instantiate default character state
			_characterStateMachine = new CharacterStateGrounded(_characterComponents, _collider2DArrary, _velocity);
		}
        #endregion

        #region Update
        private void Update()
		{
            
		}
        #endregion

        #region FixedUpdate
        private void FixedUpdate()
		{
            // Save last frame gorunded flag
            bool l_wasGrounded = _characterComponents.CharacterFlags.IsGrounded;
            // Set grounded flag to false
			_characterComponents.CharacterFlags.IsGrounded = false;
			// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
			// This can be done using layers instead but Sample Assets will not overwrite your project settings.
			Collider2D[] colliders =
                Physics2D.
                OverlapCircleAll(_characterComponents.GroundCheck.position, CharacterParams.GroundedRadius, _characterComponents.CharacterParams.GroundLayer);

            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                {
                    _characterComponents.CharacterFlags.IsGrounded = true;
                    _characterComponents.CharacterFlags.HasDoubleJumped = false;
                    _characterComponents.CharacterFlags.WasGliding = false;
                }
            }
            // If previous frame state is different from current frame, then check what state has to be used
            // This way we avoid to constantly instantiate new state classes, we only do it when it is necessary
            if (l_wasGrounded != _characterComponents.CharacterFlags.IsGrounded)
            {
                // If player was grounded and now it is not...
                if (l_wasGrounded && !_characterComponents.CharacterFlags.IsGrounded)
                {
                    _characterStateMachine = new CharacterStateNotGrounded(_characterComponents, _collider2DArrary, _velocity);
                }
                // If player was not grounded and now it is...
                if (!l_wasGrounded && _characterComponents.CharacterFlags.IsGrounded)
                {
                    _characterStateMachine = new CharacterStateGrounded(_characterComponents, _collider2DArrary, _velocity);
                }
                // Trigger grounded event for animator state changes (includes double jump and glide setting)
                _characterComponents.CharacterEvents.OnGroundedEvent.Invoke(_characterComponents.CharacterFlags.IsGrounded);
            }
        }
        #endregion

        #region Move
        public void Control(float move, bool jump, bool slide, bool glide, bool attack, bool throwkunai)
		{
			_characterStateMachine.StateControl(move, jump, slide, glide, attack, throwkunai);

        }
        #endregion

        #region Static Methods
        /// <summary>
        ///     Changes facing right flag and reoffsets gameObject colliders
        /// </summary>
        /// <param name="isFacingRight">Facing right flag</param>
        /// <param name="spriteRenderer"><see cref="SpriteRenderer"/> component of the gameObject</param>
        //private void FlipFacingDirection()
        public static bool FlipFacingDirection(bool isFacingRight, SpriteRenderer spriteRenderer)
		{
			// Switch the way the player is labelled as facing.
			isFacingRight = !isFacingRight;
			// Flip player sprite in X axis
			spriteRenderer.flipX = !spriteRenderer.flipX;
            // Return flag state
            return isFacingRight;
		}

		/// <summary>
		///     Enables one Collider2D and disables the other gameObject Collider2Ds
		/// </summary>
		/// <param name="collider2DArrary"><see cref="Collider2D"/> array component of the gameObject</param>
		/// <param name="col"><see cref="Collider2D"/> to enable</param>
		public static void ManageCollider2Ds(Collider2D[] collider2DArrary, Collider2D col)
		{
			for (int i = 0; i < collider2DArrary.Length; i++)
			{
				collider2DArrary[i].enabled = col.Equals(collider2DArrary[i]) ? true : false;
			}
		}

		/// <summary>
		///     ReOffsets gameObject Collider2Ds when changing facing direction
		/// </summary>
		/// <param name="collider2DArrary"><see cref="Collider2D"/> array component to reoffset</param>
		public static void ReOffsetCollider2Ds(Collider2D[] collider2DArrary)
		{
			Vector2 l_offset = Vector2.zero;

			for (int i = 0; i < collider2DArrary.Length; i++)
			{
				l_offset = collider2DArrary[i].offset;

				collider2DArrary[i].offset = new Vector2(-l_offset.x, l_offset.y);
			}
		}
        #endregion

        #region Animation Event Handlers
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

        /// <summary>
        ///     Gets called when throw animation reaches a keyframe when the kunai should be spawned and thrown
        /// </summary>
        public void OnThrowAnimationEvent()
        {
            throw new NotImplementedException("Implement character kunai throw");
        }

        /// <summary>
        ///     Gets called when throw animation ends, sets "is throwing" flag to false
        /// </summary>
        public void OnThrowEndAnimationEvent()
        {
            // Set "is throwing" flag to false
            _characterComponents.CharacterFlags.IsThrowing = false;
            // Triggers throw end event for character input control states
            _characterComponents.CharacterEvents.OnThrowEndEvent.Invoke();
        }
        #endregion
    }
}
