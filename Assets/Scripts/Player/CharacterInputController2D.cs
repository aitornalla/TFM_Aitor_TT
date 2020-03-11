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

		#region Start
		// Use this for initialization
		void Start ()
		{
			_gameController = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<IGameController> ();
		}
		#endregion

		#region Update
		// Update is called once per frame
		void Update ()
		{
			_horizontalMove = 0.0f;

			if (_gameController.PlayerLeft ())
			{
				_horizontalMove = -1.0f;
			}
			else if (_gameController.PlayerRight ())
			{
				//if (_horizontalMove == -1.0f)
				//{
				//	_horizontalMove = 0.0f;
				//}
				//else
				//{
					_horizontalMove = 1.0f;
				//}
			}
		}
		#endregion

		#region FixedUpdate
		// Update is called once per frame
		void FixedUpdate ()
		{
			_characterController2D.Move (_horizontalMove, false, false);
		}
		#endregion
	}
}
