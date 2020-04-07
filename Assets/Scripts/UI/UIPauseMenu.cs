using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.GameManagerController;
using Assets.Scripts.Scenes;

namespace Assets.Scripts.UI
{
    [RequireComponent(typeof(UINavigation))]
    public sealed class UIPauseMenu : MonoBehaviour
    {
        private UINavigation _uiNavigation = null;                              // Reference to the UINavigation component

        private void Awake()
        {
            // Get UINavigation component
            _uiNavigation = GetComponent<UINavigation>();
            // Add on click listeners to buttons
            //_uiButtonNavigation.Buttons[0].onClick.AddListener(() => OnButtonResumeClicked(_uiButtonNavigation.Buttons[0]));
            //_uiButtonNavigation.Buttons[1].onClick.AddListener(() => OnButtonMenuClicked(_uiButtonNavigation.Buttons[1]));
            //_uiButtonNavigation.Buttons[2].onClick.AddListener(() => OnButtonQuitClicked(_uiButtonNavigation.Buttons[2]));
        }

        // Use this for initialization
        private void Start()
        {
            // Link methods to UIButtons
            _uiNavigation.Selectables[0].GetComponent<UIButton>().OnClickCallback = OnButtonResumeClicked;
            _uiNavigation.Selectables[1].GetComponent<UIButton>().OnClickCallback = OnButtonMenuClicked;
            _uiNavigation.Selectables[2].GetComponent<UIButton>().OnClickCallback = OnButtonQuitClicked;

            // Set pause menu inactive
            gameObject.SetActive(false);
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

            if (GameManager.Instance.GameController.Cancel())
            {
                // Go back to the game, like clicking resume
                GameManager.Instance.ManagePause();
            }
        }

        public void OnButtonResumeClicked(IUISelectable uiButton)
        {
            uiButton.OnClick();

            GameManager.Instance.ManagePause();
        }

        public void OnButtonMenuClicked(IUISelectable uiButton)
        {
            uiButton.OnClick();

            GameManager.Instance.ManagePause();

            // Back to main menu
            GameManager.Instance.GameManagerState.StateChange(EGameScenes.MainMenu);
        }

        public void OnButtonQuitClicked(IUISelectable uiButton)
        {
            uiButton.OnClick();

            Debug.Log("QUIT");

            Application.Quit();
        }
    }
}
