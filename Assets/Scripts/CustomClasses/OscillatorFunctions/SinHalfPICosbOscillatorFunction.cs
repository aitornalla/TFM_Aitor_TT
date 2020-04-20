using System;
using UnityEngine;

namespace Assets.Scripts.CustomClasses.OscillatorFunctions
{
    /// <summary>
    ///     Returns oscillations of function: sin(PI/2 * cos(b * x))
    /// </summary>
    public sealed class SinHalfPICosbOscillatorFunction : OscillatorFunction
    {
        private float _b = 1.0f;                                                // Function parameter

        /// <summary>
        ///     Returns oscillations of function: sin(PI/2 * cos(b * x))
        /// </summary>
        /// <param name="oscillatorAngle0">Initial oscillator angle</param>
        /// <param name="frequency">Frequency of the oscillation</param>
        /// <param name="b">Function parameter</param>
        public SinHalfPICosbOscillatorFunction(float oscillatorAngle0, float frequency, float b) : base(oscillatorAngle0, frequency)
        {
            _b = b;
        }

        public override float GenerateOscillation(float deltaTime, float? freqOverride = null)
        {
            // Check overriding frequency
            float l_frequency = freqOverride == null ? _frequency : freqOverride.Value;

            float l_oscillation = 0.0f;

            l_oscillation = Mathf.Sin(0.5f * Mathf.PI * Mathf.Cos(_b * ((l_frequency * deltaTime + _oscillatorAngle0) * Mathf.Deg2Rad)));

            // Get new angle 0
            _oscillatorAngle0 += l_frequency * deltaTime;

            // If angle 0 is greater than 360, reset it substracting 360 to its value
            _oscillatorAngle0 = _oscillatorAngle0 >= 360.0f ? _oscillatorAngle0 - 360.0f : _oscillatorAngle0;

            return l_oscillation;
        }
    }
}