using System;

namespace Assets.Scripts.Player
{
    /// <summary>
    ///     Interface for creating character states
    /// </summary>
    public interface ICharacterStateMachine
    {
        //void StateUpdate();

        //void StateFixedUpdate();

        void StateControl(ControlFlags controlFlags);
    }
}
