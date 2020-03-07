using Assets.Scripts.GameController.PlatformControllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameController
{
	public class KeyboardGameController : MonoBehaviour, IGameController
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

		#region IGameController implementation
		public void ControllerDebug (bool enable)
		{
			_controller.IsDebugEnabled = enable;
		}

		public bool PlayerAttack ()
		{
			return _controller.GetKeyAttackDown ();
		}

		public bool PlayerJump ()
		{
			return _controller.GetKeyJumpDown ();
		}

		public bool PlayerLeft ()
		{
			return _controller.GetKeyLeftDown () || _controller.GetKeyLeft ();
		}

		public bool PlayerRight ()
		{
			return _controller.GetKeyRightDown () || _controller.GetKeyRight ();
		}

		public bool PlayerUp ()
		{
			return _controller.GetKeyUpDown () || _controller.GetKeyUp ();
		}

		public bool PlayerDown ()
		{
			return _controller.GetKeyDownDown () || _controller.GetKeyDown ();
		}

		public bool Pause ()
		{
			return _controller.GetKeyPauseDown ();
		}

		public bool Accept ()
		{
			return _controller.GetKeyAcceptDown ();
		}

		public bool Cancel ()
		{
			return _controller.GetKeyCancelDown ();
		}

		public bool MenuLeft ()
		{
			return _controller.GetKeyLeftDown ();
		}

		public bool MenuRight ()
		{
			return _controller.GetKeyRightDown ();
		}

		public bool MenuUp ()
		{
			return _controller.GetKeyUpDown ();
		}

		public bool MenuDown ()
		{
			return _controller.GetKeyDownDown ();
		}
		#endregion
	}
}
