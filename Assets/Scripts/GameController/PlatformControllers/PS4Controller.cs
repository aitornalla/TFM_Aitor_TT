using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameController.PlatformControllers
{
	/// <summary>
	/// 	PS4 controller implementation for all buttons/axis and states: down, up, held down
	/// </summary>
	public sealed class PS4Controller : MonoBehaviour
	{
		private bool m_IsButtonPressedDPadDown = false;                     // Flag to treat DPad axis as button
		private bool m_WasButtonPressedDPadDown = false;                    // Flag to treat DPad axis as button
		private bool m_IsButtonPressedDPadLeft = false;                     // Flag to treat DPad axis as button
		private bool m_WasButtonPressedDPadLeft = false;                    // Flag to treat DPad axis as button
		private bool m_IsButtonPressedDPadRight = false;                    // Flag to treat DPad axis as button
		private bool m_WasButtonPressedDPadRight = false;                   // Flag to treat DPad axis as button
		private bool m_IsButtonPressedDPadUp = false;                       // Flag to treat DPad axis as button
		private bool m_WasButtonPressedDPadUp = false;                      // Flag to treat DPad axis as button

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
			float l_amountH = PS4DPadHorizontalAxis();
			float l_amountV = PS4DPadVerticalAxis();

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

        #region PS4 Triangle button
        public bool PS4TriangleButtonDown()
		{
			if (Input.GetButtonDown("PS4_Triangle_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PS4 /\\ button down");

				return true;
			}

			return false;
		}

		public bool PS4TriangleButton()
		{
			if (Input.GetButton("PS4_Triangle_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PS4 /\\ button");

				return true;
			}

			return false;
		}

		public bool PS4TriangleButtonUp()
		{
			if (Input.GetButtonUp("PS4_Triangle_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PS4 /\\ button up");

				return true;
			}

			return false;
		}
		#endregion

		#region PS4 Square button
		public bool PS4SquareButtonDown()
		{
			if (Input.GetButtonDown("PS4_Square_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PS4 [] button down");

				return true;
			}

			return false;
		}

		public bool PS4SquareButton()
		{
			if (Input.GetButton("PS4_Square_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PS4 [] button");

				return true;
			}

			return false;
		}

		public bool PS4SquareButtonUp()
		{
			if (Input.GetButtonUp("PS4_Square_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PS4 [] button up");

				return true;
			}

			return false;
		}
		#endregion

		#region PS4 Circle button
		public bool PS4CircleButtonDown()
		{
			if (Input.GetButtonDown("PS4_Circle_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PS4 O button down");

				return true;
			}

			return false;
		}

		public bool PS4CircleButton()
		{
			if (Input.GetButton("PS4_Circle_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PS4 O button");

				return true;
			}

			return false;
		}

		public bool PS4CircleButtonUp()
		{
			if (Input.GetButtonUp("PS4_Circle_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PS4 O button up");

				return true;
			}

			return false;
		}
		#endregion

		#region PS4 Cross button
		public bool PS4CrossButtonDown()
		{
			if (Input.GetButtonDown("PS4_Cross_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PS4 X button down");

				return true;
			}

			return false;
		}

		public bool PS4CrossButton()
		{
			if (Input.GetButton("PS4_Cross_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PS4 X button");

				return true;
			}

			return false;
		}

		public bool PS4CrossButtonUp()
		{
			if (Input.GetButtonUp("PS4_Cross_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PS4 X button up");

				return true;
			}

			return false;
		}
		#endregion

		#region PS4 L1 button
		public bool PS4L1ButtonDown()
		{
			if (Input.GetButtonDown("PS4_L1_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PS4 L1 button down");

				return true;
			}

			return false;
		}

		public bool PS4L1Button()
		{
			if (Input.GetButton("PS4_L1_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PS4 L1 button");

				return true;
			}

			return false;
		}

		public bool PS4L1ButtonUp()
		{
			if (Input.GetButtonUp("PS4_L1_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PS4 L1 button up");

				return true;
			}

			return false;
		}
		#endregion

		#region PS4 L2 button
		public bool PS4L2ButtonDown()
		{
			if (Input.GetButtonDown("PS4_L2_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PS4 L2 button down");

				return true;
			}

			return false;
		}

		public bool PS4L2Button()
		{
			if (Input.GetButton("PS4_L2_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PS4 L2 button");

				return true;
			}

			return false;
		}

		public bool PS4L2ButtonUp()
		{
			if (Input.GetButtonUp("PS4_L2_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PS4 L2 button up");

				return true;
			}

			return false;
		}
		#endregion

		#region PS4 L3 button
		public bool PS4L3ButtonDown()
		{
			if (Input.GetButtonDown("PS4_L3_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PS4 L3 button down");

				return true;
			}

			return false;
		}

		public bool PS4L3Button()
		{
			if (Input.GetButton("PS4_L3_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PS4 L3 button");

				return true;
			}

			return false;
		}

		public bool PS4L3ButtonUp()
		{
			if (Input.GetButtonUp("PS4_L3_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PS4 L3 button up");

				return true;
			}

			return false;
		}
		#endregion

		#region PS4 R1 button
		public bool PS4R1ButtonDown()
		{
			if (Input.GetButtonDown("PS4_R1_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PS4 R1 button down");

				return true;
			}

			return false;
		}

		public bool PS4R1Button()
		{
			if (Input.GetButton("PS4_R1_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PS4 R1 button");

				return true;
			}

			return false;
		}

		public bool PS4R1ButtonUp()
		{
			if (Input.GetButtonUp("PS4_R1_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PS4 R1 button up");

				return true;
			}

			return false;
		}
		#endregion

		#region PS4 R2 button
		public bool PS4R2ButtonDown()
		{
			if (Input.GetButtonDown("PS4_R2_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PS4 R2 button down");

				return true;
			}

			return false;
		}

		public bool PS4R2Button()
		{
			if (Input.GetButton("PS4_R2_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PS4 R2 button");

				return true;
			}

			return false;
		}

		public bool PS4R2ButtonUp()
		{
			if (Input.GetButtonUp("PS4_R2_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PS4 R2 button up");

				return true;
			}

			return false;
		}
		#endregion

		#region PS4 R3 button
		public bool PS4R3ButtonDown()
		{
			if (Input.GetButtonDown("PS4_R3_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PS4 R3 button down");

				return true;
			}

			return false;
		}

		public bool PS4R3Button()
		{
			if (Input.GetButton("PS4_R3_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PS4 R3 button");

				return true;
			}

			return false;
		}

		public bool PS4R3ButtonUp()
		{
			if (Input.GetButtonUp("PS4_R3_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PS4 R3 button up");

				return true;
			}

			return false;
		}
		#endregion

		#region PS4 Share button
		public bool PS4ShareButtonDown()
		{
			if (Input.GetButtonDown("PS4_Share_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PS4 Share button down");

				return true;
			}

			return false;
		}

		public bool PS4ShareButton()
		{
			if (Input.GetButton("PS4_Share_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PS4 Share button");

				return true;
			}

			return false;
		}

		public bool PS4ShareButtonUp()
		{
			if (Input.GetButtonUp("PS4_Share_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PS4 Share button up");

				return true;
			}

			return false;
		}
		#endregion

		#region PS4 Options button
		public bool PS4OptionsButtonDown()
		{
			if (Input.GetButtonDown("PS4_Options_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PS4 Options button down");

				return true;
			}

			return false;
		}

		public bool PS4OptionsButton()
		{
			if (Input.GetButton("PS4_Options_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PS4 Options button");

				return true;
			}

			return false;
		}

		public bool PS4OptionsButtonUp()
		{
			if (Input.GetButtonUp("PS4_Options_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PS4 Options button up");

				return true;
			}

			return false;
		}
		#endregion

		#region PS4 PS button
		public bool PS4PSButtonDown()
		{
			if (Input.GetButtonDown("PS4_PS_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PS4 PS button down");

				return true;
			}

			return false;
		}

		public bool PS4PSButton()
		{
			if (Input.GetButton("PS4_PS_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PS4 PS button");

				return true;
			}

			return false;
		}

		public bool PS4PSButtonUp()
		{
			if (Input.GetButtonUp("PS4_PS_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PS4 PS button up");

				return true;
			}

			return false;
		}
		#endregion

		#region PS4 TouchPad button
		public bool PS4TouchPadButtonDown()
		{
			if (Input.GetButtonDown("PS4_TouchPad_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PS4 TouchPad button down");

				return true;
			}

			return false;
		}

		public bool PS4TouchPadButton()
		{
			if (Input.GetButton("PS4_TouchPad_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PS4 TouchPad button");

				return true;
			}

			return false;
		}

		public bool PS4TouchPadButtonUp()
		{
			if (Input.GetButtonUp("PS4_TouchPad_Button"))
			{
				if (IsDebugEnabled) Debug.Log("PS4 TouchPad button up");

				return true;
			}

			return false;
		}
		#endregion

		#region PS4 Direction pad horizontal axis
		public float PS4DPadHorizontalAxis()
		{
			float l_amount = Input.GetAxis("PS4_DPad_Horizontal");

			if (IsDebugEnabled && l_amount != 0.0f)
				Debug.Log("Horizontal DPad: " + l_amount.ToString());

			return l_amount;
		}
		#endregion

		#region PS4 Direction pad vertical axis
		public float PS4DPadVerticalAxis()
		{
			float l_amount = Input.GetAxis("PS4_DPad_Vertical");

			if (IsDebugEnabled && l_amount != 0.0f)
				Debug.Log("Vertical DPad: " + l_amount.ToString());

			return l_amount;
		}
		#endregion

		#region PS4 DPad Up as button
		public bool PS4DPadUpAsButtonDown()
		{
			if (!m_WasButtonPressedDPadUp && m_IsButtonPressedDPadUp)
			{
				if (IsDebugEnabled) Debug.Log("PS4 DPad Up button down");

				return true;
			}

			return false;
		}

		public bool PS4DPadUpAsButton()
		{
			if (m_WasButtonPressedDPadUp && m_IsButtonPressedDPadUp)
			{
				if (IsDebugEnabled) Debug.Log("PS4 DPad Up button");

				return true;
			}

			return false;
		}

		public bool PS4DPadUpAsButtonUp()
		{
			if (m_WasButtonPressedDPadUp && !m_IsButtonPressedDPadUp)
			{
				if (IsDebugEnabled) Debug.Log("PS4 DPad Up button up");

				return true;
			}

			return false;
		}
		#endregion

		#region PS4 DPad Down as button
		public bool PS4DPadDownAsButtonDown()
		{
			if (!m_WasButtonPressedDPadDown && m_IsButtonPressedDPadDown)
			{
				if (IsDebugEnabled) Debug.Log("PS4 DPad Down button down");

				return true;
			}

			return false;
		}

		public bool PS4DPadDownAsButton()
		{
			if (m_WasButtonPressedDPadDown && m_IsButtonPressedDPadDown)
			{
				if (IsDebugEnabled) Debug.Log("PS4 DPad Down button");

				return true;
			}

			return false;
		}

		public bool PS4DPadDownAsButtonUp()
		{
			if (m_WasButtonPressedDPadDown && !m_IsButtonPressedDPadDown)
			{
				if (IsDebugEnabled) Debug.Log("PS4 DPad Down button up");

				return true;
			}

			return false;
		}
		#endregion

		#region PS4 DPad Left as button
		public bool PS4DPadLeftAsButtonDown()
		{
			if (!m_WasButtonPressedDPadLeft && m_IsButtonPressedDPadLeft)
			{
				if (IsDebugEnabled) Debug.Log("PS4 DPad Left button down");

				return true;
			}

			return false;
		}

		public bool PS4DPadLeftAsButton()
		{
			if (m_WasButtonPressedDPadLeft && m_IsButtonPressedDPadLeft)
			{
				if (IsDebugEnabled) Debug.Log("PS4 DPad Left button");

				return true;
			}

			return false;
		}

		public bool PS4DPadLeftAsButtonUp()
		{
			if (m_WasButtonPressedDPadLeft && !m_IsButtonPressedDPadLeft)
			{
				if (IsDebugEnabled) Debug.Log("PS4 DPad Left button up");

				return true;
			}

			return false;
		}
		#endregion

		#region PS4 DPad Right as button
		public bool PS4DPadRightAsButtonDown()
		{
			if (!m_WasButtonPressedDPadRight && m_IsButtonPressedDPadRight)
			{
				if (IsDebugEnabled) Debug.Log("PS4 DPad Right button down");

				return true;
			}

			return false;
		}

		public bool PS4DPadRightAsButton()
		{
			if (m_WasButtonPressedDPadRight && m_IsButtonPressedDPadRight)
			{
				if (IsDebugEnabled) Debug.Log("PS4 DPad Right button");

				return true;
			}

			return false;
		}

		public bool PS4DPadRightAsButtonUp()
		{
			if (m_WasButtonPressedDPadRight && !m_IsButtonPressedDPadRight)
			{
				if (IsDebugEnabled) Debug.Log("PS4 DPad Right button up");

				return true;
			}

			return false;
		}
		#endregion

		#region PS4 Left joystick horizontal axis
		public float PS4LeftJoystickHorizontalAxis()
		{
			float l_amount = Input.GetAxis("PS4_L_Joystick_Horizontal");

			if (IsDebugEnabled && l_amount != 0.0f)
				Debug.Log("Horizontal L joystick: " + l_amount.ToString());

			return l_amount;
		}
		#endregion

		#region PS4 Left joystick vertical axis
		public float PS4LeftJoystickVerticalAxis()
		{
			float l_amount = Input.GetAxis("PS4_L_Joystick_Vertical");

			if (IsDebugEnabled && l_amount != 0.0f)
				Debug.Log("Vertical L joystick: " + l_amount.ToString());

			return l_amount;
		}
		#endregion

		#region PS4 Right joystick horizontal axis
		public float PS4RightJoystickHorizontalAxis()
		{
			float l_amount = Input.GetAxis("PS4_R_Joystick_Horizontal");

			if (IsDebugEnabled && l_amount != 0.0f)
				Debug.Log("Horizontal R joystick: " + l_amount.ToString());

			return l_amount;
		}
		#endregion

		#region PS4 Right joystick vertical axis
		public float PS4RightJoystickVerticalAxis()
		{
			float l_amount = Input.GetAxis("PS4_R_Joystick_Vertical");

			if (IsDebugEnabled && l_amount != 0.0f)
				Debug.Log("Vertical R joystick: " + l_amount.ToString());

			return l_amount;
		}
		#endregion

		#region PS4 L2 trigger
		public float PS4L2Trigger()
		{
			float l_amount = Input.GetAxis("PS4_L2_Trigger");

			if (IsDebugEnabled && l_amount != 0.0f)
				Debug.Log("L2 Trigger: " + l_amount.ToString());

			return l_amount;
		}
		#endregion

		#region PS4 R2 trigger
		public float PS4R2Trigger()
		{
			float l_amount = Input.GetAxis("PS4_R2_Trigger");

			if (IsDebugEnabled && l_amount != 0.0f)
				Debug.Log("R2 Trigger: " + l_amount.ToString());

			return l_amount;
		}
		#endregion
	}
}
