using System;
using Assets.Scripts.CustomClasses.OscillatorFunctions;
using UnityEngine;

namespace Assets.Scripts.CustomClasses
{
    public sealed class Oscillator
    {
        private OscillatorFunction _oscillatorFunction;

        /// <summary>
        ///     Oscillator constructor
        /// </summary>
        /// <param name="oscillatorAngle0">Initial angle</param>
        /// <param name="frequency">Frequency</param>
        /// <param name="oscillatorFunction">Type of oscillator, see <see cref="OscillatorFunction"/></param>
        public Oscillator(float oscillatorAngle0, float frequency, OscillatorFunction oscillatorFunction)
        {
            _oscillatorFunction = oscillatorFunction;
        }

        /// <summary>
        ///     Make the oscillator generate a new oscillation
        /// </summary>
        /// <param name="deltaTime">Delta time</param>
        /// <param name="freqOverride">Frequency override</param>
        /// <returns>Increment of the oscillation</returns>
        public float Oscillate(float deltaTime, float? freqOverride = null)
        {
            float l_oscillation = 0.0f;

            if (freqOverride == null)
            {
                l_oscillation = _oscillatorFunction.GenerateOscillation(deltaTime);
            }
            else
            {
                l_oscillation = _oscillatorFunction.GenerateOscillation(deltaTime, freqOverride);
            }

            return l_oscillation;
        }
    }
}
