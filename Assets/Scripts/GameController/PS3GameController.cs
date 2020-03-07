using Assets.Scripts.GameController.PlatformControllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameController
{
	/// <summary>
	/// 	PS3 controller game implementation
	/// </summary>
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

		#region IGameController implementation
		public void ControllerDebug (bool enable)
		{
			_controller.IsDebugEnabled = enable;
		}

		public bool PlayerAttack ()
		{
			return _controller.PS3SquareButtonDown ();
		}

		public bool PlayerJump ()
		{
			return _controller.PS3CrossButtonDown ();
		}

		public bool PlayerLeft ()
		{
			return _controller.PS3PadLeftButtonDown () || _controller.PS3PadLeftButton ();
		}

		public bool PlayerRight ()
		{
			return _controller.PS3PadRightButtonDown () || _controller.PS3PadRightButton ();
		}

		public bool PlayerUp ()
		{
			return _controller.PS3PadUpButtonDown () || _controller.PS3PadUpButton ();
		}

		public bool PlayerDown ()
		{
			return _controller.PS3PadDownButtonDown () || _controller.PS3PadDownButton ();
		}

		public bool Pause ()
		{
			return _controller.PS3StartButtonDown ();
		}

		public bool Accept ()
		{
			return _controller.PS3CrossButtonDown ();
		}

		public bool Cancel ()
		{
			return _controller.PS3CircleButtonDown ();
		}

		public bool MenuLeft ()
		{
			return _controller.PS3PadLeftButtonDown ();
		}

		public bool MenuRight ()
		{
			return _controller.PS3PadRightButtonDown ();
		}

		public bool MenuUp ()
		{
			return _controller.PS3PadUpButtonDown ();
		}

		public bool MenuDown ()
		{
			return _controller.PS3PadDownButtonDown ();
		}
		#endregion
    }
}
