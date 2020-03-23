using System;
using UnityEngine;

namespace Assets.Scripts.Player
{
	public sealed class CharacterStateNotGrounded : ICharacterStateMachine
	{
		private CharacterComponents _characterComponents = null;
		private Collider2D[] _collider2DArrary = null;
		private Vector3 _velocity = Vector3.zero;

		public CharacterStateNotGrounded(CharacterComponents characterComponents, Collider2D[] collider2DArrary, Vector3 velocity)
		{
			_characterComponents = characterComponents;
			_collider2DArrary = collider2DArrary;
			_velocity = velocity;
		}

		public void StateControl(ControlFlags controlFlags)
		{
			#region Not grounded
			if (_characterComponents.CharacterParams.AirControl)
			{

			}

			#region Double jump
			if (controlFlags.Jump && !_characterComponents.CharacterFlags.HasDoubleJumped && !_characterComponents.CharacterFlags.WasGliding)
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
			if (controlFlags.Glide)
			{
				if (!_characterComponents.CharacterFlags.WasGliding)
				{
					// Zero out y velocity before applying glide
					_characterComponents.Rigidbody2D.velocity = new Vector2(_characterComponents.Rigidbody2D.velocity.x, 0.0f);
					// Manage gameObject Collider2Ds
					CharacterController2D.ManageCollider2Ds(_collider2DArrary, _characterComponents.GlideCapsuleCollider2D);
				}
				// Put glide flag to true
				_characterComponents.CharacterFlags.WasGliding = true;
				// Trigger glide event for animator state changes
				_characterComponents.CharacterEvents.OnGlideEvent.Invoke(true);

                // Gliding horizontal movement control
				float l_move = controlFlags.HorizontalMove * _characterComponents.CharacterParams.GlideHorizontalSpeed;
				// Apply smooth transition to player glide horizontal velocity
				_characterComponents.Rigidbody2D.velocity =
					Vector3.SmoothDamp(
						_characterComponents.Rigidbody2D.velocity,
						new Vector3(l_move, _characterComponents.Rigidbody2D.velocity.y, 0.0f),
						ref _velocity,
						_characterComponents.CharacterParams.GlideMovementSmoothing);
				// Manage player facing direction and relevant components
				CharacterController2D.ManagePlayerFacing(_characterComponents, _collider2DArrary, l_move);

                #region GIZ
                // If player is in a glide impulse zone let the area effector apply any forces
                if (_characterComponents.CharacterFlags.IsInGlideImpulseZone)
                {
					// Limit player glide vertical velocity up
					if (_characterComponents.Rigidbody2D.velocity.y >= _characterComponents.CharacterParams.GlideVerticalSpeedUp)
						_characterComponents.Rigidbody2D.velocity =
							new Vector2(
								_characterComponents.Rigidbody2D.velocity.x,
								_characterComponents.CharacterParams.GlideVerticalSpeedUp);

					return;
                }
                #endregion

                // Apply smooth transition to player glide vertical velocity down
                _characterComponents.Rigidbody2D.velocity =
					Vector3
					.SmoothDamp(_characterComponents.Rigidbody2D.velocity,
						new Vector3(_characterComponents.Rigidbody2D.velocity.x, _characterComponents.CharacterParams.GlideVerticalSpeedDown, 0.0f),
						ref _velocity,
						_characterComponents.CharacterParams.MovementSmoothing);
			}
			else
			{
				// Put glide flag to false
				_characterComponents.CharacterFlags.WasGliding = false;
				// Manage gameObject Collider2Ds
				CharacterController2D.ManageCollider2Ds(_collider2DArrary, _characterComponents.MainCapsuleCollider2D);
				// Trigger glide event for animator state changes
				_characterComponents.CharacterEvents.OnGlideEvent.Invoke(false);
			}
			#endregion
			#endregion
		}
	}
}
