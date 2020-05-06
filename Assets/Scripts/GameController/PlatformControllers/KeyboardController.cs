using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameController.PlatformControllers
{
	public sealed class KeyboardController : MonoBehaviour
    {
		public bool IsDebugEnabled { get; set; }

		#region Keyboard Attack key
		public bool GetKeyAttackDown ()
		{
			if (Input.GetButtonDown ("KB_Attack_Key"))
			{
				if (IsDebugEnabled) Debug.Log ("Keyboard attack key down");

				return true;
			}

			return false;
		}
		#endregion

		#region Keyboard Throw key
		public bool GetKeyThrowDown()
		{
			if (Input.GetButtonDown("KB_Throw_Key"))
			{
				if (IsDebugEnabled) Debug.Log("Keyboard throw key down");

				return true;
			}

			return false;
		}
		#endregion

		#region Keyboard Jump key
		public bool GetKeyJumpDown ()
		{
			if (Input.GetButtonDown ("KB_Jump_Key"))
			{
				if (IsDebugEnabled) Debug.Log ("Keyboard jump key down");

				return true;
			}

			return false;
		}
		#endregion

		#region Keyboard left key
		public bool GetKeyLeftDown ()
		{
			if (Input.GetButtonDown ("KB_Left_Key"))
			{
				if (IsDebugEnabled) Debug.Log ("Keyboard left key down");

				return true;
			}

			return false;
		}

		public bool GetKeyLeft ()
		{
			if (Input.GetButton ("KB_Left_Key"))
			{
				if (IsDebugEnabled) Debug.Log ("Keyboard left key");

				return true;
			}

			return false;
		}

		public bool GetKeyLeftUp ()
		{
			if (Input.GetButtonUp ("KB_Left_Key"))
			{
				if (IsDebugEnabled) Debug.Log ("Keyboard left key up");

				return true;
			}

			return false;
		}
		#endregion

		#region Keyboard right key
		public bool GetKeyRightDown ()
		{
			if (Input.GetButtonDown ("KB_Right_Key"))
			{
				if (IsDebugEnabled) Debug.Log ("Keyboard right key down");

				return true;
			}

			return false;
		}

		public bool GetKeyRight ()
		{
			if (Input.GetButton ("KB_Right_Key"))
			{
				if (IsDebugEnabled) Debug.Log ("Keyboard right key");

				return true;
			}

			return false;
		}

		public bool GetKeyRightUp ()
		{
			if (Input.GetButtonUp ("KB_Right_Key"))
			{
				if (IsDebugEnabled) Debug.Log ("Keyboard right key up");

				return true;
			}

			return false;
		}
		#endregion

		#region Keyboard up key
		public bool GetKeyUpDown ()
		{
			if (Input.GetButtonDown ("KB_Up_Key"))
			{
				if (IsDebugEnabled) Debug.Log ("Keyboard up key down");

				return true;
			}

			return false;
		}

		public bool GetKeyUp ()
		{
			if (Input.GetButton ("KB_Up_Key"))
			{
				if (IsDebugEnabled) Debug.Log ("Keyboard up key");

				return true;
			}

			return false;
		}

		public bool GetKeyUpUp ()
		{
			if (Input.GetButtonUp ("KB_Up_Key"))
			{
				if (IsDebugEnabled) Debug.Log ("Keyboard up key up");

				return true;
			}

			return false;
		}
		#endregion

		#region Keyboard down key
		public bool GetKeyDownDown ()
		{
			if (Input.GetButtonDown ("KB_Down_Key"))
			{
				if (IsDebugEnabled) Debug.Log ("Keyboard down key down");

				return true;
			}

			return false;
		}

		public bool GetKeyDown ()
		{
			if (Input.GetButton ("KB_Down_Key"))
			{
				if (IsDebugEnabled) Debug.Log ("Keyboard down key");

				return true;
			}

			return false;
		}

		public bool GetKeyDownUp ()
		{
			if (Input.GetButtonUp ("KB_Down_Key"))
			{
				if (IsDebugEnabled) Debug.Log ("Keyboard down key up");

				return true;
			}

			return false;
		}
        #endregion

        #region Keyboard slide key
        public bool GetKeySlideDown ()
        {
			if (Input.GetButtonDown("KB_Slide_Key"))
			{
				if (IsDebugEnabled) Debug.Log("Keyboard slide key down");

				return true;
			}

			return false;
		}

        public bool GetKeySlideUp ()
        {
			if (Input.GetButtonUp("KB_Slide_Key"))
			{
				if (IsDebugEnabled) Debug.Log("Keyboard slide key up");

				return true;
			}

			return false;
		}
		#endregion

		#region Keyboard glide key
		public bool GetKeyGlideDown()
		{
			if (Input.GetButtonDown("KB_Glide_Key"))
			{
				if (IsDebugEnabled) Debug.Log("Keyboard glide key down");

				return true;
			}

			return false;
		}

		public bool GetKeyGlideUp()
		{
			if (Input.GetButtonUp("KB_Glide_Key"))
			{
				if (IsDebugEnabled) Debug.Log("Keyboard glide key up");

				return true;
			}

			return false;
		}
		#endregion

		#region Keyboard pause key
		public bool GetKeyPauseDown ()
		{
			if (Input.GetButtonDown ("KB_Pause_Key"))
			{
				if (IsDebugEnabled) Debug.Log ("Keyboard pause key down");

				return true;
			}

			return false;
		}
		#endregion

		#region Keyboard accept key
		public bool GetKeyAcceptDown ()
		{
			if (Input.GetButtonDown ("KB_Accept_Key"))
			{
				if (IsDebugEnabled) Debug.Log ("Keyboard accept key down");

				return true;
			}

			return false;
		}
		#endregion

		#region Keyboard cancel key
		public bool GetKeyCancelDown ()
		{
			if (Input.GetButtonDown ("KB_Cancel_Key"))
			{
				if (IsDebugEnabled) Debug.Log ("Keyboard cancel key down");

				return true;
			}

			return false;
		}
		#endregion

		#region Keyboard option key
		public bool GetKeyOptionDown()
		{
			if (Input.GetButtonDown("KB_Option_Key"))
			{
				if (IsDebugEnabled) Debug.Log("Keyboard option key down");

				return true;
			}

			return false;
		}
		#endregion
	}
}
