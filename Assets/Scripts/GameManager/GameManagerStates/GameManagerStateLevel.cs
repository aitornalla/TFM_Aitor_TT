using Assets.Scripts.Scenes;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.GameManagerController.States
{
    public sealed class GameManagerStateLevel : IGameManagerState
    {
        private static GameManager _gameManagerInstance = null;

        private IGameManagerState _nextState = null;                            // To hold next state until scenes unloads

        public GameManagerStateLevel(GameManager gameManager)
        {
            _gameManagerInstance = gameManager;
        }

        #region IGameManagerState implementation
        public void StateAwake()
        {
            _gameManagerInstance.AssignLevelReferences();

            // First player lifes assigment
            if (_gameManagerInstance.PlayerLifes == 0)
            {
                _gameManagerInstance.PlayerLifes =
                    _gameManagerInstance.PlayerInstance.GetComponent<Assets.Scripts.Player.CharacterHealth>().PlayerMaxLifes;
            }

            _gameManagerInstance.PlayerLifesText.GetComponent<Text>().text = "x" + _gameManagerInstance.PlayerLifes.ToString();

            _gameManagerInstance.SetSceneGameManagerAudioSource();
        }

        public void StateUpdate()
        {
            // When player press pause
            if (_gameManagerInstance.GameController.Pause() &&
                !_gameManagerInstance.IsPlayerDead &&
                _gameManagerInstance.IsPlayerControlAllowed)
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
                    _nextState = new GameManagerStateMainMenu(_gameManagerInstance);
                    break;

                case EGameScenes.LevelsMenu:
                    _nextState = new GameManagerStateLevelsMenu(_gameManagerInstance);
                    break;

                case EGameScenes.Level_01:
                case EGameScenes.Level_02:
                case EGameScenes.Level_03:
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
