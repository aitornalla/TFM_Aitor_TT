using Assets.Scripts.Scenes;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.GameManagerController.States
{
    public sealed class GameManagerStateLevelLoad : IGameManagerState
    {
        private static GameManager _gameManagerInstance = null;

        public GameManagerStateLevelLoad(GameManager gameManager)
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
                case EGameScenes.TestLevel:
                case EGameScenes.Level_01:
                case EGameScenes.Level_02:
                case EGameScenes.Level_03:
                case EGameScenes.Level_04:
                    _gameManagerInstance.GameManagerState = new GameManagerStateLevel(_gameManagerInstance);
                    break;

                //case EGameScenes.TestLevel:
                //    _gameManagerInstance.GameManagerState = new GameManagerStateTestLevel(_gameManagerInstance);
                //    break;

                default:
                    _gameManagerInstance.GameManagerState = new GameManagerStateMainMenu(_gameManagerInstance);
                    break;
            }
        }

        public void StateOnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            GameObject.FindObjectOfType<LevelLoad>().LoadLevelSceneAsync();
        }

        public void StateOnSceneUnLoaded(Scene scene)
        {
            // When scene finishes unloading, change game state to next
            StateChange(GameManager.Instance.LevelLoadNextScene);
        }
        #endregion
    }
}
