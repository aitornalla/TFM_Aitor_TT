using System;
using UnityEngine;

namespace Assets.Scripts.CustomClasses
{
    public enum EOscillatorFunction
    {
        SinFunction,
        CosFunction
    }

    /// <summary>
    ///  Oscillator class to simulate a sin o cos function and get values from -1.0 to 1.0
    /// </summary>
    public sealed class Oscillator
    {
        private float _oscillatorAngle0;
        private float _frequency;
        private EOscillatorFunction _oscillatorFunction;

        /// <summary>
        ///     Oscillator constructor
        /// </summary>
        /// <param name="oscillatorAngle0">Initial angle</param>
        /// <param name="frequency">Frequency</param>
        /// <param name="oscillatorFunction">Type of oscillator, see <see cref="EOscillatorFunction"/></param>
        public Oscillator(float oscillatorAngle0, float frequency, EOscillatorFunction oscillatorFunction)
        {
            _oscillatorAngle0 = oscillatorAngle0;
            _frequency = frequency;
            _oscillatorFunction = oscillatorFunction;
        }

        /// <summary>
        ///     Make the oscillator generate a new oscillation
        /// </summary>
        /// <param name="deltaTime">Delta time</param>
        /// <returns>Increment of the oscillation</returns>
        public float Oscillate(float deltaTime)
        {
            float l_oscillation = Mathf.Cos((_frequency * deltaTime + _oscillatorAngle0) * Mathf.Deg2Rad);

            _oscillatorAngle0 += _frequency * deltaTime;

            return l_oscillation;
        }
    }
}
