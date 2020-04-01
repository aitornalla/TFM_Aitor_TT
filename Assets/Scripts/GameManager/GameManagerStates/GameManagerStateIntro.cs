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
            //throw new NotImplementedException();
        }

        public void StateStart()
        {
            //throw new NotImplementedException();
        }

        public void StateUpdate()
        {
            //throw new NotImplementedException();
        }

        public void StateChange()
        {
            // Assign new game state
            _gameManagerInstance.GameManagerState = new GameManagerStateTestLevel(_gameManagerInstance);

            // Load next scene
            string l_scene = string.Empty;

            if (_gameManagerInstance.GameScenesDictionary.TryGetValue(EGameScenes.TestLevel, out l_scene))
                SceneManager.LoadScene(l_scene);
        }
        #endregion
    }
}
