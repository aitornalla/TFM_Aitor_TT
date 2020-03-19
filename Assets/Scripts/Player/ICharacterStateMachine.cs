using System;

namespace Assets.Scripts.Player
{
    public interface ICharacterStateMachine
    {
        //void StateUpdate();

        //void StateFixedUpdate();

        void StateControl(float move, bool jump, bool slide, bool glide, bool attack, bool throwkunai);
    }
}
