using Assets.Scripts.GameController;
using Assets.Scripts.GameManagerController.States;
using Assets.Scripts.Player;
using Assets.Scripts.Scenes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

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
		// Player instance
		private GameObject _playerInstance = null;
		// Vector2 instance to hold current checkpoint spawn position
		private Vector2 _currentCheckPointSpawnPosition = Vector2.zero;
		// Main camera instance
		private Transform _mainCamera = null;
		// PrefabContainer instance
		private Transform _prefabContainer = null;
		// Next scene to load after level load scene
		private EGameScenes _levelLoadNextScene;

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
        private readonly string ControllersXMLPath = string.Join(Path.DirectorySeparatorChar.ToString(), new string[] { "Assets", "ConfigFiles", "controllers.xml" });
		// Path to xml file for controllers configuration
		private readonly string ScenesXMLPath = string.Join(Path.DirectorySeparatorChar.ToString(), new string[] { "Assets", "ConfigFiles", "scenes.xml" });
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
        public Vector2 CurrentCheckPointSpawnPosition
		{
			get
			{ return _instance._currentCheckPointSpawnPosition; }

            set
			{ _instance._currentCheckPointSpawnPosition = value; }
		}
        public Transform PrefabContainer { get { return _instance._prefabContainer; } }
        public EGameScenes LevelLoadNextScene
        {
            get
            { return _instance._levelLoadNextScene; }
            set
            { _instance._levelLoadNextScene = value; }
        }
        public bool IsPlayerDead { get { return _instance._playerInstance.GetComponent<CharacterFlags>().IsDead; } }
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

				// Load scenes dictionary
				_instance.LoadSceneDictionary();

                // Link GameManager functions to scenes load/unload events
				SceneManager.sceneLoaded += OnSceneLoaded;
				SceneManager.sceneUnloaded += OnSceneUnLoaded;

				// First state is game intro
				_instance._gameManagerState = new GameManagerStateIntro(_instance);

			} else {

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
        /// <summary>
        ///     Called when SceneManager has finished loading the scene
        /// </summary>
        /// <param name="scene">Loaded scene</param>
        /// <param name="mode">Mode used to load the scene</param>
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			_instance.GameManagerState.StateOnSceneLoaded(scene, mode);
		}

        /// <summary>
        ///     Called when SceneManager has finished unloading the scene
        /// </summary>
        /// <param name="scene">Unloaded scene</param>
		private void OnSceneUnLoaded(Scene scene)
		{
			_instance.GameManagerState.StateOnSceneUnLoaded(scene);
		}
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

			// Assign initial player spawn position
			_instance._currentCheckPointSpawnPosition = _instance._initialPlayerSpawnPosition;

			// Get pause menu gameObject instance
			_instance._pauseMenuInstance = GameObject.FindGameObjectWithTag("PauseMenu");

			// Initialize OnPauseEvent
			_instance._onPauseEvent = new BoolEvent();
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
				Time.timeScale = 0.0f;
			}
			else
			{
				Time.timeScale = 1.0f;
			}

			// Call event
			_instance._onPauseEvent.Invoke(_instance._isPaused);
		}

		/// <summary>
		///     Manages player death and respawn
		/// </summary>
		public void ManagePlayerDeathAndRespawn()
        {
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
            // Set player gameObject to active
			_instance._playerInstance.SetActive(true);
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
		#endregion
	}

	#region Events
	[Serializable]
	public class BoolEvent : UnityEvent<bool>
	{

	}
    #endregion
}

