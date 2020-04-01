using Assets.Scripts.Scenes;
using System;

namespace Assets.Scripts.GameManagerController.States
{
    public interface IGameManagerState
    {
        void StateAwake();

        void StateStart();

        void StateUpdate();

        void StateChange(EGameScenes gameScenes);
    }
}
