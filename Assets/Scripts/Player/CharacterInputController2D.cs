using Assets.Scripts.GameController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Player
{
	public sealed class CharacterInputController2D : MonoBehaviour {

		[SerializeField]
		private CharacterController2D _characterController2D;

		private IGameController _gameController;

		private float _horizontalMove = 0.0f;
		private bool _jump = false;

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
            // Player jump
			_jump = _gameController.PlayerJump();
		}
		#endregion

		#region FixedUpdate
		// Update is called once per frame
		private void FixedUpdate ()
		{
			_characterController2D.Move (_horizontalMove, _jump, false);
            // Put movement variable back to 0.0f
			_horizontalMove = 0.0f;
            // Put jump variable back to false
			_jump = false;
		}
		#endregion
	}
}
