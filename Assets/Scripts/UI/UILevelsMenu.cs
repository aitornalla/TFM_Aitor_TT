using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.GameManagerController;
using Assets.Scripts.Scenes;

namespace Assets.Scripts.UI
{
    [RequireComponent(typeof(UINavigation))]
    public sealed class UILevelsMenu : MonoBehaviour
    {
        [SerializeField]
        private GameObject _levelStats;                                         // Reference to level stats panel gameObject
        [SerializeField]
        private Text _statsRibbonText;                                          // Reference to stats ribbon text
        [SerializeField]
        private Text _statsMaxScoreText;                                        // Reference to stats max score text
        [SerializeField]
        private Text _statsLastScoreText;                                       // Reference to stats last score text
        [SerializeField]
        private GameObject[] _statsMessages;                                    // Reference to the slides of the controllers

        private UINavigation _uiNavigation = null;                              // Reference to the UINavigation component
        private bool _statsPanelActive = false;                                 // Flag for level stats panel
        private GameObject _statsMessage;                                       // Stats message to show

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

            // Set stats message depending on controller
            if (GameManager.Instance.GameController is Assets.Scripts.GameController.PS3GameController)
            {
                _statsMessage = _statsMessages[1];
            }
            else if (GameManager.Instance.GameController is Assets.Scripts.GameController.PS4GameController)
            {
                _statsMessage = _statsMessages[2];
            }
            else if (GameManager.Instance.GameController is Assets.Scripts.GameController.PSPGameController)
            {
                _statsMessage = _statsMessages[3];
            }
            else
            {
                // Keyboard
                _statsMessage = _statsMessages[0];
            }
        }

        // Update is called once per frame
        private void Update()
        {
            // Show stats message depending on the current button
            _statsMessage.SetActive(_uiNavigation.CurrentSelectableID > 0);
            //
            if (GameManager.Instance.GameController.Accept())
            {
                if (!_statsPanelActive)
                {
                    // Invoke method linked to the UIButton
                    _uiNavigation.Selectables[_uiNavigation.CurrentSelectableID].GetComponent<IUISelectable>()
                        .OnClickCallback.Invoke(_uiNavigation.Selectables[_uiNavigation.CurrentSelectableID].GetComponent<IUISelectable>());
                }    
            }
            // Manage stats panel
            ManageLevelStatsPanel();
        }

        private void ManageLevelStatsPanel()
        {
            if (_uiNavigation.CurrentSelectableID == 0)
                return;

            if (GameManager.Instance.GameController.Option())
            {
                if (_statsPanelActive)
                {
                    // Disable stats panel
                    _statsPanelActive = false;
                    _levelStats.SetActive(false);
                    // Enable button navigation
                    // Back button goes separate
                    Button l_button = _uiNavigation.Selectables[0].GetComponent<Button>();
                    Navigation l_buttonNavigation = l_button.navigation;
                    l_buttonNavigation.mode = Navigation.Mode.Explicit;
                    l_buttonNavigation.selectOnUp = _uiNavigation.Selectables[1];
                    l_button.navigation = l_buttonNavigation;
                    // Level buttons
                    SetButtonsForUnlockedLevels();
                    // Change to selected sprite after above method
                    _uiNavigation.Selectables[_uiNavigation.CurrentSelectableID].GetComponent<UIButton>().ChangeButtonSprite(EButtonSpriteType.Selected);
                }
                else
                {
                    // Disable button navigation
                    for (int i = 0; i < _uiNavigation.Selectables.Length; i++)
                    {
                        // Change navigation mode
                        Button l_button = _uiNavigation.Selectables[i].GetComponent<Button>();

                        Navigation l_buttonNavigation = l_button.navigation;

                        l_buttonNavigation.mode = Navigation.Mode.None;

                        l_button.navigation = l_buttonNavigation;
                    }
                    // Enable stats panel
                    _statsPanelActive = true;
                    _levelStats.SetActive(true);
                    // Retrieve level scores
                    int[] l_levelScores = GameManager.Instance.RetrieveLevelScores(_uiNavigation.CurrentSelectableID);
                    // Set level scores
                    if (l_levelScores != null)
                    {
                        _statsRibbonText.text = "Level " + _uiNavigation.CurrentSelectableID.ToString();
                        _statsMaxScoreText.text = l_levelScores[0].ToString();
                        _statsLastScoreText.text = l_levelScores[1].ToString();
                    }
                }
            }

            if (GameManager.Instance.GameController.Accept() || GameManager.Instance.GameController.Cancel())
            {
                if (_statsPanelActive)
                {
                    // Disable stats panel
                    _statsPanelActive = false;
                    _levelStats.SetActive(false);
                    // Enable button navigation
                    // Back button goes separate
                    Button l_button = _uiNavigation.Selectables[0].GetComponent<Button>();
                    Navigation l_buttonNavigation = l_button.navigation;
                    l_buttonNavigation.mode = Navigation.Mode.Explicit;
                    l_buttonNavigation.selectOnUp = _uiNavigation.Selectables[1];
                    l_button.navigation = l_buttonNavigation;
                    // Level buttons
                    SetButtonsForUnlockedLevels();
                    // Change to selected sprite after above method
                    _uiNavigation.Selectables[_uiNavigation.CurrentSelectableID].GetComponent<UIButton>().ChangeButtonSprite(EButtonSpriteType.Selected);
                }
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
