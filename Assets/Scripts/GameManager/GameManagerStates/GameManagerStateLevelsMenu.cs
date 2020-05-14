using Assets.Scripts.Scenes;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.GameManagerController.States
{
    public sealed class GameManagerStateLevelsMenu : IGameManagerState
    {
        private static GameManager _gameManagerInstance = null;

        private IGameManagerState _nextState = null;                            // To hold next state until scenes unloads

        public GameManagerStateLevelsMenu(GameManager gameManager)
        {
            _gameManagerInstance = gameManager;
        }

        #region IGameManagerState implementation
        public void StateAwake()
        {
            _gameManagerInstance.SetSceneGameManagerAudioSource();
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

                case EGameScenes.Level_01:
                case EGameScenes.Level_02:
                case EGameScenes.Level_03:
                case EGameScenes.Level_04:
                    {
                        // Level scene to be loaded next
                        _gameManagerInstance.LevelLoadNextScene = gameScenes;
                        // Assing new game state
                        _nextState = new GameManagerStateLevelLoad(_gameManagerInstance);
                        // Load LevelLoad scene that will load the next level asynchronously
                        gameScenes = EGameScenes.LevelLoad;
                    }
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
