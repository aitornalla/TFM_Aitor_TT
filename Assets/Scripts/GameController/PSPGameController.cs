using Assets.Scripts.GameController.PlatformControllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameController
{
	public sealed class PSPGameController : MonoBehaviour//, IGameController
	{
		private PSPController _controller = null;

		#region Properties
		//public PSPController Controller { get { return _controller; } }
		#endregion

		#region Awake
		void Awake () {

			_controller = gameObject.AddComponent<PSPController>();

		}
		#endregion
	}
}
