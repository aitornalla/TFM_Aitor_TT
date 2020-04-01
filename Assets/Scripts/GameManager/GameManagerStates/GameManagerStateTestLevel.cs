using Assets.Scripts.Scenes;
using System;

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
            _gameManagerInstance.SetUpController();
            _gameManagerInstance.AssignLevelReferences();
        }

        public void StateStart()
        {
            //throw new NotImplementedException();
        }

        public void StateUpdate()
        {
            // When player press pause
            if (_gameManagerInstance.GameController.Pause())
            {
                _gameManagerInstance.ManagePause();
            }
        }

        public void StateChange()
        {

        }
        #endregion
    }
}
