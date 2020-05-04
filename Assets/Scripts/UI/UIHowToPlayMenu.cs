using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.GameManagerController;
using Assets.Scripts.Scenes;

namespace Assets.Scripts.UI
{
    [RequireComponent(typeof(UINavigation))]
    public sealed class UIHowToPlayMenu : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] _howToPlaySlides;                                  // Reference to the slides in the how to play menu
        [SerializeField]
        private GameObject[] _controlsSlides;                                   // Reference to the slides of the controllers

        private UINavigation _uiNavigation = null;                              // Reference to the UINavigation component
        private int _currentSlideIndex = 0;                                     // Slide index

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
            _uiNavigation.Selectables[0].GetComponent<UIButton>().OnClickCallback = OnButtonRightClicked;
            _uiNavigation.Selectables[1].GetComponent<UIButton>().OnClickCallback = OnButtonLeftClicked;
            _uiNavigation.Selectables[2].GetComponent<UIButton>().OnClickCallback = OnButtonBackClicked;

            // Set left button to inactive
            _uiNavigation.Selectables[1].gameObject.SetActive(false);

            // Set first slide to active
            _howToPlaySlides[_currentSlideIndex].SetActive(true);
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

        public void OnButtonRightClicked(IUISelectable uiButton)
        {
            uiButton.OnClick();

            // Browse screen slides
            if (_currentSlideIndex + 1 < _howToPlaySlides.Length)
            {
                // Set index to next slide
                _currentSlideIndex++;
                // Set current index slide active only
                for (int i = 0; i < _howToPlaySlides.Length; i++)
                {
                    if (i == _currentSlideIndex)
                    {
                        _howToPlaySlides[i].SetActive(true);
                    }
                    else
                    {
                        _howToPlaySlides[i].SetActive(false);
                    }
                }
                // Manage left/right buttons
                if (_currentSlideIndex == _howToPlaySlides.Length - 1)
                {
                    // At the last slide disable right button
                    _uiNavigation.Selectables[0].gameObject.SetActive(false);
                    // Select back button
                    _uiNavigation.Selectables[2].Select();
                    _uiNavigation.CurrentSelectableID = 2;

                    // Controller slide
                    if (GameManager.Instance.GameController is Assets.Scripts.GameController.PS3GameController)
                    {
                        _controlsSlides[1].SetActive(true);
                    }
                    else if (GameManager.Instance.GameController is Assets.Scripts.GameController.PS4GameController)
                    {
                        _controlsSlides[2].SetActive(true);
                    }
                    else if (GameManager.Instance.GameController is Assets.Scripts.GameController.PSPGameController)
                    {
                        _controlsSlides[3].SetActive(true);
                    }
                    else
                    {
                        // Keyboard
                        _controlsSlides[0].SetActive(true);
                    }
                }
                else
                {
                    _uiNavigation.Selectables[1].gameObject.SetActive(true);
                    _uiNavigation.Selectables[0].gameObject.SetActive(true);
                }
            }
        }

        public void OnButtonLeftClicked(IUISelectable uiButton)
        {
            uiButton.OnClick();

            // Browse screen slides
            if (_currentSlideIndex - 1 >= 0)
            {
                // Set index to previous slide
                _currentSlideIndex--;
                // Set current index slide active only
                for (int i = 0; i < _howToPlaySlides.Length; i++)
                {
                    if (i == _currentSlideIndex)
                    {
                        _howToPlaySlides[i].SetActive(true);
                    }
                    else
                    {
                        _howToPlaySlides[i].SetActive(false);
                    }
                }
                // Manage left/right buttons
                if (_currentSlideIndex == 0)
                {
                    // At the first slide disable left button
                    _uiNavigation.Selectables[1].gameObject.SetActive(false);
                    // Select right button
                    _uiNavigation.Selectables[0].Select();
                    _uiNavigation.CurrentSelectableID = 0;
                }
                else
                {
                    _uiNavigation.Selectables[1].gameObject.SetActive(true);
                    _uiNavigation.Selectables[0].gameObject.SetActive(true);
                }
            }
        }

        public void OnButtonBackClicked(IUISelectable uiButton)
        {
            uiButton.OnClick();

            // Load main menu scene
            GameManager.Instance.GameManagerState.StateChange(EGameScenes.MainMenu);
        }
    }
}
