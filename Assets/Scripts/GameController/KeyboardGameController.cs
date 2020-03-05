using Assets.Scripts.GameController.PlatformControllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameController
{
	public class KeyboardGameController : MonoBehaviour//, IGameController
	{
		private KeyboardController _controller = null;

		#region Properties
		//public KeyboardController Controller { get { return _controller; } }
		#endregion

		#region Awake
		void Awake () {

			_controller = gameObject.AddComponent<KeyboardController>();

		}
		#endregion
	}
}
