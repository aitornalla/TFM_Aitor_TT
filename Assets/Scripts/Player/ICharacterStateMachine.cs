using System;

namespace Assets.Scripts.Player
{
    public interface ICharacterStateMachine
    {
        void StateUpdate();

        void StateFixedUpdate();

        void StateMove(float move, bool jump, bool slide, bool glide, bool attack);
    }
}
