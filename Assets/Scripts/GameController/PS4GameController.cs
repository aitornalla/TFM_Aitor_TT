using Assets.Scripts.GameController.PlatformControllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameController
{
	public sealed class PS4GameController : MonoBehaviour, IGameController
	{
		private PS4Controller _controller = null;

		#region Properties
		//public PS4Controller Controller { get { return _controller; } }
		#endregion

		#region Awake
		private void Awake ()
        {
			_controller = gameObject.AddComponent<PS4Controller>();
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
			return _controller.PS4SquareButtonDown();
		}

		public bool PlayerThrow()
		{
			return _controller.PS4TriangleButtonDown();
		}

		public bool PlayerJump()
		{
			return _controller.PS4CrossButtonDown();
		}

		public bool PlayerLeft()
		{
			return _controller.PS4DPadLeftAsButtonDown() || _controller.PS4DPadLeftAsButton();
		}

		public bool PlayerRight()
		{
			return _controller.PS4DPadRightAsButtonDown() || _controller.PS4DPadRightAsButton();
		}

		public bool PlayerUp()
		{
			return _controller.PS4DPadUpAsButtonDown() || _controller.PS4DPadUpAsButton();
		}

		public bool PlayerDown()
		{
			return _controller.PS4DPadDownAsButtonDown() || _controller.PS4DPadDownAsButton();
		}

		public bool PlayerSliding()
		{
			return _controller.PS4R1ButtonDown();
		}

		public bool PlayerQuitSliding()
		{
			return _controller.PS4R1ButtonUp();
		}

		public bool PlayerGliding()
		{
			return _controller.PS4L1ButtonDown();
		}

		public bool PlayerQuitGliding()
		{
			return _controller.PS4L1ButtonUp();
		}
		#endregion

		#region Options
		public bool Pause()
		{
			return _controller.PS4OptionsButtonDown();
		}

		public bool Accept()
		{
			return _controller.PS4CrossButtonDown();
		}

		public bool Cancel()
		{
			return _controller.PS4CircleButtonDown();
		}

        public bool Option ()
        {
			return _controller.PS4CircleButtonDown();
        }
		#endregion

		#region Menu
		public bool MenuLeft()
		{
			return _controller.PS4DPadLeftAsButtonDown();
		}

		public bool MenuRight()
		{
			return _controller.PS4DPadRightAsButtonDown();
		}

		public bool MenuUp()
		{
			return _controller.PS4DPadUpAsButtonDown();
		}

		public bool MenuDown()
		{
			return _controller.PS4DPadDownAsButtonDown();
		}
		#endregion
		#endregion
	}
}
