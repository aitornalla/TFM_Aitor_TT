using Assets.Scripts.GameController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Player
{
	public sealed class CharacterInputController2D : MonoBehaviour {

		[SerializeField]
		private CharacterController2D _characterController2D;       // CharacterController2D component to control character physics
		[SerializeField]
		private Animator _animator;                                 // Animator component for setting player animation transitions

		private IGameController _gameController;                    // IGameController component for player input

		private float _horizontalMove = 0.0f;                       // Variable to store horizontal move value, range (-1.0f, 1.0f)
		private bool _jump = false;                                 // Variable to store jump value
		private bool _slide = false;                                // Variable to store slide value

		#region Start
		// Use this for initialization
		private void Start ()
		{
            // Get IGameController component from GameManager
			_gameController = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<IGameController> ();
		}
		#endregion

		#region Update
		// Update is called once per frame
		private void Update ()
		{
			// Player horizontal move
            if (_gameController.PlayerLeft())
			{
				_horizontalMove = -1.0f;
			}
			else if (_gameController.PlayerRight())
			{
				_horizontalMove = 1.0f;
			}

			// Animator player speed parameter setting
			_animator.SetFloat("PlayerSpeed", Mathf.Abs(_horizontalMove));

			// Player jump
			if (_gameController.PlayerJump())
			{
				_jump = true;
				// Animator player jump parameter setting
				_animator.SetBool("PlayerJump", true);
			}

			// Player slide
			if (_gameController.PlayerSliding())
			{
				_slide = true;
			}
			else if (_gameController.PlayerQuitSliding())
			{
				_slide = false;
			}
		}
		#endregion

		#region FixedUpdate
		// Update is called once per frame
		private void FixedUpdate ()
		{
			_characterController2D.Move (_horizontalMove, _jump, _slide);
            // Put movement variable back to 0.0f
			_horizontalMove = 0.0f;
            // Put jump variable back to false
			_jump = false;
		}
		#endregion

        /// <summary>
        ///     Added to OnLandEvent from CharacterController2D script
        /// </summary>
        public void OnLanding()
        {
			// Animator player jump parameter setting
			_animator.SetBool("PlayerJump", false);
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
	}
}
