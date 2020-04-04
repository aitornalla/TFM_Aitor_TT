using Assets.Scripts.Scenes;
using System;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.GameManagerController.States
{
    public interface IGameManagerState
    {
        void StateAwake();

        void StateUpdate();

        void StateChange(EGameScenes gameScenes);

        void StateOnSceneLoaded(Scene scene, LoadSceneMode mode);

        void StateOnSceneUnLoaded(Scene scene);
    }
}
