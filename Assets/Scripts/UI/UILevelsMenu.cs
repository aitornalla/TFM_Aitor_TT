﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.GameManagerController;
using Assets.Scripts.Scenes;

namespace Assets.Scripts.UI
{
    [RequireComponent(typeof(UINavigation))]
    public sealed class UILevelsMenu : MonoBehaviour
    {
        private UINavigation _uiNavigation = null;                              // Reference to the UINavigation component

        private void Awake()
        {
            // Get UINavigation component
            _uiNavigation = GetComponent<UINavigation>();
            // Add on click listeners to UIButtons
            //_uiButtonNavigation.Buttons[0].onClick.AddListener(() => OnButtonTestLevelClicked(_uiButtonNavigation.Buttons[0]));
            //_uiButtonNavigation.Buttons[1].onClick.AddListener(() => OnButtonQuitClicked(_uiButtonNavigation.Buttons[1]));
        }

        // Use this for initialization
        private void Start()
        {
            // Link methods to UIButtons
            _uiNavigation.Selectables[0].GetComponent<UIButton>().OnClickCallback = OnButtonBackClicked;
            _uiNavigation.Selectables[1].GetComponent<UIButton>().OnClickCallback = OnButtonLevel01Clicked;
            _uiNavigation.Selectables[2].GetComponent<UIButton>().OnClickCallback = OnButtonLevel02Clicked;
            _uiNavigation.Selectables[3].GetComponent<UIButton>().OnClickCallback = OnButtonLevel03Clicked;

            // Set buttons for unlocked levels
            SetButtonsForUnlockedLevels();
        }

        // Update is called once per frame
        private void Update()
        {
            if (GameManager.Instance.GameController.Accept())
            {
                // Invoke method linked to the UIButton
                _uiNavigation.Selectables[_uiNavigation.CurrentSelectableID].GetComponent<IUISelectable>()
                    .OnClickCallback.Invoke(_uiNavigation.Selectables[_uiNavigation.CurrentSelectableID].GetComponent<IUISelectable>());
            }
        }

        private void SetButtonsForUnlockedLevels()
        {
            // Set navigation mode to Automatic and change button sprite for buttons of unlocked levels
            int[] l_unlockedLevelsIndex = GameManager.Instance.UnlockedLevels();

            for (int i = 0; i < l_unlockedLevelsIndex.Length; i++)
            {
                // Change navigation mode
                Button l_button = _uiNavigation.Selectables[l_unlockedLevelsIndex[i]].GetComponent<Button>();

                Navigation l_buttonNavigation = l_button.navigation;

                l_buttonNavigation.mode = Navigation.Mode.Automatic;

                l_button.navigation = l_buttonNavigation;

                // Change sprite
                _uiNavigation.Selectables[l_unlockedLevelsIndex[i]].GetComponent<UIButton>().ChangeButtonSprite(EButtonSpriteType.Normal);
            }
        }

        public void OnButtonBackClicked(IUISelectable uiButton)
        {
            uiButton.OnClick();

            // Load main menu scene
            GameManager.Instance.GameManagerState.StateChange(EGameScenes.MainMenu);
        }

        public void OnButtonLevel01Clicked(IUISelectable uiButton)
        {
            uiButton.OnClick();

            // Load level 01 scene
            GameManager.Instance.GameManagerState.StateChange(EGameScenes.Level_01);
        }

        public void OnButtonLevel02Clicked(IUISelectable uiButton)
        {
            uiButton.OnClick();

            // Load level 02 scene
            GameManager.Instance.GameManagerState.StateChange(EGameScenes.Level_02);
        }

        public void OnButtonLevel03Clicked(IUISelectable uiButton)
        {
            uiButton.OnClick();

            // Load level 03 scene
            GameManager.Instance.GameManagerState.StateChange(EGameScenes.Level_03);
        }
    }
}
