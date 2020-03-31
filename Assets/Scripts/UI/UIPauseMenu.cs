using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.GameManagerController;

namespace Assets.Scripts.UI
{
    [RequireComponent(typeof(UIButtonNavigation))]
    public sealed class UIPauseMenu : MonoBehaviour
    {
        private UIButtonNavigation _uiButtonNavigation = null;          // Reference to the UIButtonNavigation component
        private bool _isActive = false;

        private void Awake()
        {
            // Get UIButtonNavigation component
            _uiButtonNavigation = GetComponent<UIButtonNavigation>();
            // Add on click listeners to buttons
            _uiButtonNavigation.Buttons[0].onClick.AddListener(() => OnButtonResumeClicked(_uiButtonNavigation.Buttons[0]));
            _uiButtonNavigation.Buttons[1].onClick.AddListener(() => OnButtonMenuClicked(_uiButtonNavigation.Buttons[1]));
            _uiButtonNavigation.Buttons[2].onClick.AddListener(() => OnButtonQuitClicked(_uiButtonNavigation.Buttons[2]));
        }

        // Use this for initialization
        private void Start()
        {
            // Set pause menu inactive
            gameObject.SetActive(false);
        }

        // Update is called once per frame
        private void Update()
        {
            if (GameManager.Instance.GameController.Accept())
            {
                _uiButtonNavigation.Buttons[_uiButtonNavigation.CurrentButtonID].onClick.Invoke();
            }

            if (GameManager.Instance.GameController.Cancel())
            {
                // Go back to the game, like clicking resume
                GameManager.Instance.ManagePause();
            }
        }

        public void OnButtonResumeClicked(Button button)
        {
            button.gameObject.GetComponent<UIButton>().OnClick();

            GameManager.Instance.ManagePause();
        }

        public void OnButtonMenuClicked(Button button)
        {
            button.gameObject.GetComponent<UIButton>().OnClick();

            Debug.Log("MENU");
        }

        public void OnButtonQuitClicked(Button button)
        {
            button.gameObject.GetComponent<UIButton>().OnClick();

            Debug.Log("QUIT");

            Application.Quit();
        }
    }
}
