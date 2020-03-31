using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.GameManagerController;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
	public sealed class UIButtonNavigation : MonoBehaviour
	{
		[SerializeField]
		private Button[] _buttons = null;                       // Array of buttons within the navigation
		[SerializeField]
		private int _currentButtonID = 0;                       // Flag of current selected button

		public Button[] Buttons { get { return _buttons; } }
        public int CurrentButtonID { get { return _currentButtonID; } }

		// Use this for initialization
		private void Start()
		{
            // Default selected button (first in the array)
			_buttons[0].Select();
			_currentButtonID = 0;
		}

		// Update is called once per frame
		private void Update()
		{
            // Navigate among the buttons
			Navigate();
		}

		private void OnEnable()
		{
			// Set first button as selected by default 
			_buttons[0].GetComponent<UIButton>().SelectButtonOnEnable();
			// Set current button id
			_currentButtonID = 0;
			// Deselect all other buttons
			for (int i = 1; i < _buttons.Length; i++)
			{
				_buttons[i].GetComponent<UIButton>().OnDeselect(null);
			}
		}

		/// <summary>
		///  Navigates among the buttons
		/// </summary>
		private void Navigate()
		{
			Button l_button = null;

			if (GameManager.Instance.GameController.MenuUp())
			{
				l_button = (Button)_buttons[_currentButtonID].FindSelectableOnUp();
			}

			if (GameManager.Instance.GameController.MenuDown())
			{
				l_button = (Button)_buttons[_currentButtonID].FindSelectableOnDown();
			}

			if (GameManager.Instance.GameController.MenuLeft())
			{
				l_button = (Button)_buttons[_currentButtonID].FindSelectableOnLeft();
			}

			if (GameManager.Instance.GameController.MenuRight())
			{
				l_button = (Button)_buttons[_currentButtonID].FindSelectableOnRight();
			}

			if (l_button != null)
			{
				l_button.Select();

				for (int i = 0; i < _buttons.Length; i++)
				{
					if (l_button.Equals(_buttons[i]))
					{
						_currentButtonID = i;

						return;
					}
				}
			}
		}
	}
}
