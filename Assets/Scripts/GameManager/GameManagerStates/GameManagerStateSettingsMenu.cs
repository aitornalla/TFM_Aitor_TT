using Assets.Scripts.Scenes;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.GameManagerController.States
{
    public sealed class GameManagerStateSettingsMenu : IGameManagerState
    {
        private static GameManager _gameManagerInstance = null;

        private IGameManagerState _nextState = null;                            // To hold next state until scenes unloads

        public GameManagerStateSettingsMenu(GameManager gameManager)
        {
            _gameManagerInstance = gameManager;
        }

        #region IGameManagerState implementation
        public void StateAwake()
        {
            //throw new NotImplementedException();
        }

        public void StateUpdate()
        {
            //throw new NotImplementedException();
        }

        public void StateChange(EGameScenes gameScenes)
        {
            // Assign new game state
            switch (gameScenes)
            {
                case EGameScenes.MainMenu:
                    _nextState = new GameManagerStateMainMenu(_gameManagerInstance);
                    break;

                default:
                    _nextState = new GameManagerStateMainMenu(_gameManagerInstance);
                    break;
            }

            // Load next scene
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
