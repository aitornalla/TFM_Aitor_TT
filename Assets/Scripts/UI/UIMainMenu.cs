using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.GameManagerController;
using Assets.Scripts.Scenes;

namespace Assets.Scripts.UI
{
    [RequireComponent(typeof(UIButtonNavigation))]
    public sealed class UIMainMenu : MonoBehaviour
    {
        private UIButtonNavigation _uiButtonNavigation = null;          // Reference to the UIButtonNavigation component

        private void Awake()
        {
            // Get UIButtonNavigation component
            _uiButtonNavigation = GetComponent<UIButtonNavigation>();
            // Add on click listeners to UIButtons
            //_uiButtonNavigation.Buttons[0].onClick.AddListener(() => OnButtonTestLevelClicked(_uiButtonNavigation.Buttons[0]));
            //_uiButtonNavigation.Buttons[1].onClick.AddListener(() => OnButtonQuitClicked(_uiButtonNavigation.Buttons[1]));
        }

        // Use this for initialization
        private void Start()
        {
            // Link methods to UIButtons
            _uiButtonNavigation.UIButtons[0].onClick = OnButtonTestLevelClicked;
            _uiButtonNavigation.UIButtons[1].onClick = OnButtonQuitClicked;
        }

        // Update is called once per frame
        private void Update()
        {
            if (GameManager.Instance.GameController.Accept())
            {
                // Invoke method linked to the UIButton
                _uiButtonNavigation.UIButtons[_uiButtonNavigation.CurrentButtonID]
                    .onClick.Invoke(_uiButtonNavigation.UIButtons[_uiButtonNavigation.CurrentButtonID]);

                //switch (_uiButtonNavigation.CurrentButtonID)
                //{
                //    case 0:
                //        OnButtonTestLevelClicked(_uiButtonNavigation.Buttons[_uiButtonNavigation.CurrentButtonID]);
                //        break;
                //    case 1:
                //        OnButtonQuitClicked(_uiButtonNavigation.Buttons[_uiButtonNavigation.CurrentButtonID]);
                //        break;
                //    default:
                //        break;
                //}
            }
        }

        public void OnButtonTestLevelClicked(UIButton uiButton)
        {
            uiButton.OnClick();

            // Load test level scene
            GameManager.Instance.GameManagerState.StateChange(EGameScenes.TestLevel);
        }

        public void OnButtonQuitClicked(UIButton uiButton)
        {
            uiButton.OnClick();

            Debug.Log("QUIT");

            Application.Quit();
        }
    }
}
