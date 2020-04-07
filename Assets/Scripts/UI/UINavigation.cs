using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.GameManagerController;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
	public delegate void UISelectableOnClick(IUISelectable uISelectable);       // Delegate to assign methods to UIToggle

	public sealed class UINavigation : MonoBehaviour
	{
		[SerializeField]
		private Selectable[] _selectables = null;                               // Array of selectables within the navigation
        [SerializeField]
		private int _currentSelectableID = 0;                                   // Flag of current selected selectable

        public Selectable[] Selectables { get { return _selectables; } }

        public int CurrentSelectableID
        {
            get
            { return _currentSelectableID; }

            set
            { _currentSelectableID = value; }
        }

        private void Awake()
        {
            
        }

        // Use this for initialization
        private void Start()
		{
			// Default selected selectable (first in the array)
			_selectables[0].Select();
			_currentSelectableID = 0;
		}

		// Update is called once per frame
		private void Update()
		{
            // Navigate through the selectables
			Navigate();
		}

        private void OnEnable()
        {
			// Set first selectable as selected by default 
			_selectables[0].GetComponent<IUISelectable>().SelectOnEnable();
			// Set current selectable id
			_currentSelectableID = 0;
            // Deselect all other selectables
            // Set first enable flag to false
            for (int i = 1; i < _selectables.Length; i++)
            {
				_selectables[i].OnDeselect(null);
				_selectables[i].GetComponent<IUISelectable>().FirstEnable = false;
            }
        }

        /// <summary>
        ///     Navigates through the selectables
        /// </summary>
        private void Navigate()
		{
			Selectable l_selectable = null;

			if (GameManager.Instance.GameController.MenuUp())
			{
				l_selectable = _selectables[_currentSelectableID].FindSelectableOnUp();
			}

			if (GameManager.Instance.GameController.MenuDown())
			{
				l_selectable = _selectables[_currentSelectableID].FindSelectableOnDown();
			}

			if (GameManager.Instance.GameController.MenuLeft())
			{
				l_selectable = _selectables[_currentSelectableID].FindSelectableOnLeft();
			}

			if (GameManager.Instance.GameController.MenuRight())
			{
				l_selectable = _selectables[_currentSelectableID].FindSelectableOnRight();
			}

			if (l_selectable != null)
			{
				l_selectable.Select();

				for (int i = 0; i < _selectables.Length; i++)
				{
					if (l_selectable.Equals(_selectables[i]))
					{
						_currentSelectableID = i;

						return;
					}
				}
			}
		}
	}
}
