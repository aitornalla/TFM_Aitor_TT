using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameController.PlatformControllers
{
	/// <summary>
	/// 	PSP controller implementation for all buttons/axis and states: down, up, held down
	/// </summary>
	public sealed class PSPController : MonoBehaviour
	{
		private bool m_IsButtonPressedDPadDown = false;                     // Flag to treat DPad axis as button
		private bool m_WasButtonPressedDPadDown = false;                    // Flag to treat DPad axis as button
		private bool m_IsButtonPressedDPadLeft = false;                     // Flag to treat DPad axis as button
		private bool m_WasButtonPressedDPadLeft = false;                    // Flag to treat DPad axis as button
		private bool m_IsButtonPressedDPadRight = false;                    // Flag to treat DPad axis as button
		private bool m_WasButtonPressedDPadRight = false;                   // Flag to treat DPad axis as button
		private bool m_IsButtonPressedDPadUp = false;                       // Flag to treat DPad axis as button
		private bool m_WasButtonPressedDPadUp = false;                      // Flag to treat DPad axis as button

		private const float PadDeadZone = 0.004f;

		public bool IsDebugEnabled { get; set; }

		private void Update()
		{
			UpdateDPadStateAsButton();
		}

		/// <summary>
		///     Updates DPad states as buttons
		/// </summary>
		private void UpdateDPadStateAsButton()
		{
			// Manage pad as buttons states derived from pad input
			float l_amountH = PSPDPadHorizontalAxis();
			float l_amountV = PSPDPadVerticalAxis();

			// Pass previous button state to was button flag
			m_WasButtonPressedDPadDown = m_IsButtonPressedDPadDown;
			m_WasButtonPressedDPadLeft = m_IsButtonPressedDPadLeft;
			m_WasButtonPressedDPadRight = m_IsButtonPressedDPadRight;
			m_WasButtonPressedDPadUp = m_IsButtonPressedDPadUp;

			if (l_amountH == 0.0f)
			{
				m_IsButtonPressedDPadLeft = false;
				m_IsButtonPressedDPadRight = false;
			}

			if (l_amountH > 0.0f)
			{
				m_IsButtonPressedDPadRight = true;
			}

			if (l_amountH < 0.0f)
			{
				m_IsButtonPressedDPadLeft = true;
			}

			if (l_amountV == 0.0f)
			{
				m_IsButtonPressedDPadDown = false;
				m_IsButtonPressedDPadUp = false;
			}

			if (l_amountV > 0.0f)
			{
				m_IsButtonPressedDPadUp = true;
			}

			if (l_amountV < 0.0f)
			{
				m_IsButtonPressedDPadDown = true;
			}
		}

		#region PSP Triangle button
		public bool PSPTriangleButtonDown()
		{
			if (Input.GetButtonDown("PSP_Triangle_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PSP /\\ button down");

				return true;
			}

			return false;
		}

		public bool PSPTriangleButton()
		{
			if (Input.GetButton("PSP_Triangle_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PSP /\\ button");

				return true;
			}

			return false;
		}

		public bool PSPTriangleButtonUp()
		{
			if (Input.GetButtonUp("PSP_Triangle_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PSP /\\ button up");

				return true;
			}

			return false;
		}
		#endregion

		#region PSP Square button
		public bool PSPSquareButtonDown()
		{
			if (Input.GetButtonDown("PSP_Square_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PSP [] button down");

				return true;
			}

			return false;
		}

		public bool PSPSquareButton()
		{
			if (Input.GetButton("PSP_Square_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PSP [] button");

				return true;
			}

			return false;
		}

		public bool PSPSquareButtonUp()
		{
			if (Input.GetButtonUp("PSP_Square_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PSP [] button up");

				return true;
			}

			return false;
		}
		#endregion

		#region PSP Circle button
		public bool PSPCircleButtonDown()
		{
			if (Input.GetButtonDown("PSP_Circle_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PSP O button down");

				return true;
			}

			return false;
		}

		public bool PSPCircleButton()
		{
			if (Input.GetButton("PSP_Circle_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PSP O button");

				return true;
			}

			return false;
		}

		public bool PSPCircleButtonUp()
		{
			if (Input.GetButtonUp("PSP_Circle_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PSP O button up");

				return true;
			}

			return false;
		}
		#endregion

		#region PSP Cross button
		public bool PSPCrossButtonDown()
		{
			if (Input.GetButtonDown("PSP_Cross_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PSP X button down");

				return true;
			}

			return false;
		}

		public bool PSPCrossButton()
		{
			if (Input.GetButton("PSP_Cross_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PSP X button");

				return true;
			}

			return false;
		}

		public bool PSPCrossButtonUp()
		{
			if (Input.GetButtonUp("PSP_Cross_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PSP X button up");

				return true;
			}

			return false;
		}
		#endregion

		#region PSP L button
		public bool PSPLButtonDown()
		{
			if (Input.GetButtonDown("PSP_L_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PSP L button down");

				return true;
			}

			return false;
		}

		public bool PSPLButton()
		{
			if (Input.GetButton("PSP_L_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PSP L button");

				return true;
			}

			return false;
		}

		public bool PSPLButtonUp()
		{
			if (Input.GetButtonUp("PSP_L_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PSP L button up");

				return true;
			}

			return false;
		}
		#endregion

		#region PSP R button
		public bool PSPRButtonDown()
		{
			if (Input.GetButtonDown("PSP_R_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PSP R button down");

				return true;
			}

			return false;
		}

		public bool PSPRButton()
		{
			if (Input.GetButton("PSP_R_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PSP R button");

				return true;
			}

			return false;
		}

		public bool PSPRButtonUp()
		{
			if (Input.GetButtonUp("PSP_R_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PSP R button up");

				return true;
			}

			return false;
		}
		#endregion

		#region PSP Select button
		public bool PSPSelectButtonDown()
		{
			if (Input.GetButtonDown("PSP_Select_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PSP Select button down");

				return true;
			}

			return false;
		}

		public bool PSPSelectButton()
		{
			if (Input.GetButton("PSP_Select_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PSP Select button");

				return true;
			}

			return false;
		}

		public bool PSPSelectButtonUp()
		{
			if (Input.GetButtonUp("PSP_Select_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PSP Select button up");

				return true;
			}

			return false;
		}
		#endregion

		#region PSP Start button
		public bool PSPStartButtonDown()
		{
			if (Input.GetButtonDown("PSP_Start_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PSP Start button down");

				return true;
			}

			return false;
		}

		public bool PSPStartButton()
		{
			if (Input.GetButton("PSP_Start_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PSP Start button");

				return true;
			}

			return false;
		}

		public bool PSPStartButtonUp()
		{
			if (Input.GetButtonUp("PSP_Start_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PSP Start button up");

				return true;
			}

			return false;
		}
		#endregion

		#region PSP Home button
		public bool PSPHomeButtonDown()
		{
			if (Input.GetButtonDown("PSP_Home_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PSP Home button down");

				return true;
			}

			return false;
		}

		public bool PSPHomeButton()
		{
			if (Input.GetButton("PSP_Home_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PSP Home button");

				return true;
			}

			return false;
		}

		public bool PSPHomeButtonUp()
		{
			if (Input.GetButtonUp("PSP_Home_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PSP Home button up");

				return true;
			}

			return false;
		}
		#endregion

		#region PSP Screen button
		public bool PSPScreenButtonDown()
		{
			if (Input.GetButtonDown("PSP_Screen_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PSP Screen button down");

				return true;
			}

			return false;
		}

		public bool PSPScreenButton()
		{
			if (Input.GetButton("PSP_Screen_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PSP Screen button");

				return true;
			}

			return false;
		}

		public bool PSPScreenButtonUp()
		{
			if (Input.GetButtonUp("PSP_Screen_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PSP Screen button up");

				return true;
			}

			return false;
		}
		#endregion

		#region PSP Volume Down button
		public bool PSPVolumeDownButtonDown()
		{
			if (Input.GetButtonDown("PSP_VolumeDown_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PSP VolumeDown button down");

				return true;
			}

			return false;
		}

		public bool PSPVolumeDownButton()
		{
			if (Input.GetButton("PSP_VolumeDown_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PSP VolumeDown button");

				return true;
			}

			return false;
		}

		public bool PSPVolumeDownButtonUp()
		{
			if (Input.GetButtonUp("PSP_VolumeDown_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PSP VolumeDown button up");

				return true;
			}

			return false;
		}
		#endregion

		#region PSP Volume Up button
		public bool PSPVolumeUpButtonDown()
		{
			if (Input.GetButtonDown("PSP_VolumeUp_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PSP VolumeUp button down");

				return true;
			}

			return false;
		}

		public bool PSPVolumeUpButton()
		{
			if (Input.GetButton("PSP_VolumeUp_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PSP VolumeUp button");

				return true;
			}

			return false;
		}

		public bool PSPVolumeUpButtonUp()
		{
			if (Input.GetButtonUp("PSP_VolumeUp_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PSP VolumeUp button up");

				return true;
			}

			return false;
		}
		#endregion

		#region PSP Direction pad horizontal axis
		public float PSPDPadHorizontalAxis()
		{
			float l_amount = Input.GetAxis("PSP_DPad_Horizontal");

			l_amount = (Mathf.Abs(l_amount) > PadDeadZone) ? l_amount : 0.0f;

			if (IsDebugEnabled && l_amount != 0.0f)
				Debug.Log("Horizontal DPad: " + l_amount.ToString());

			return l_amount;
		}
		#endregion

		#region PSP Direction pad vertical axis
		public float PSPDPadVerticalAxis()
		{
			float l_amount = Input.GetAxis("PSP_DPad_Vertical");

			l_amount = (Mathf.Abs(l_amount) > PadDeadZone) ? l_amount : 0.0f;

			if (IsDebugEnabled && l_amount != 0.0f)
				Debug.Log("Vertical DPad: " + l_amount.ToString());

			return l_amount;
		}
		#endregion

		#region PSP DPad Up as button
		public bool PSPDPadUpAsButtonDown()
		{
			if (!m_WasButtonPressedDPadUp && m_IsButtonPressedDPadUp)
			{
				if (IsDebugEnabled) Debug.Log("PSP DPad Up button down");

				return true;
			}

			return false;
		}

		public bool PSPDPadUpAsButton()
		{
			if (m_WasButtonPressedDPadUp && m_IsButtonPressedDPadUp)
			{
				if (IsDebugEnabled) Debug.Log("PSP DPad Up button");

				return true;
			}

			return false;
		}

		public bool PSPDPadUpAsButtonUp()
		{
			if (m_WasButtonPressedDPadUp && !m_IsButtonPressedDPadUp)
			{
				if (IsDebugEnabled) Debug.Log("PSP DPad Up button up");

				return true;
			}

			return false;
		}
		#endregion

		#region PSP DPad Down as button
		public bool PSPDPadDownAsButtonDown()
		{
			if (!m_WasButtonPressedDPadDown && m_IsButtonPressedDPadDown)
			{
				if (IsDebugEnabled) Debug.Log("PSP DPad Down button down");

				return true;
			}

			return false;
		}

		public bool PSPDPadDownAsButton()
		{
			if (m_WasButtonPressedDPadDown && m_IsButtonPressedDPadDown)
			{
				if (IsDebugEnabled) Debug.Log("PSP DPad Down button");

				return true;
			}

			return false;
		}

		public bool PSPDPadDownAsButtonUp()
		{
			if (m_WasButtonPressedDPadDown && !m_IsButtonPressedDPadDown)
			{
				if (IsDebugEnabled) Debug.Log("PSP DPad Down button up");

				return true;
			}

			return false;
		}
		#endregion

		#region PSP DPad Left as button
		public bool PSPDPadLeftAsButtonDown()
		{
			if (!m_WasButtonPressedDPadLeft && m_IsButtonPressedDPadLeft)
			{
				if (IsDebugEnabled) Debug.Log("PSP DPad Left button down");

				return true;
			}

			return false;
		}

		public bool PSPDPadLeftAsButton()
		{
			if (m_WasButtonPressedDPadLeft && m_IsButtonPressedDPadLeft)
			{
				if (IsDebugEnabled) Debug.Log("PSP DPad Left button");

				return true;
			}

			return false;
		}

		public bool PSPDPadLeftAsButtonUp()
		{
			if (m_WasButtonPressedDPadLeft && !m_IsButtonPressedDPadLeft)
			{
				if (IsDebugEnabled) Debug.Log("PSP DPad Left button up");

				return true;
			}

			return false;
		}
		#endregion

		#region PSP DPad Right as button
		public bool PSPDPadRightAsButtonDown()
		{
			if (!m_WasButtonPressedDPadRight && m_IsButtonPressedDPadRight)
			{
				if (IsDebugEnabled) Debug.Log("PSP DPad Right button down");

				return true;
			}

			return false;
		}

		public bool PSPDPadRightAsButton()
		{
			if (m_WasButtonPressedDPadRight && m_IsButtonPressedDPadRight)
			{
				if (IsDebugEnabled) Debug.Log("PSP DPad Right button");

				return true;
			}

			return false;
		}

		public bool PSPDPadRightAsButtonUp()
		{
			if (m_WasButtonPressedDPadRight && !m_IsButtonPressedDPadRight)
			{
				if (IsDebugEnabled) Debug.Log("PSP DPad Right button up");

				return true;
			}

			return false;
		}
		#endregion

		#region PSP joystick horizontal axis
		//public float PSPJoystickHorizontalAxis()
		//{
		//	float l_amount = Input.GetAxis("PSP_Joystick_Horizontal");

		//	if (IsDebugEnabled && l_amount != 0.0f)
		//		Debug.Log("Horizontal joystick: " + l_amount.ToString());

		//	return l_amount;
		//}
		#endregion

		#region PSP joystick vertical axis
		//public float PSPJoystickVerticalAxis()
		//{
		//	float l_amount = Input.GetAxis("PSP_Joystick_Vertical");

		//	if (IsDebugEnabled && l_amount != 0.0f)
		//		Debug.Log("Vertical joystick: " + l_amount.ToString());

		//	return l_amount;
		//}
		#endregion
	}
}
