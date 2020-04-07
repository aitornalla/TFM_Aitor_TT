using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Assets.Scripts.GameManagerController;
using Assets.Scripts.Scenes;

namespace Assets.Scripts.UI
{
    [RequireComponent(typeof(UINavigation))]
    public sealed class UISettingsMenu : MonoBehaviour
    {
        private UINavigation _uiNavigation = null;                              // Reference to the UINavigation component
        private string[] _formattedScreenResolutions = null;                    // Array of available resolutions
        private int _currentScreenResolutionIndex = 0;                          // Index of current screen resolution

        [SerializeField]
        private Text _resolutionOptionText = null;                              // Reference to resolution option text
        [SerializeField]
        private Slider _volumeSlider = null;                                    // Reference to volume slider

        private void Awake()
        {
            // Get UIButtonNavigation component
            _uiNavigation = GetComponent<UINavigation>();
            // Add on click listeners to UIButtons
            //_uiButtonNavigation.Buttons[0].onClick.AddListener(() => OnButtonTestLevelClicked(_uiButtonNavigation.Buttons[0]));
            //_uiButtonNavigation.Buttons[1].onClick.AddListener(() => OnButtonQuitClicked(_uiButtonNavigation.Buttons[1]));

            // Get and set up screen resolutions
            SetUpScreenResolutions();
        }

        // Use this for initialization
        private void Start()
        {
            // Link method to "Back" UIButton
            _uiNavigation.Selectables[0].GetComponent<UIButton>().OnClickCallback = OnButtonBackClicked;
            // Link method to "Fullscreen" UIToggle
            _uiNavigation.Selectables[1].GetComponent<UIToggle>().OnClickCallback = OnToggleFullScreenClicked;
            // Set toogle of "Fullscreen" UIToggle
            _uiNavigation.Selectables[1].GetComponent<UIToggle>().SetToggle(Screen.fullScreen);
            // Link methods to "Resolution" UIButtons
            _uiNavigation.Selectables[2].GetComponent<UIButton>().OnClickCallback = OnButtonLeftResolutionClicked;
            _uiNavigation.Selectables[3].GetComponent<UIButton>().OnClickCallback = OnButtonRightResolutionClicked;
            // Link methods to "Volume" UIButtons
            _uiNavigation.Selectables[4].GetComponent<UIButton>().OnClickCallback = OnButtonLeftVolumeClicked;
            _uiNavigation.Selectables[5].GetComponent<UIButton>().OnClickCallback = OnButtonRightVolumeClicked;
            // Set volume slider value
            _volumeSlider.value = GameManager.Instance.AudioMixerController.GetCurrentVolume01();
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

        private void SetUpScreenResolutions()
        {
            // Instantiate formatted screen resolutions array
            _formattedScreenResolutions = new string[Screen.resolutions.Length];
            // Populate array with formatted string resolutions
            for (int i = 0; i < Screen.resolutions.Length; i++)
            {
                _formattedScreenResolutions[i] = Screen.resolutions[i].width + "x" + Screen.resolutions[i].height;
                // Set current screen resolution index
                if (Screen.resolutions[i].width == Screen.currentResolution.width &&
                    Screen.resolutions[i].height == Screen.currentResolution.height)
                {
                    _currentScreenResolutionIndex = i;
                }
            }
            // Set current resolution option text
            _resolutionOptionText.text = _formattedScreenResolutions[_currentScreenResolutionIndex];
        }

        public void OnButtonBackClicked(IUISelectable uiButton)
        {
            uiButton.OnClick();

            // Load main menu scene
            GameManager.Instance.GameManagerState.StateChange(EGameScenes.MainMenu);
        }

        public void OnButtonLeftResolutionClicked(IUISelectable uiButton)
        {
            uiButton.OnClick();

            if (_currentScreenResolutionIndex != 0)
            {
                // Change current resolution index
                _currentScreenResolutionIndex--;
                // Change resolution
                Screen.SetResolution(
                    Screen.resolutions[_currentScreenResolutionIndex].width,
                    Screen.resolutions[_currentScreenResolutionIndex].height,
                    _uiNavigation.Selectables[1].GetComponent<Toggle>().isOn);
                // Set current resolution option text
                _resolutionOptionText.text = _formattedScreenResolutions[_currentScreenResolutionIndex];
            }
        }

        public void OnButtonRightResolutionClicked(IUISelectable uiButton)
        {
            uiButton.OnClick();

            if (_currentScreenResolutionIndex < Screen.resolutions.Length - 1)
            {
                // Change current resolution index
                _currentScreenResolutionIndex++;
                // Change resolution
                Screen.SetResolution(
                    Screen.resolutions[_currentScreenResolutionIndex].width,
                    Screen.resolutions[_currentScreenResolutionIndex].height,
                    _uiNavigation.Selectables[1].GetComponent<Toggle>().isOn);
                // Set current resolution option text
                _resolutionOptionText.text = _formattedScreenResolutions[_currentScreenResolutionIndex];
            }
        }

        public void OnButtonLeftVolumeClicked(IUISelectable uiButton)
        {
            uiButton.OnClick();

            // Set AudioMixer volume down
            GameManager.Instance.AudioMixerController.SetVolume(CustomClasses.EAudioMixerVolume.Down);

            // Set volume slider value
            _volumeSlider.value = GameManager.Instance.AudioMixerController.GetCurrentVolume01();
        }

        public void OnButtonRightVolumeClicked(IUISelectable uiButton)
        {
            uiButton.OnClick();

            // Set AudioMixer volume up
            GameManager.Instance.AudioMixerController.SetVolume(CustomClasses.EAudioMixerVolume.Up);

            // Set volume slider value
            _volumeSlider.value = GameManager.Instance.AudioMixerController.GetCurrentVolume01();
        }

        public void OnToggleFullScreenClicked(IUISelectable uiToggle)
        {
            uiToggle.OnClick();

            // Change fullscreen mode
            Screen.fullScreen = !Screen.fullScreen;
            // Set toggle
            uiToggle.Selectable.GetComponent<UIToggle>().SetToggle(Screen.fullScreen);
        }
    }
}
