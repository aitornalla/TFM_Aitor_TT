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

        public GameManagerStateIntro(GameManager gameManager)
        {
            _gameManagerInstance = gameManager;
        }

        #region IGameManagerState implementation
        public void StateAwake()
        {
            _gameManagerInstance.SetUpController();
        }

        public void StateStart()
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
            _gameManagerInstance.GameManagerState = new GameManagerStateMainMenu(_gameManagerInstance);

            // Load next scene -> main menu
            string l_scene = string.Empty;

            if (_gameManagerInstance.GameScenesDictionary.TryGetValue(gameScenes, out l_scene))
                SceneManager.LoadScene(l_scene);
        }
        #endregion
    }
}
