using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameController.PlatformControllers
{
	/// <summary>
	/// 	PS3 controller implementation for all buttons/axis and states: down, up, held down
	/// </summary>
	public sealed class PS3Controller : MonoBehaviour
	{
		public bool IsDebugEnabled { get; set; }

		#region PS3 Triangle button
		public bool PS3TriangleButtonDown ()
		{
			if (Input.GetButtonDown ("PS3_Triangle_Button"))
			{
				if (IsDebugEnabled) Debug.Log ("PS3 /\\ button down");

				return true;
			}

			return false;
		}

		public bool PS3TriangleButton ()
		{
			if (Input.GetButton ("PS3_Triangle_Button"))
			{
				if (IsDebugEnabled) Debug.Log ("PS3 /\\ button");

				return true;
			}

			return false;
		}

		public bool PS3TriangleButtonUp ()
		{
			if (Input.GetButtonUp ("PS3_Triangle_Button"))
			{
				if (IsDebugEnabled) Debug.Log ("PS3 /\\ button up");

				return true;
			}

			return false;
		}
		#endregion

		#region PS3 Square button
		public bool PS3SquareButtonDown ()
		{
			if (Input.GetButtonDown ("PS3_Square_Button"))
			{
				if (IsDebugEnabled) Debug.Log ("PS3 [] button down");

				return true;
			}

			return false;
		}

		public bool PS3SquareButton ()
		{
			if (Input.GetButton ("PS3_Square_Button"))
			{
				if (IsDebugEnabled) Debug.Log ("PS3 [] button");

				return true;
			}

			return false;
		}

		public bool PS3SquareButtonUp ()
		{
			if (Input.GetButtonUp ("PS3_Square_Button"))
			{
				if (IsDebugEnabled) Debug.Log ("PS3 [] button up");

				return true;
			}

			return false;
		}
		#endregion

		#region PS3 Circle button
		public bool PS3CircleButtonDown ()
		{
			if (Input.GetButtonDown ("PS3_Circle_Button"))
			{
				if (IsDebugEnabled) Debug.Log ("PS3 O button down");

				return true;
			}

			return false;
		}

		public bool PS3CircleButton ()
		{
			if (Input.GetButton ("PS3_Circle_Button"))
			{
				if (IsDebugEnabled) Debug.Log ("PS3 O button");

				return true;
			}

			return false;
		}

		public bool PS3CircleButtonUp ()
		{
			if (Input.GetButtonUp ("PS3_Circle_Button"))
			{
				if (IsDebugEnabled) Debug.Log ("PS3 O button up");

				return true;
			}

			return false;
		}
		#endregion

		#region PS3 Cross button
		public bool PS3CrossButtonDown ()
		{
			if (Input.GetButtonDown ("PS3_Cross_Button"))
			{
				if (IsDebugEnabled) Debug.Log ("PS3 X button down");

				return true;
			}

			return false;
		}

		public bool PS3CrossButton ()
		{
			if (Input.GetButton ("PS3_Cross_Button"))
			{
				if (IsDebugEnabled) Debug.Log ("PS3 X button");

				return true;
			}

			return false;
		}

		public bool PS3CrossButtonUp ()
		{
			if (Input.GetButtonUp ("PS3_Cross_Button"))
			{
				if (IsDebugEnabled) Debug.Log ("PS3 X button up");

				return true;
			}

			return false;
		}
		#endregion

		#region PS3 L1 button
		public bool PS3L1ButtonDown ()
		{
			if (Input.GetButtonDown ("PS3_L1_Button"))
			{
				if (IsDebugEnabled) Debug.Log ("PS3 L1 button down");

				return true;
			}

			return false;
		}

		public bool PS3L1Button ()
		{
			if (Input.GetButton ("PS3_L1_Button"))
			{
				if (IsDebugEnabled) Debug.Log ("PS3 L1 button");

				return true;
			}

			return false;
		}

		public bool PS3L1ButtonUp ()
		{
			if (Input.GetButtonUp ("PS3_L1_Button"))
			{
				if (IsDebugEnabled) Debug.Log ("PS3 L1 button up");

				return true;
			}

			return false;
		}
		#endregion

		#region PS3 L2 button
		public bool PS3L2ButtonDown ()
		{
			if (Input.GetButtonDown ("PS3_L2_Button"))
			{
				if (IsDebugEnabled) Debug.Log ("PS3 L2 button down");

				return true;
			}

			return false;
		}

		public bool PS3L2Button ()
		{
			if (Input.GetButton ("PS3_L2_Button"))
			{
				if (IsDebugEnabled) Debug.Log ("PS3 L2 button");

				return true;
			}

			return false;
		}

		public bool PS3L2ButtonUp ()
		{
			if (Input.GetButtonUp ("PS3_L2_Button"))
			{
				if (IsDebugEnabled) Debug.Log ("PS3 L2 button up");

				return true;
			}

			return false;
		}
		#endregion

		#region PS3 L3 button
		public bool PS3L3ButtonDown ()
		{
			if (Input.GetButtonDown ("PS3_L3_Button"))
			{
				if (IsDebugEnabled) Debug.Log ("PS3 L3 button down");

				return true;
			}

			return false;
		}

		public bool PS3L3Button ()
		{
			if (Input.GetButton ("PS3_L3_Button"))
			{
				if (IsDebugEnabled) Debug.Log ("PS3 L3 button");

				return true;
			}

			return false;
		}

		public bool PS3L3ButtonUp ()
		{
			if (Input.GetButtonUp ("PS3_L3_Button"))
			{
				if (IsDebugEnabled) Debug.Log ("PS3 L3 button up");

				return true;
			}

			return false;
		}
		#endregion

		#region PS3 R1 button
		public bool PS3R1ButtonDown ()
		{
			if (Input.GetButtonDown ("PS3_R1_Button"))
			{
				if (IsDebugEnabled) Debug.Log ("PS3 R1 button down");

				return true;
			}

			return false;
		}

		public bool PS3R1Button ()
		{
			if (Input.GetButton ("PS3_R1_Button"))
			{
				if (IsDebugEnabled) Debug.Log ("PS3 R1 button");

				return true;
			}

			return false;
		}

		public bool PS3R1ButtonUp ()
		{
			if (Input.GetButtonUp ("PS3_R1_Button"))
			{
				if (IsDebugEnabled) Debug.Log ("PS3 R1 button up");

				return true;
			}

			return false;
		}
		#endregion

		#region PS3 R2 button
		public bool PS3R2ButtonDown ()
		{
			if (Input.GetButtonDown ("PS3_R2_Button"))
			{
				if (IsDebugEnabled) Debug.Log ("PS3 R2 button down");

				return true;
			}

			return false;
		}

		public bool PS3R2Button ()
		{
			if (Input.GetButton ("PS3_R2_Button"))
			{
				if (IsDebugEnabled) Debug.Log ("PS3 R2 button");

				return true;
			}

			return false;
		}

		public bool PS3R2ButtonUp ()
		{
			if (Input.GetButtonUp ("PS3_R2_Button"))
			{
				if (IsDebugEnabled) Debug.Log ("PS3 R2 button up");

				return true;
			}

			return false;
		}
		#endregion

		#region PS3 R3 button
		public bool PS3R3ButtonDown ()
		{
			if (Input.GetButtonDown ("PS3_R3_Button"))
			{
				if (IsDebugEnabled) Debug.Log ("PS3 R3 button down");

				return true;
			}

			return false;
		}

		public bool PS3R3Button ()
		{
			if (Input.GetButton ("PS3_R3_Button"))
			{
				if (IsDebugEnabled) Debug.Log ("PS3 R3 button");

				return true;
			}

			return false;
		}

		public bool PS3R3ButtonUp ()
		{
			if (Input.GetButtonUp ("PS3_R3_Button"))
			{
				if (IsDebugEnabled) Debug.Log ("PS3 R3 button up");

				return true;
			}

			return false;
		}
		#endregion

		#region PS3 Start button
		public bool PS3StartButtonDown ()
		{
			if (Input.GetButtonDown ("PS3_Start_Button"))
			{
				if (IsDebugEnabled) Debug.Log ("PS3 Start button down");

				return true;
			}

			return false;
		}

		public bool PS3StartButton ()
		{
			if (Input.GetButton ("PS3_Start_Button"))
			{
				if (IsDebugEnabled) Debug.Log ("PS3 Start button");

				return true;
			}

			return false;
		}

		public bool PS3StartButtonUp ()
		{
			if (Input.GetButtonUp ("PS3_Start_Button"))
			{
				if (IsDebugEnabled) Debug.Log ("PS3 Start button up");

				return true;
			}

			return false;
		}
		#endregion

		#region PS3 Select button
		public bool PS3SelectButtonDown ()
		{
			if (Input.GetButtonDown ("PS3_Select_Button"))
			{
				if (IsDebugEnabled) Debug.Log ("PS3 Select button down");

				return true;
			}

			return false;
		}

		public bool PS3SelectButton ()
		{
			if (Input.GetButton ("PS3_Select_Button"))
			{
				if (IsDebugEnabled) Debug.Log ("PS3 Select button");

				return true;
			}

			return false;
		}

		public bool PS3SelectButtonUp ()
		{
			if (Input.GetButtonUp ("PS3_Select_Button"))
			{
				if (IsDebugEnabled) Debug.Log ("PS3 Select button up");

				return true;
			}

			return false;
		}
		#endregion

		#region PS3 PS button
		public bool PS3PSButtonDown ()
		{
			if (Input.GetButtonDown ("PS3_PS_Button"))
			{
				if (IsDebugEnabled) Debug.Log ("PS3 PS button down");

				return true;
			}

			return false;
		}

		public bool PS3PSButton ()
		{
			if (Input.GetButton ("PS3_PS_Button"))
			{
				if (IsDebugEnabled) Debug.Log ("PS3 PS button");

				return true;
			}

			return false;
		}

		public bool PS3PSButtonUp ()
		{
			if (Input.GetButtonUp ("PS3_PS_Button"))
			{
				if (IsDebugEnabled) Debug.Log ("PS3 PS button up");

				return true;
			}

			return false;
		}
		#endregion

		#region PS3 Pad left button
		public bool PS3PadLeftButtonDown ()
		{
			if (Input.GetButtonDown ("PS3_Pad_Left_Button"))
			{
				if (IsDebugEnabled) Debug.Log ("PS3 <- button down");

				return true;
			}

			return false;
		}

		public bool PS3PadLeftButton ()
		{
			if (Input.GetButton ("PS3_Pad_Left_Button"))
			{
				if (IsDebugEnabled) Debug.Log ("PS3 <- button");

				return true;
			}

			return false;
		}

		public bool PS3PadLeftButtonUp ()
		{
			if (Input.GetButtonUp ("PS3_Pad_Left_Button"))
			{
				if (IsDebugEnabled) Debug.Log ("PS3 <- button up");

				return true;
			}

			return false;
		}
		#endregion

		#region PS3 Pad right button
		public bool PS3PadRightButtonDown ()
		{
			if (Input.GetButtonDown ("PS3_Pad_Right_Button"))
			{
				if (IsDebugEnabled) Debug.Log ("PS3 -> button down");

				return true;
			}

			return false;
		}

		public bool PS3PadRightButton ()
		{
			if (Input.GetButton ("PS3_Pad_Right_Button"))
			{
				if (IsDebugEnabled) Debug.Log ("PS3 -> button");

				return true;
			}

			return false;
		}

		public bool PS3PadRightButtonUp ()
		{
			if (Input.GetButtonUp ("PS3_Pad_Right_Button"))
			{
				if (IsDebugEnabled) Debug.Log ("PS3 -> button up");

				return true;
			}

			return false;
		}
		#endregion

		#region PS3 Pad up button
		public bool PS3PadUpButtonDown ()
		{
			if (Input.GetButtonDown ("PS3_Pad_Up_Button"))
			{
				if (IsDebugEnabled) Debug.Log ("PS3 ↑ button down");

				return true;
			}

			return false;
		}

		public bool PS3PadUpButton ()
		{
			if (Input.GetButton ("PS3_Pad_Up_Button"))
			{
				if (IsDebugEnabled) Debug.Log ("PS3 ↑ button");

				return true;
			}

			return false;
		}

		public bool PS3PadUpButtonUp ()
		{
			if (Input.GetButtonUp ("PS3_Pad_Up_Button"))
			{
				if (IsDebugEnabled) Debug.Log ("PS3 ↑ button up");

				return true;
			}

			return false;
		}
		#endregion

		#region PS3 Pad down button
		public bool PS3PadDownButtonDown ()
		{
			if (Input.GetButtonDown ("PS3_Pad_Down_Button"))
			{
				if (IsDebugEnabled) Debug.Log ("PS3 ↓ button down");

				return true;
			}

			return false;
		}

		public bool PS3PadDownButton ()
		{
			if (Input.GetButton ("PS3_Pad_Down_Button"))
			{
				if (IsDebugEnabled) Debug.Log ("PS3 ↓ button");

				return true;
			}

			return false;
		}

		public bool PS3PadDownButtonUp ()
		{
			if (Input.GetButtonUp ("PS3_Pad_Down_Button"))
			{
				if (IsDebugEnabled) Debug.Log ("PS3 ↓ button up");

				return true;
			}

			return false;
		}
		#endregion

		#region PS3 Left joystick horizontal axis
		public float PS3LeftJoystickHorizontalAxis ()
		{
			float l_amount = Input.GetAxis ("PS3_L_Joystick_Horizontal");

			if (IsDebugEnabled && l_amount != 0.0f)
				Debug.Log ("Horizontal L joystick: " + l_amount.ToString ());

			return l_amount;
		}
		#endregion

		#region PS3 Left joystick vertical axis
		public float PS3LeftJoystickVerticalAxis ()
		{
			float l_amount = Input.GetAxis ("PS3_L_Joystick_Vertical");

			if (IsDebugEnabled && l_amount != 0.0f)
				Debug.Log ("Vertical L joystick: " + l_amount.ToString ());

			return l_amount;
		}
		#endregion

		#region PS3 Right joystick horizontal axis
		public float PS3RightJoystickHorizontalAxis ()
		{
			float l_amount = Input.GetAxis ("PS3_R_Joystick_Horizontal");

			if (IsDebugEnabled && l_amount != 0.0f)
				Debug.Log ("Horizontal R joystick: " + l_amount.ToString ());

			return l_amount;
		}
		#endregion

		#region PS3 Right joystick vertical axis
		public float PS3RightJoystickVerticalAxis ()
		{
			float l_amount = Input.GetAxis ("PS3_R_Joystick_Vertical");

			if (IsDebugEnabled && l_amount != 0.0f)
				Debug.Log ("Vertical R joystick: " + l_amount.ToString ());

			return l_amount;
		}
		#endregion
	}
}
