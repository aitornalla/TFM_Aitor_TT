using Assets.Scripts.GameController.PlatformControllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameController
{
	public class PS3GameController : MonoBehaviour, IGameController
    {
		private PS3Controller _controller = null;

		#region Properties
		//public PS3Controller Controller { get { return _controller; } }
		#endregion

        #region Awake
		void Awake () {

			_controller = gameObject.AddComponent<PS3Controller>();

		}
		#endregion
    }
}
