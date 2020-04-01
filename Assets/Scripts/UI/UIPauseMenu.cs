using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.GameManagerController;
using Assets.Scripts.Scenes;

namespace Assets.Scripts.UI
{
    [RequireComponent(typeof(UIButtonNavigation))]
    public sealed class UIPauseMenu : MonoBehaviour
    {
        private UIButtonNavigation _uiButtonNavigation = null;          // Reference to the UIButtonNavigation component

        private void Awake()
        {
            // Get UIButtonNavigation component
            _uiButtonNavigation = GetComponent<UIButtonNavigation>();
            // Add on click listeners to buttons
            //_uiButtonNavigation.Buttons[0].onClick.AddListener(() => OnButtonResumeClicked(_uiButtonNavigation.Buttons[0]));
            //_uiButtonNavigation.Buttons[1].onClick.AddListener(() => OnButtonMenuClicked(_uiButtonNavigation.Buttons[1]));
            //_uiButtonNavigation.Buttons[2].onClick.AddListener(() => OnButtonQuitClicked(_uiButtonNavigation.Buttons[2]));
        }

        // Use this for initialization
        private void Start()
        {
            // Link methods to UIButtons
            _uiButtonNavigation.UIButtons[0].onClick = OnButtonResumeClicked;
            _uiButtonNavigation.UIButtons[1].onClick = OnButtonMenuClicked;
            _uiButtonNavigation.UIButtons[2].onClick = OnButtonQuitClicked;

            // Set pause menu inactive
            gameObject.SetActive(false);
        }

        // Update is called once per frame
        private void Update()
        {
            if (GameManager.Instance.GameController.Accept())
            {
                // Invoke method linked to the UIButton
                _uiButtonNavigation.UIButtons[_uiButtonNavigation.CurrentButtonID]
                    .onClick.Invoke(_uiButtonNavigation.UIButtons[_uiButtonNavigation.CurrentButtonID]);

                //_uiButtonNavigation.Buttons[_uiButtonNavigation.CurrentButtonID].onClick.Invoke();

                //switch (_uiButtonNavigation.CurrentButtonID)
                //{
                //    case 0:
                //        //OnButtonResumeClicked(_uiButtonNavigation.Buttons[_uiButtonNavigation.CurrentButtonID]);
                //        break;
                //    case 1:
                //        //OnButtonMenuClicked(_uiButtonNavigation.Buttons[_uiButtonNavigation.CurrentButtonID]);
                //        break;
                //    case 2:
                //        //OnButtonQuitClicked(_uiButtonNavigation.Buttons[_uiButtonNavigation.CurrentButtonID]);
                //        break;
                //    default:
                //        break;
                //}
            }

            if (GameManager.Instance.GameController.Cancel())
            {
                // Go back to the game, like clicking resume
                GameManager.Instance.ManagePause();
            }
        }

        public void OnButtonResumeClicked(UIButton uiButton)
        {
            uiButton.OnClick();

            GameManager.Instance.ManagePause();
        }

        public void OnButtonMenuClicked(UIButton uiButton)
        {
            uiButton.OnClick();

            GameManager.Instance.ManagePause();

            // Back to main menu
            GameManager.Instance.GameManagerState.StateChange(EGameScenes.MainMenu);
        }

        public void OnButtonQuitClicked(UIButton uiButton)
        {
            uiButton.OnClick();

            Debug.Log("QUIT");

            Application.Quit();
        }
    }
}
