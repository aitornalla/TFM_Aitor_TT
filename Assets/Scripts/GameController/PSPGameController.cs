using Assets.Scripts.GameController.PlatformControllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameController
{
	public sealed class PSPGameController : MonoBehaviour, IGameController
	{
		private PSPController _controller = null;

		#region Properties
		//public PSPController Controller { get { return _controller; } }
		#endregion

		#region Awake
		private void Awake ()
        {
			_controller = gameObject.AddComponent<PSPController>();
		}
		#endregion

		#region IGameController implementation
		public void ControllerDebug(bool enable)
		{
			_controller.IsDebugEnabled = enable;
		}

		#region Player
		public bool PlayerAttack()
		{
			return _controller.PSPSquareButtonDown();
		}

		public bool PlayerThrow()
		{
			return _controller.PSPTriangleButtonDown();
		}

		public bool PlayerJump()
		{
			return _controller.PSPCrossButtonDown();
		}

		public bool PlayerLeft()
		{
			return _controller.PSPDPadLeftAsButtonDown() || _controller.PSPDPadLeftAsButton();
		}

		public bool PlayerRight()
		{
			return _controller.PSPDPadRightAsButtonDown() || _controller.PSPDPadRightAsButton();
		}

		public bool PlayerUp()
		{
			return _controller.PSPDPadUpAsButtonDown() || _controller.PSPDPadUpAsButton();
		}

		public bool PlayerDown()
		{
			return _controller.PSPDPadDownAsButtonDown() || _controller.PSPDPadDownAsButton();
		}

		public bool PlayerSliding()
		{
			return _controller.PSPRButtonDown();
		}

		public bool PlayerQuitSliding()
		{
			return _controller.PSPRButtonUp();
		}

		public bool PlayerGliding()
		{
			return _controller.PSPLButtonDown();
		}

		public bool PlayerQuitGliding()
		{
			return _controller.PSPLButtonUp();
		}
		#endregion

		#region Options
		public bool Pause()
		{
			return _controller.PSPStartButtonDown();
		}

		public bool Accept()
		{
			return _controller.PSPCrossButtonDown();
		}

		public bool Cancel()
		{
			return _controller.PSPCircleButtonDown();
		}

        public bool Option ()
        {
			return _controller.PSPTriangleButtonDown();
        }
		#endregion

		#region Menu
		public bool MenuLeft()
		{
			return _controller.PSPDPadLeftAsButtonDown();
		}

		public bool MenuRight()
		{
			return _controller.PSPDPadRightAsButtonDown();
		}

		public bool MenuUp()
		{
			return _controller.PSPDPadUpAsButtonDown();
		}

		public bool MenuDown()
		{
			return _controller.PSPDPadDownAsButtonDown();
		}
		#endregion
		#endregion
	}
}
