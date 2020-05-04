using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.GameManagerController;
using Assets.Scripts.Scenes;

namespace Assets.Scripts.UI
{
    [RequireComponent(typeof(UINavigation))]
    public sealed class UIMainMenu : MonoBehaviour
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
            _uiNavigation.Selectables[0].GetComponent<UIButton>().OnClickCallback = OnButtonLevelsClicked;
            _uiNavigation.Selectables[1].GetComponent<UIButton>().OnClickCallback = OnButtonTestLevelClicked;
            _uiNavigation.Selectables[2].GetComponent<UIButton>().OnClickCallback = OnButtonSettingsClicked;
            _uiNavigation.Selectables[3].GetComponent<UIButton>().OnClickCallback = OnButtonHowToPlayClicked;
            _uiNavigation.Selectables[4].GetComponent<UIButton>().OnClickCallback = OnButtonQuitClicked;
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

        public void OnButtonLevelsClicked(IUISelectable uiButton)
        {
            uiButton.OnClick();

            // Load levels scene
            GameManager.Instance.GameManagerState.StateChange(EGameScenes.LevelsMenu);
        }

        public void OnButtonTestLevelClicked(IUISelectable uiButton)
        {
            uiButton.OnClick();

            // Load test level scene
            GameManager.Instance.GameManagerState.StateChange(EGameScenes.TestLevel);
        }

        public void OnButtonSettingsClicked(IUISelectable uiButton)
        {
            uiButton.OnClick();

            // Load settings scene
            GameManager.Instance.GameManagerState.StateChange(EGameScenes.SettingsMenu);
        }

        public void OnButtonHowToPlayClicked(IUISelectable uiButton)
        {
            uiButton.OnClick();

            // Load how to play scene
            GameManager.Instance.GameManagerState.StateChange(EGameScenes.HowToPlayMenu);
        }

        public void OnButtonQuitClicked(IUISelectable uiButton)
        {
            uiButton.OnClick();

            Debug.Log("QUIT");

            Application.Quit();
        }
    }
}
