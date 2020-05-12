using Assets.Scripts.GameController;
using Assets.Scripts.GameManagerController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Player
{
	public sealed class CharacterInputController2D : MonoBehaviour
    {
		[SerializeField]
		private CharacterComponents _characterComponents;                       // CharacterComponents component
		
		private IGameController _gameController;                                // IGameController component for player input
		private ControlFlags _controlFlags;                                     // Control flags grouped in a class
		private bool _wasPaused = false;                                        // Flag for game paused in previous frame

        #region
        public ControlFlags ControlFlags { get { return _controlFlags; } }
        #endregion

        #region Start
        // Use this for initialization
        private void Start ()
		{
			// Get IGameController component from GameManager
			// _gameController = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<IGameController> ();
			_gameController = GameManager.Instance.GameController;
			// Instantiate new object to hold control flags
			_controlFlags = new ControlFlags();
			// Add listener to GameManager pause event
			GameManager.Instance.OnPauseEvent.AddListener(OnPause);
		}
		#endregion

		#region Update
		// Update is called once per frame
		private void Update ()
		{
			// If player is dead control is disabled
			if (_characterComponents.CharacterFlags.IsDead)
            {
				return;
            }

            // If game is paused, return
			if (GameManager.Instance.IsPaused)
            {
				return;
            }

            // Do nothing after unpause first frame to avoid unwanted behaviour
            if (_wasPaused)
            {
                _wasPaused = false;

                return;
            }

            // Player horizontal move
            if (_gameController.PlayerLeft())
			{
				_controlFlags.HorizontalMove = -1.0f;
			}
			else if (_gameController.PlayerRight())
			{
				_controlFlags.HorizontalMove = 1.0f;
			}

			// Animator player speed parameter setting
			//_animator.SetFloat("PlayerSpeed", Mathf.Abs(_controlFlags.HorizontalMove));
			_characterComponents.Animator.SetFloat("PlayerSpeed", Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x));
            // For very low velocities set float to 0.0f
            if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) < 0.1f)
				_characterComponents.Animator.SetFloat("PlayerSpeed", 0.0f);


			// Player jump
			if (_gameController.PlayerJump())
			{
				_controlFlags.Jump = true;
                // Play jump sound
				_characterComponents.CharacterAudio.Jump();
			}

			// Player slide
			if (_gameController.PlayerSliding())
			{
				_controlFlags.Slide = true;
				// Play slide sound
				_characterComponents.CharacterAudio.Slide();
			}
			else if (_gameController.PlayerQuitSliding())
			{
				_controlFlags.Slide = false;
			}

			// Player glide
			if (_gameController.PlayerGliding())
			{
				_controlFlags.Glide = true;
				// Play open parachute sound
				_characterComponents.CharacterAudio.OpenParachute();
			}
			else if (_gameController.PlayerQuitGliding())
			{
				_controlFlags.Glide = false;
			}

            // Player attack
            if (_gameController.PlayerAttack() &&
				_characterComponents.CharacterFlags.IsGrounded &&
                !_characterComponents.CharacterFlags.IsAttacking &&
				!_characterComponents.CharacterFlags.IsThrowing &&
				!_characterComponents.CharacterFlags.WasSliding)
            {
				_controlFlags.Attack = true;
				// Animator player attack parameter setting
				_characterComponents.Animator.SetBool("PlayerAttack", true);
				// Play attack sound
				_characterComponents.CharacterAudio.Attack();
            }

			// Player throw
			if (_gameController.PlayerThrow() &&
				_characterComponents.CharacterFlags.IsGrounded &&
				!_characterComponents.CharacterFlags.IsAttacking &&
                !_characterComponents.CharacterFlags.IsThrowing &&
				!_characterComponents.CharacterFlags.WasSliding)
			{
				_controlFlags.Throw = true;
				// Animator player throw parameter setting
				_characterComponents.Animator.SetBool("PlayerThrow", true);
			}

			// If player control is not allowed then reset control flags
			if (!_characterComponents.CharacterFlags.IsPlayerControlAllowed)
			{
				_controlFlags.ResetFlags();
			}
		}
		#endregion

		#region FixedUpdate
		// Update is called once per frame
		private void FixedUpdate ()
		{
			_characterComponents.CharacterController2D.Control(_controlFlags);
			// Put movement variable back to 0.0f
			_controlFlags.HorizontalMove = 0.0f;
			// Put jump flag back to false
			_controlFlags.Jump = false;
		}
		#endregion

		#region Event Handlers
        /// <summary>
        ///     Added to OnPuaseEvent from GameManager
        /// </summary>
        /// <param name="isPaused">Flag for paused state</param>
        public void OnPause(bool isPaused)
		{
			if (isPaused)
			{
				_wasPaused = true;
			}
			else
			{
				_controlFlags.ResetFlags();
			}
		}

		/// <summary>
		///     Added to OnGroundedEvent from CharacterController2D script
		/// </summary>
		/// <param name="playerIsGrounded"><code>true</code> if player is grounded, otherwise <code>false</code></param>
		public void OnGrounded(bool playerIsGrounded)
        {
			// Animator player grounded parameter setting
			_characterComponents.Animator.SetBool("PlayerIsGrounded", playerIsGrounded);

            if (playerIsGrounded)
            {
				// Animator player double jump parameter setting
				_characterComponents.Animator.SetBool("PlayerDoubleJump", false);
				// Animator player glide parameter setting
				_characterComponents.Animator.SetBool("PlayerGlide", false);
			}
		}

		/// <summary>
		///     Added to OnSlideEvent from CharacterController2D script
		/// </summary>
		/// <param name="playerSliding"><code>true</code> if player is sliding, otherwise <code>false</code></param>
		public void OnSliding(bool playerSliding)
        {
			// Animator player slide parameter setting
			_characterComponents.Animator.SetBool("PlayerSlide", playerSliding);
        }

        /// <summary>
        ///     Added to OnDoubleJumpEvent from CharacterController2D
        /// </summary>
        public void OnDoubleJump()
        {
			// Animator player double jump parameter setting
			_characterComponents.Animator.SetBool("PlayerDoubleJump", true);
		}

		/// <summary>
		///     Added to OnGlideEvent from CharacterController2D script
		/// </summary>
		/// <param name="playerGliding"><code>true</code> if player is gliding, otherwise <code>false</code></param>
		public void OnGliding(bool playerGliding)
		{
			// Animator player slide parameter setting
			_characterComponents.Animator.SetBool("PlayerGlide", playerGliding);
		}

        /// <summary>
        ///     Added to OnAttackEndEvent from CharacterController2D script
        /// </summary>
        public void OnAttackEnd()
		{
			// When attack animation ends, put attack flag back to false
			_controlFlags.Attack = false;
			// Animator player attack parameter setting
			_characterComponents.Animator.SetBool("PlayerAttack", false);
        }

		/// <summary>
		///     Added to OnThrowEndEvent from CharacterController2D script
		/// </summary>
		public void OnThrowEnd()
		{
			// When throw animation ends, put throw flag back to false
			_controlFlags.Throw = false;
			// Animator player attack parameter setting
			_characterComponents.Animator.SetBool("PlayerThrow", false);
		}
		#endregion
	}
}
