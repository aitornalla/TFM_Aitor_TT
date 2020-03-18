using System;

public interface ICharacterStateMachine
{
    void StateUpdate();

    void StateFixedUpdate();

    void Move(float move, bool jump, bool slide, bool glide, bool attack);
}
