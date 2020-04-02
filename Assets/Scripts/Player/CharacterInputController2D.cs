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
		private CharacterController2D _characterController2D;       // CharacterController2D component to control character physics
		[SerializeField]
		private CharacterFlags _characterFlags;                     // CharacterFlags component to control character states
		[SerializeField]
		private Animator _animator;                                 // Animator component for setting player animation transitions

		private IGameController _gameController;                    // IGameController component for player input
		private ControlFlags _controlFlags;                         // Control flags grouped in a class
		private bool _wasPaused = false;                            // Flag for game paused in previous frame

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
            if (_characterFlags.IsDead)
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
			_animator.SetFloat("PlayerSpeed", Mathf.Abs(_controlFlags.HorizontalMove));

			// Player jump
			if (_gameController.PlayerJump())
			{
				_controlFlags.Jump = true;
			}

			// Player slide
			if (_gameController.PlayerSliding())
			{
				_controlFlags.Slide = true;
			}
			else if (_gameController.PlayerQuitSliding())
			{
				_controlFlags.Slide = false;
			}

			// Player glide
			if (_gameController.PlayerGliding())
			{
				_controlFlags.Glide = true;
			}
			else if (_gameController.PlayerQuitGliding())
			{
				_controlFlags.Glide = false;
			}

            // Player attack
            if (_gameController.PlayerAttack() &&
				_characterFlags.IsGrounded &&
                !_characterFlags.IsAttacking &&
				!_characterFlags.IsThrowing &&
				!_characterFlags.WasSliding)
            {
				_controlFlags.Attack = true;
				// Animator player attack parameter setting
				_animator.SetBool("PlayerAttack", true);
            }

			// Player throw
			if (_gameController.PlayerThrow() &&
				_characterFlags.IsGrounded &&
				!_characterFlags.IsAttacking &&
                !_characterFlags.IsThrowing &&
				!_characterFlags.WasSliding)
			{
				_controlFlags.Throw = true;
				// Animator player throw parameter setting
				_animator.SetBool("PlayerThrow", true);
			}
		}
		#endregion

		#region FixedUpdate
		// Update is called once per frame
		private void FixedUpdate ()
		{
			_characterController2D.Control(_controlFlags);
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
			_animator.SetBool("PlayerIsGrounded", playerIsGrounded);

            if (playerIsGrounded)
            {
				// Animator player double jump parameter setting
				_animator.SetBool("PlayerDoubleJump", false);
				// Animator player glide parameter setting
				_animator.SetBool("PlayerGlide", false);
			}
		}

		/// <summary>
		///     Added to OnSlideEvent from CharacterController2D script
		/// </summary>
		/// <param name="playerSliding"><code>true</code> if player is sliding, otherwise <code>false</code></param>
		public void OnSliding(bool playerSliding)
        {
            // Animator player slide parameter setting
			_animator.SetBool("PlayerSlide", playerSliding);
        }

        /// <summary>
        ///     Added to OnDoubleJumpEvent from CharacterController2D
        /// </summary>
        public void OnDoubleJump()
        {
			// Animator player double jump parameter setting
			_animator.SetBool("PlayerDoubleJump", true);
		}

		/// <summary>
		///     Added to OnGlideEvent from CharacterController2D script
		/// </summary>
		/// <param name="playerGliding"><code>true</code> if player is gliding, otherwise <code>false</code></param>
		public void OnGliding(bool playerGliding)
		{
			// Animator player slide parameter setting
			_animator.SetBool("PlayerGlide", playerGliding);
		}

        /// <summary>
        ///     Added to OnAttackEndEvent from CharacterController2D script
        /// </summary>
        public void OnAttackEnd()
		{
			// When attack animation ends, put attack flag back to false
			_controlFlags.Attack = false;
			// Animator player attack parameter setting
			_animator.SetBool("PlayerAttack", false);
        }

		/// <summary>
		///     Added to OnThrowEndEvent from CharacterController2D script
		/// </summary>
		public void OnThrowEnd()
		{
			// When throw animation ends, put throw flag back to false
			_controlFlags.Throw = false;
			// Animator player attack parameter setting
			_animator.SetBool("PlayerThrow", false);
		}
		#endregion
	}
}
