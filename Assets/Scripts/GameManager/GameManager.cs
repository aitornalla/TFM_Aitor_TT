using Assets.Scripts.CustomClasses;
using Assets.Scripts.GameController;
using Assets.Scripts.GameManagerController.States;
using Assets.Scripts.Player;
using Assets.Scripts.Scenes;
using Assets.Scripts.TimeTrialController;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.GameManagerController
{
	public sealed class GameManager : MonoBehaviour
	{
		// GameManager static instance
		private static GameManager _instance = null;
		// Instance to hold and manage game states
		private IGameManagerState _gameManagerState = null;
		// Scenes dictionary
		private Dictionary<EGameScenes, string> _gameScenesDictionary = null;
        // IGameController instance
		private IGameController _gameController = null;
		// Main AudioMixer instance
		private AudioMixerController _audioMixerController = null;
        // Player instance
		private GameObject _playerInstance = null;
		// Vector2 instance to hold current checkpoint spawn position
		private Vector2 _currentCheckPointSpawnPosition = Vector2.zero;
		// Main camera instance
		private Transform _mainCamera = null;
		// PrefabContainer instance
		private Transform _prefabContainer = null;
		// EnemyContainer instance
		private Transform _enemyContainer = null;
		// Next scene to load after level load scene
		private EGameScenes _levelLoadNextScene;
		// Black panel on canvas to make transition from death to respawn player
		private GameObject _deathRespawnBlackPanel = null;
		// Player lifes text in level UI
		private GameObject _playerLifesText = null;
		// Level score counter
		private GameObject _levelScoreCounter = null;
		// Time trial clock
		private GameObject _timeTrialClock = null;

        #region Pause variables
        // Pause flag
        private bool _isPaused = false;
		// Pause menu gameObject
		private GameObject _pauseMenuInstance = null;
		// Pause event for gameObjects that need it
		private BoolEvent _onPauseEvent = null;
        #endregion

        #region readonly variables
        // Path to xml file for controllers configuration
        private readonly string ControllersXMLPath = string.Join(Path.DirectorySeparatorChar.ToString(), new string[] { "Assets", "Resources", "ConfigFiles", "controllers.xml" });
		// Path to xml file for controllers configuration
		private readonly string ScenesXMLPath = string.Join(Path.DirectorySeparatorChar.ToString(), new string[] { "Assets", "Resources", "ConfigFiles", "scenes.xml" });
		// Path to xml file for controllers configuration
		private readonly string LevelsXMLPath = string.Join(Path.DirectorySeparatorChar.ToString(), new string[] { "Assets", "Resources", "ConfigFiles", "levels.xml" });
		// Initial player spawn position in levels
		private readonly Vector2 _initialPlayerSpawnPosition = new Vector2(0.0f, 5.0f);
		#endregion

		#region Properties
		public static GameManager Instance { get { return _instance; } }
        public IGameManagerState GameManagerState
        {
            get
            { return _instance._gameManagerState; }

            set
            { _instance._gameManagerState = value; }
        }
        public Dictionary<EGameScenes, string> GameScenesDictionary { get { return _instance._gameScenesDictionary; } }
        public IGameController GameController { get { return _instance._gameController; } }
        public AudioMixerController AudioMixerController { get { return _instance._audioMixerController; } }
        public GameObject PlayerInstance { get { return _instance._playerInstance; } }
        public Vector2 CurrentCheckPointSpawnPosition
		{
			get
			{ return _instance._currentCheckPointSpawnPosition; }

            set
			{ _instance._currentCheckPointSpawnPosition = value; }
		}
		public Transform PrefabContainer { get { return _instance._prefabContainer; } }
		public Transform EnemyContainer { get { return _instance._enemyContainer; } }
		public EGameScenes LevelLoadNextScene
        {
            get
            { return _instance._levelLoadNextScene; }
            set
            { _instance._levelLoadNextScene = value; }
        }
        public GameObject DeathRespawnBlackPanel { get { return _instance._deathRespawnBlackPanel; } }
		public GameObject PlayerLifesText { get { return _instance._playerLifesText; } }
        public GameObject LevelScoreCounter { get { return _instance._levelScoreCounter; } }
		public GameObject TimeTrialClock { get { return _instance._timeTrialClock; } }
		public int PlayerLifes { get; set; }
        public bool IsPlayerDead { get { return _instance._playerInstance.GetComponent<CharacterFlags>().IsDead; } }
        public bool IsPlayerControlAllowed { get { return _instance._playerInstance.GetComponent<CharacterFlags>().IsPlayerControlAllowed; } }
		public bool IsPaused { get { return _instance._isPaused; } }
        public BoolEvent OnPauseEvent { get { return _instance._onPauseEvent; } }
		#endregion

		#region Awake
		private void Awake ()
        {
			// Singleton creation
			if (_instance == null)
            {
				_instance = this;

				DontDestroyOnLoad (gameObject);

				// First state is game intro
				_instance._gameManagerState = new GameManagerStateIntro(_instance);

			}
            else
            {
				DestroyImmediate(gameObject);
			}

			// Game manager state Awake
			_instance._gameManagerState.StateAwake();
		}
        #endregion

        #region Start
        // Use this for initialization
        private void Start ()
        {
            
		}
		#endregion

		#region Update
		// Update is called once per frame
		private void Update ()
        {
			// Game manager state Update
			_instance._gameManagerState.StateUpdate();
		}
		#endregion

		#region Private methods
		#endregion

		#region Public methods
		/// <summary>
		///     For level scenes, assigns gameObjects to references needed
		/// </summary>
		public void AssignLevelReferences()
        {
			// Get player gameObject
			_instance._playerInstance = GameObject.FindGameObjectWithTag("Player");

			// Get main camera transform
			_instance._mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;

			// Get empty gameObject to place instantiations prefabs
			_instance._prefabContainer = GameObject.FindGameObjectWithTag("PrefabContainer").transform;

			// Get gameObject where enemies are placed
			_instance._enemyContainer = GameObject.FindGameObjectWithTag("EnemyContainer").transform;

			// Assign initial player spawn position
			_instance._currentCheckPointSpawnPosition = _instance._initialPlayerSpawnPosition;

			// Get pause menu gameObject instance
			_instance._pauseMenuInstance = GameObject.FindGameObjectWithTag("PauseMenu");

			// Initialize OnPauseEvent
			_instance._onPauseEvent = new BoolEvent();

			// Get black panel on canvas for transition from death to respawn
			_instance._deathRespawnBlackPanel = GameObject.FindGameObjectWithTag("DeathRespawnBlackPanel");

			// Get player lifes text on canvas for displaying remaining lifes
			_instance._playerLifesText = GameObject.FindGameObjectWithTag("PlayerLifesText");

			// Get level score counter reference
			_instance._levelScoreCounter = GameObject.FindGameObjectWithTag("LevelScoreCounter");

			// Get time trial clock gameObject and set it to enabled/disabled
			_instance._timeTrialClock = GameObject.FindGameObjectWithTag("TimeTrialClock");
			string l_levelName = SceneManager.GetActiveScene().name;
			_instance._timeTrialClock.SetActive(_instance.IsTimeTrialEnabled(l_levelName));
		}

		/// <summary>
        ///     Flag for level time trial enabled
        /// </summary>
        /// <param name="levelName">Current level</param>
        /// <returns><code>true</code> if level time trial is enabled, otherwise <code>false</code></returns>
		public bool IsTimeTrialEnabled(string levelName)
		{
			// Load levels xml file
			XDocument l_xDoc = XDocument.Load(_instance.LevelsXMLPath);
			// Find level elements by attribute ("name")
			XElement l_currentLevelXElem = l_xDoc.Descendants("level").Where(atr => (string)atr.Attribute("name") == levelName).FirstOrDefault();
			// Initialize flag
			bool l_currentTimeTrialLevel = false;
			// Parse current level time trial status
			bool.TryParse(l_currentLevelXElem.Element("timeTrialEnabled").Value, out l_currentTimeTrialLevel);
            // Return flag
			return l_currentTimeTrialLevel;
		}

		/// <summary>
		///     Loads scenes names linked to EGameScenes enum into a dictionary
		/// </summary>
		public void LoadSceneDictionary()
        {
            // Instantiate game scenes dictionary
			_instance._gameScenesDictionary = new Dictionary<EGameScenes, string>();
			// Load scenes xml file
			XDocument l_xDoc = XDocument.Load(_instance.ScenesXMLPath);
            // Get all descendant elements
			XElement[] l_xElems = l_xDoc.Descendants("scene").ToArray();
			// Fill in the dictionary
			for (int i = 0; i < l_xElems.Length; i++)
            {
				string l_sceneName = l_xElems[i].Attribute("name").Value;
                string l_enumValue = l_xElems[i].Attribute("enumValue").Value;

                if (Enum.IsDefined(typeof(EGameScenes), l_enumValue))
                {
					_instance._gameScenesDictionary.Add((EGameScenes)Enum.Parse(typeof(EGameScenes), l_enumValue), l_sceneName);
                }
			}
		}

        /// <summary>
		///     Manages game pause
		/// </summary>
		public void ManagePause()
		{
			// Change is paused flag
			_instance._isPaused = !_instance._isPaused;
			// Enable/Disable pause menu
			_instance._pauseMenuInstance.SetActive(_instance._isPaused);
			// Change time scale
			if (_instance._isPaused)
			{
                // Change time scale
				Time.timeScale = 0.0f;
                // If time trial
                if (_instance._timeTrialClock.GetComponent<TimeTrial>().IsTimeTrial)
                {
					_instance._timeTrialClock.GetComponent<TimeTrial>().PauseTimeTrial();
				}
			}
			else
			{
                // Change time scale
				Time.timeScale = 1.0f;
				// If time trial
				if (_instance._timeTrialClock.GetComponent<TimeTrial>().IsTimeTrial)
				{
					_instance._timeTrialClock.GetComponent<TimeTrial>().ResumeTimeTrial();
				}
			}

			// Call event
			_instance._onPauseEvent.Invoke(_instance._isPaused);
		}

		/// <summary>
		///     Manages player death and respawn
		/// </summary>
		public void ManagePlayerDeathAndRespawn()
        {
			// If time trial then reload the level
			if (_instance._timeTrialClock.GetComponent<TimeTrial>().IsTimeTrial)
			{
                // Get scene name
				string l_levelName = SceneManager.GetActiveScene().name;
                // Prepare game scene enum
				EGameScenes l_gameScene = EGameScenes.MainMenu;
                // Look for game scene enum
				l_gameScene = _instance._gameScenesDictionary.FirstOrDefault(x => x.Value == l_levelName).Key;
                // Change state
				_instance.GameManagerState.StateChange(l_gameScene);
				// Return
				return;
			}

			// Manage player lifes
			_instance.PlayerLifes--;
			// Update remaining lifes
			_instance._playerLifesText.GetComponent<Text>().text = "x" + _instance.PlayerLifes.ToString();
            // If no more lifes left, return to main menu
			if (_instance.PlayerLifes == 0)
            {
				_instance._gameManagerState.StateChange(EGameScenes.MainMenu);

				return;
            }

            // Get CharacterHealth component from player instance
			CharacterHealth l_characterHealth = _instance._playerInstance.GetComponent<CharacterHealth>();
            // Restore maximum health
			l_characterHealth.RestoreHealth(l_characterHealth.PlayerMaxHealth);

            // Set player gameObject to inactive
			_instance._playerInstance.SetActive(false);
            // Translate player gameObject to respawn position
			_instance._playerInstance.transform.Translate(
				_instance._currentCheckPointSpawnPosition.x - _instance._playerInstance.transform.position.x,
				_instance._currentCheckPointSpawnPosition.y - _instance._playerInstance.transform.position.y,
				0.0f);
			// Translate main camera gameObject to respawn position
			_instance._mainCamera.Translate(
				_instance._currentCheckPointSpawnPosition.x - _instance._playerInstance.transform.position.x,
				_instance._currentCheckPointSpawnPosition.y - _instance._playerInstance.transform.position.y,
				0.0f);
			// Set player not dead flag
			_instance._playerInstance.GetComponent<CharacterFlags>().IsDead = false;
            // Reset player control flags
			_instance._playerInstance.GetComponent<CharacterInputController2D>().ControlFlags.ResetFlags();
            // Set player gameObject to active
			_instance._playerInstance.SetActive(true);
			// Once everything is set up, set black panel inactive
			_instance._deathRespawnBlackPanel.SetActive(false);
		}

		/// <summary>
		///     Called when SceneManager has finished loading the scene
		/// </summary>
		/// <param name="scene">Loaded scene</param>
		/// <param name="mode">Mode used to load the scene</param>
		public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			_instance.GameManagerState.StateOnSceneLoaded(scene, mode);
		}

		/// <summary>
		///     Called when SceneManager has finished unloading the scene
		/// </summary>
		/// <param name="scene">Unloaded scene</param>
		public void OnSceneUnLoaded(Scene scene)
		{
			_instance.GameManagerState.StateOnSceneUnLoaded(scene);
		}

        /// <summary>
        ///     Retrieve max and last level score by level index
        /// </summary>
        /// <param name="levelIndex">Level index</param>
        /// <returns>Array with max score and last score</returns>
        public int[] RetrieveLevelScores(int levelIndex)
        {
			int[] l_levelScores = new int[2];

			// Load levels xml file
			XDocument l_xDoc = XDocument.Load(_instance.LevelsXMLPath);
			// Find level elements by attribute ("index")
			XElement l_xElem = l_xDoc.Descendants("level").Where(atr => (string)atr.Attribute("index") == levelIndex.ToString()).FirstOrDefault();
			// Retrieve last score and max score
			if (l_xElem != null)
			{
				// Retrieve max score
				int l_levelMaxScore = 0;

                if (int.TryParse(l_xElem.Element("maxScore").Value, out l_levelMaxScore))
                {
					l_levelScores[0] = l_levelMaxScore;
                }

				// Retrieve last score
				int l_levelLastScore = 0;

                if (int.TryParse(l_xElem.Element("lastScore").Value, out l_levelLastScore))
                {
					l_levelScores[1] = l_levelLastScore;
                }
			}
            else
            {
				return null;
            }
            // Return scores
			return l_levelScores;
        }

		/// <summary>
		///     Retrieve level times
		/// </summary>
		/// <param name="levelName">Level name</param>
		/// <returns>Array with first, second and third times</returns>
		public string[] RetrieveLevelTimes(string levelName)
        {
			// Load levels xml file
			XDocument l_xDoc = XDocument.Load(_instance.LevelsXMLPath);
			// Find level elements by attribute ("name")
			XElement l_xElem = l_xDoc.Descendants("level").Where(atr => (string)atr.Attribute("name") == levelName).FirstOrDefault();
            // Retrieve level index
            if (l_xElem != null)
            {
				int l_index = 0;

				if (int.TryParse(l_xElem.Attribute("index").Value, out l_index))
                {
					return _instance.RetrieveLevelTimes(l_index);
                }
            }
			//
			return null;
		}

		/// <summary>
		///     Retrieve level times
		/// </summary>
		/// <param name="levelIndex">Level index</param>
		/// <returns>Array with first, second and third times</returns>
		public string[] RetrieveLevelTimes(int levelIndex)
        {
			string[] l_levelTimes = new string[3];

			string l_noTime = "--:--:---";

			// Load levels xml file
			XDocument l_xDoc = XDocument.Load(_instance.LevelsXMLPath);
			// Find level elements by attribute ("index")
			XElement l_xElem = l_xDoc.Descendants("level").Where(atr => (string)atr.Attribute("index") == levelIndex.ToString()).FirstOrDefault();
			// Retrieve last score and max score
			if (l_xElem != null)
			{
				// First time
				if (string.IsNullOrEmpty(l_xElem.Element("timeTrialFirstTime").Value))
				{
					l_levelTimes[0] = l_noTime;
                }
                else
                {
					int l_index = l_xElem.Element("timeTrialFirstTime").Value.IndexOf(":");

					l_levelTimes[0] = l_xElem.Element("timeTrialFirstTime").Value.Substring(l_index + 1).Replace('.', ':');
				}
				// Second time
				if (string.IsNullOrEmpty(l_xElem.Element("timeTrialSecondTime").Value))
				{
					l_levelTimes[1] = l_noTime;
				}
				else
				{
					int l_index = l_xElem.Element("timeTrialSecondTime").Value.IndexOf(":");

					l_levelTimes[1] = l_xElem.Element("timeTrialSecondTime").Value.Substring(l_index + 1).Replace('.', ':');
				}
				// Third time
				if (string.IsNullOrEmpty(l_xElem.Element("timeTrialThirdTime").Value))
				{
					l_levelTimes[2] = l_noTime;
				}
				else
				{
					int l_index = l_xElem.Element("timeTrialThirdTime").Value.IndexOf(":");

					l_levelTimes[2] = l_xElem.Element("timeTrialThirdTime").Value.Substring(l_index + 1).Replace('.', ':');
				}
			}
            else
            {
				return null;
            }
			// Return times
			return l_levelTimes;
		}

		/// <summary>
		///     Create AudioMixerController instance
		/// </summary>
		public void SetUpAudioMixerController()
        {
			_instance._audioMixerController = new AudioMixerController();
        }

		/// <summary>
		/// 	Adds controller components to GameManager gameObject.
		/// 	One component must implement interface <see cref="IGameController"/> and the other must implement specific methods from controller buttons/axis
		/// </summary>
		public void SetUpController()
		{
			try
			{
				// Get controller name
				string l_controllerName = Input.GetJoystickNames()[0];
				// Load controllers xml file
				XDocument l_xDoc = XDocument.Load(_instance.ControllersXMLPath);
				// Find controller by attribute ("name")
				XElement l_xElem = l_xDoc.Descendants().Where(atr => (string)atr.Attribute("name") == l_controllerName).FirstOrDefault();
				// If null, controller not in the list
				if (l_xElem == null)
				{
					throw new ArgumentNullException("l_xElem", "Connected controller not in the controllers list");
				}
				// Combine namespace and name to get full name
				string l_fullName = string.Join(".", new string[] { l_xElem.Element("namespace").Value, l_xElem.Element("name").Value });
				// Add IGameController component
				_instance._gameController = (IGameController)gameObject.AddComponent(Type.GetType(l_fullName));
				// Read debug attribute
				bool l_debugController = false;
				bool.TryParse(l_xElem.Attribute("debug").Value, out l_debugController);
				// If debug enabled ...
				if (l_debugController)
				{
					_instance._gameController.ControllerDebug(true);

					Debug.Log("Controller: " + l_controllerName);
				}
			}
			catch (IndexOutOfRangeException e)
			{
				Debug.LogException(e);
				Debug.LogWarning("No controller connected");
			}
			catch (ArgumentNullException e)
			{
				Debug.LogException(e);
			}
			catch (Exception e)
			{
				Debug.LogException(e);
			}
			finally
			{
				if (_instance._gameController == null)
				{
					_instance._gameController = (IGameController)gameObject.AddComponent<KeyboardGameController>();

					Debug.Log("Default controller: keyboard");
				}

				// Enables controller debug in development mode
				//_instance._gameControllerInstance.ControllerDebug (true);
			}
		}

        /// <summary>
        ///     Returns array of index of unlocked levels
        /// </summary>
        /// <returns></returns>
        public int[] UnlockedLevels()
        {
			List<int> l_unlockedLevelsList = new List<int>();

			// Load levels xml file
			XDocument l_xDoc = XDocument.Load(_instance.LevelsXMLPath);
			// 
			XElement[] l_levelXElems = l_xDoc.Descendants("level").ToArray();

            for (int i = 0; i < l_levelXElems.ToArray().Length; i++)
            {
				bool l_levelUnlocked = false;

                if (bool.TryParse(l_levelXElems[i].Element("unlocked").Value, out l_levelUnlocked))
                {
                    if (l_levelUnlocked)
                    {
						int l_index = 0;

                        if (int.TryParse(l_levelXElems[i].Attribute("index").Value, out l_index))
                        {
							l_unlockedLevelsList.Add(l_index);
                        }
                    }
                }
            }

			return l_unlockedLevelsList.ToArray();
        }

		/// <summary>
		///     Unlock level time trial
		/// </summary>
		/// <param name="levelName">Current level</param>
		public void UnlockLevelTimeTrial(string levelName)
		{
			// Load levels xml file
			XDocument l_xDoc = XDocument.Load(_instance.LevelsXMLPath);
			// Find level elements by attribute ("name")
			XElement l_currentLevelXElem = l_xDoc.Descendants("level").Where(atr => (string)atr.Attribute("name") == levelName).FirstOrDefault();
			// Initialize flag
			bool l_currentTimeTrialLevel = false;
            // Parse current level time trial status
            if (bool.TryParse(l_currentLevelXElem.Element("timeTrialEnabled").Value, out l_currentTimeTrialLevel))
            {
                // If time trial not enabled
                if (!l_currentTimeTrialLevel)
                {
                    // Change value
					l_currentLevelXElem.Element("timeTrialEnabled").Value = "true";
					// Save changes
					l_xDoc.Save(_instance.LevelsXMLPath);
				}
            }
		}

		/// <summary>
		///     Unlock next level
		/// </summary>
		/// <param name="levelName">Current level</param>
		public void UnlockNextLevel(string levelName)
        {
			// Load levels xml file
			XDocument l_xDoc = XDocument.Load(_instance.LevelsXMLPath);
			// Find level elements by attribute ("name")
			XElement l_currentLevelXElem = l_xDoc.Descendants("level").Where(atr => (string)atr.Attribute("name") == levelName).FirstOrDefault();
            // Initialize index to 0
			int l_currentLevelIndex = 0;
            // Parse current level index and unlock next level if there is one
            if (int.TryParse(l_currentLevelXElem.Attribute("index").Value, out l_currentLevelIndex))
            {
                // Next level index
				float l_nextLevelIndex = l_currentLevelIndex + 1;
				// Find next level by attribute ("index")
				XElement l_nextLevelXElem = l_xDoc.Descendants("level").Where(atr => (string)atr.Attribute("index") == l_nextLevelIndex.ToString()).FirstOrDefault();
                // If there is a next level
                if (l_nextLevelXElem != null)
                {
                    // Check if next level is already unlocked
					bool l_nextLevelUnlocked = false;
                    // Parse bool value for unlocked element
					bool.TryParse(l_nextLevelXElem.Element("unlocked").Value, out l_nextLevelUnlocked);
                    // If next level is not unlocked
                    if (!l_nextLevelUnlocked)
                    {
						// Unlock level
						l_nextLevelXElem.Element("unlocked").Value = "true";
						// Save changes
						l_xDoc.Save(_instance.LevelsXMLPath);
					}
				}
			}
		}

        /// <summary>
        ///     Updates maximum and last score of the played level
        /// </summary>
        /// <param name="levelName">Level name</param>
        /// <param name="score">Level score</param>
        public void UpdateLevelScore(string levelName, int score)
        {
			// Load levels xml file
			XDocument l_xDoc = XDocument.Load(_instance.LevelsXMLPath);
			// Find level elements by attribute ("name")
			XElement l_xElem = l_xDoc.Descendants("level").Where(atr => (string)atr.Attribute("name") == levelName).FirstOrDefault();
            // Update last score and max score
            if (l_xElem != null)
            {
                // Update last score
				XElement l_lastScoreXElem = l_xElem.Element("lastScore");

				l_lastScoreXElem.Value = score.ToString();

				// Update max score if it is greater than the current one
				XElement l_maxScoreXElem = l_xElem.Element("maxScore");

				int l_maxScore = 0;

                if (int.TryParse(l_maxScoreXElem.Value, out l_maxScore))
                {
					if (score > l_maxScore)
						l_maxScoreXElem.Value = score.ToString();
                }
			}
            // Save changes
			l_xDoc.Save(_instance.LevelsXMLPath);
		}

		/// <summary>
		///     Updates level times
		/// </summary>
		/// <param name="levelName">Level name</param>
		/// <param name="levelTime">Level time</param>
		public void UpdateLevelTime(string levelName, TimeSpan levelTime)
        {
			// Load levels xml file
			XDocument l_xDoc = XDocument.Load(_instance.LevelsXMLPath);
			// Find level elements by attribute ("name")
			XElement l_xElem = l_xDoc.Descendants("level").Where(atr => (string)atr.Attribute("name") == levelName).FirstOrDefault();
			// Update last score and max score
			if (l_xElem != null)
			{
                // First time
                if (string.IsNullOrEmpty(l_xElem.Element("timeTrialFirstTime").Value))
                {
                    // Format time value
					l_xElem.Element("timeTrialFirstTime").Value =
						levelTime.Hours.ToString("D2") + ":" +
						levelTime.Minutes.ToString("D2") + ":" +
						levelTime.Seconds.ToString("D2") + "." +
						levelTime.Milliseconds.ToString("D3");
					// Save changes
					l_xDoc.Save(_instance.LevelsXMLPath);
					// Go back
					return;
				}
                else
                {
                    // Prepare Timespan variable
					TimeSpan l_firstLevelTime;
					// Parse Timespan
					if (TimeSpan.TryParse(l_xElem.Element("timeTrialFirstTime").Value, out l_firstLevelTime))
                    {
                        if (levelTime < l_firstLevelTime)
                        {
							// Set third time with second time
							l_xElem.Element("timeTrialThirdTime").Value =
								l_xElem.Element("timeTrialSecondTime").Value;
							// Set second time with first time
							l_xElem.Element("timeTrialSecondTime").Value =
								l_xElem.Element("timeTrialFirstTime").Value;
							// Set first time
							l_xElem.Element("timeTrialFirstTime").Value =
						        levelTime.Hours.ToString("D2") + ":" +
						        levelTime.Minutes.ToString("D2") + ":" +
						        levelTime.Seconds.ToString("D2") + "." +
						        levelTime.Milliseconds.ToString("D3");
							// Save changes
							l_xDoc.Save(_instance.LevelsXMLPath);
							// Go back
							return;
						}
                    }
                }

				// Second time
				if (string.IsNullOrEmpty(l_xElem.Element("timeTrialSecondTime").Value))
				{
					// Format time value
					l_xElem.Element("timeTrialSecondTime").Value =
						levelTime.Hours.ToString("D2") + ":" +
						levelTime.Minutes.ToString("D2") + ":" +
						levelTime.Seconds.ToString("D2") + "." +
						levelTime.Milliseconds.ToString("D3");
					// Save changes
					l_xDoc.Save(_instance.LevelsXMLPath);
					// Go back
					return;
				}
				else
				{
					// Prepare Timespan variable
					TimeSpan l_secondLevelTime;
					// Parse Timespan
					if (TimeSpan.TryParse(l_xElem.Element("timeTrialSecondTime").Value, out l_secondLevelTime))
					{
						if (levelTime < l_secondLevelTime)
						{
							// Set third time with second time
							l_xElem.Element("timeTrialThirdTime").Value =
								l_xElem.Element("timeTrialSecondTime").Value;
							// Set second time
							l_xElem.Element("timeTrialSecondTime").Value =
								levelTime.Hours.ToString("D2") + ":" +
								levelTime.Minutes.ToString("D2") + ":" +
								levelTime.Seconds.ToString("D2") + "." +
								levelTime.Milliseconds.ToString("D3");
							// Save changes
							l_xDoc.Save(_instance.LevelsXMLPath);
							// Go back
							return;
						}
					}
				}

				// Third time
				if (string.IsNullOrEmpty(l_xElem.Element("timeTrialThirdTime").Value))
				{
					// Format time value
					l_xElem.Element("timeTrialThirdTime").Value =
						levelTime.Hours.ToString("D2") + ":" +
						levelTime.Minutes.ToString("D2") + ":" +
						levelTime.Seconds.ToString("D2") + "." +
						levelTime.Milliseconds.ToString("D3");
					// Save changes
					l_xDoc.Save(_instance.LevelsXMLPath);
					// Go back
					return;
				}
				else
				{
					// Prepare Timespan variable
					TimeSpan l_thirdLevelTime;
					// Parse Timespan
					if (TimeSpan.TryParse(l_xElem.Element("timeTrialThirdTime").Value, out l_thirdLevelTime))
					{
						if (levelTime < l_thirdLevelTime)
						{
							// Set third time
							l_xElem.Element("timeTrialThirdTime").Value =
								levelTime.Hours.ToString("D2") + ":" +
								levelTime.Minutes.ToString("D2") + ":" +
								levelTime.Seconds.ToString("D2") + "." +
								levelTime.Milliseconds.ToString("D3");
							// Save changes
							l_xDoc.Save(_instance.LevelsXMLPath);
							// Go back
							return;
						}
					}
				}
			}
			// Parse example
			//TimeSpan timeSpan;
			//TimeSpan.TryParse("0:12:22.435", out timeSpan);
			//Debug.Log(timeSpan.Minutes + ":" + timeSpan.Seconds + ":" + timeSpan.Milliseconds);
		}
		#endregion
	}

	#region Events
	[Serializable]
	public class BoolEvent : UnityEvent<bool>
	{

	}
    #endregion
}

