using Assets.Scripts.GameController.PlatformControllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameController
{
	public class PS4GameController : MonoBehaviour//, IGameController
	{
		private PS4Controller _controller = null;

		#region Properties
		//public PS4Controller Controller { get { return _controller; } }
		#endregion

		#region Awake
		void Awake () {

			_controller = gameObject.AddComponent<PS4Controller>();

		}
		#endregion
	}
}
