using Assets.Scripts.Scenes;
using System;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.GameManagerController.States
{
    public sealed class GameManagerStateTestLevel : IGameManagerState
    {
        private static GameManager _gameManagerInstance = null;

        public GameManagerStateTestLevel(GameManager gameManager)
        {
            _gameManagerInstance = gameManager;
        }

        #region IGameManagerState implementation
        public void StateAwake()
        {
            _gameManagerInstance.AssignLevelReferences();
        }

        public void StateStart()
        {
            //throw new NotImplementedException();
        }

        public void StateUpdate()
        {
            // When player press pause
            if (_gameManagerInstance.GameController.Pause() && !_gameManagerInstance.IsPlayerDead)
            {
                _gameManagerInstance.ManagePause();
            }
        }

        public void StateChange(EGameScenes gameScenes)
        {
            // Assign new game state
            switch (gameScenes)
            {
                case EGameScenes.MainMenu:
                    _gameManagerInstance.GameManagerState = new GameManagerStateMainMenu(_gameManagerInstance);
                    break;

                default:
                    _gameManagerInstance.GameManagerState = new GameManagerStateMainMenu(_gameManagerInstance);
                    break;
            }

            // Load next scene
            string l_scene = string.Empty;

            if (_gameManagerInstance.GameScenesDictionary.TryGetValue(gameScenes, out l_scene))
                SceneManager.LoadScene(l_scene);
        }
        #endregion
    }
}
