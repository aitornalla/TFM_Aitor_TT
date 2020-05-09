using Assets.Scripts.Scenes;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.GameManagerController.States
{
    public sealed class GameManagerStateIntro : IGameManagerState
    {
        private static GameManager _gameManagerInstance = null;

        private IGameManagerState _nextState = null;                            // To hold next state until scenes unloads

        public GameManagerStateIntro(GameManager gameManager)
        {
            _gameManagerInstance = gameManager;
        }

        #region IGameManagerState implementation
        public void StateAwake()
        {
            // Set resolution
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
            // Load scenes dictionary
            _gameManagerInstance.LoadSceneDictionary();
            // Link GameManager functions to scenes load/unload events
            SceneManager.sceneLoaded += _gameManagerInstance.OnSceneLoaded;
            SceneManager.sceneUnloaded += _gameManagerInstance.OnSceneUnLoaded;
            // Set up the controller
            _gameManagerInstance.SetUpController();
            // Set up main audio settings
            _gameManagerInstance.SetUpAudioSettings();
        }

        public void StateUpdate()
        {
            //throw new NotImplementedException();
        }

        public void StateChange(EGameScenes gameScenes)
        {
            // Assign new game state
            _nextState = new GameManagerStateMainMenu(_gameManagerInstance);

            // Load next scene -> main menu
            string l_scene = string.Empty;

            if (_gameManagerInstance.GameScenesDictionary.TryGetValue(gameScenes, out l_scene))
                SceneManager.LoadScene(l_scene);
        }

        public void StateOnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            //throw new NotImplementedException();
        }

        public void StateOnSceneUnLoaded(Scene scene)
        {
            // Assing next state when scenes finishes unloading
            _gameManagerInstance.GameManagerState = _nextState;
        }
        #endregion
    }
}
