using System;
using UnityEngine;

namespace Assets.Scripts.Player
{
	public sealed class CharacterStateGrounded : ICharacterStateMachine
	{
		private CharacterComponents _characterComponents = null;
		private Collider2D[] _collider2DArrary = null;
		private Vector3 _velocity = Vector3.zero;

		public CharacterStateGrounded(CharacterComponents characterComponents, Collider2D[] collider2DArrary, Vector3 velocity)
		{
			_characterComponents = characterComponents;
			_collider2DArrary = collider2DArrary;
			_velocity = velocity;
		}

		public void StateControl(ControlFlags controlFlags)
		{
            // If player is attacking or throwing, control of the player is disabled
            if (_characterComponents.CharacterFlags.IsAttacking ||
                _characterComponents.CharacterFlags.IsThrowing)
                return;

            #region Attack
            if (controlFlags.Attack &&
                !_characterComponents.CharacterFlags.IsAttacking &&
                !_characterComponents.CharacterFlags.IsThrowing)
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
                CharacterController2D.ManageCollider2Ds(_collider2DArrary, _characterComponents.AttackCapsuleCollider2D);
                // Set "is attacking" flag to true
                _characterComponents.CharacterFlags.IsAttacking = true;

                return;
            }
            #endregion
            #region Throw
            if (controlFlags.Throw &&
                !_characterComponents.CharacterFlags.IsAttacking &&
                !_characterComponents.CharacterFlags.IsThrowing)
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
                CharacterController2D.ManageCollider2Ds(_collider2DArrary, _characterComponents.ThrowCapsuleCollider2D);
                // Set "is throwing" flag to true
                _characterComponents.CharacterFlags.IsThrowing = true;

                return;
            }
            #endregion
            #region Slide
            if (controlFlags.Slide)
            {
                if (!_characterComponents.CharacterFlags.WasSliding)
                {
                    // Set sliding flag to true
                    _characterComponents.CharacterFlags.WasSliding = true;
                    // Trigger slide event for animator state changes
                    _characterComponents.CharacterEvents.OnSlideEvent.Invoke(true);
                    // Manage gameObject Collider2Ds
                    CharacterController2D.ManageCollider2Ds(_collider2DArrary, _characterComponents.SlideCapsuleCollider2D);

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
                CharacterController2D.ManageCollider2Ds(_collider2DArrary, _characterComponents.MainCapsuleCollider2D);
                // Apply run speed and fixed delta time to move parameter
                float move = controlFlags.HorizontalMove * _characterComponents.CharacterParams.RunSpeed * Time.fixedDeltaTime;
                // Move the character by finding the target velocity
                Vector3 l_targetVelocity = new Vector2(move * 10.0f, _characterComponents.Rigidbody2D.velocity.y);
                // And then smoothing it out and applying it to the character
                _characterComponents.Rigidbody2D.velocity =
                             Vector3
                             .SmoothDamp(_characterComponents.Rigidbody2D.velocity,
                                 l_targetVelocity,
                                 ref _velocity,
                                 _characterComponents.CharacterParams.MovementSmoothing);
                // Manage player facing direction and relevant components
                CharacterController2D.ManagePlayerFacing(_characterComponents, _collider2DArrary, move);
            }
            #endregion
            #region Player jump
            if (controlFlags.Jump)
            {
                // Check if player was sliding to put back main settings
                if (_characterComponents.CharacterFlags.WasSliding)
                {
                    // Set sliding flag to false
                    _characterComponents.CharacterFlags.WasSliding = false;
                    // Trigger slide event for animator state changes
                    _characterComponents.CharacterEvents.OnSlideEvent.Invoke(false);
                    // Manage gameObject Collider2Ds
                    CharacterController2D.ManageCollider2Ds(_collider2DArrary, _characterComponents.MainCapsuleCollider2D);
                }
                // Add a vertical force to the player
                _characterComponents.Rigidbody2D.AddForce(new Vector2(0.0f, _characterComponents.CharacterParams.JumpForce));
            }
            #endregion
        }
    }
}
