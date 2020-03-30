using System;

namespace Assets.Scripts.CustomClasses.OscillatorFunctions
{
    public abstract class OscillatorFunction
    {
        protected float _oscillatorAngle0;
        protected float _frequency;

        public OscillatorFunction(float oscillatorAngle0, float frequency)
        {
            _oscillatorAngle0 = oscillatorAngle0;
            _frequency = frequency;
        }

        public abstract float GenerateOscillation(float deltaTime, float? freqOverride = null);
    }
}
