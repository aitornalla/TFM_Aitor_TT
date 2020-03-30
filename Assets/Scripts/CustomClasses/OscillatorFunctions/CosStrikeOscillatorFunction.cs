using System;
using UnityEngine;

namespace Assets.Scripts.CustomClasses.OscillatorFunctions
{
    /// <summary>
    ///     Returns oscillations of function: cos(strikes * PI * sin^int(x) + displ)
    /// </summary>
    public sealed class CosStrikeOscillatorFunction : OscillatorFunction
    {
        private int _sines = 1;
        private float _strikes = 1.0f;
        private float _displacement = 0.0f;

        /// <summary>
        ///     Returns oscillations of function: cos(strikes * PI * sin^int(x) + displ)
        /// </summary>
        /// <param name="oscillatorAngle0">Initial oscillator angle</param>
        /// <param name="frequency">Frequency of the oscillation</param>
        /// <param name="sines">Number of sines, intensity</param>
        /// <param name="strikes">Strikes of the oscillation</param>
        /// <param name="displacement">Oscillation displacement to change when the strikes happen</param>
        public CosStrikeOscillatorFunction(float oscillatorAngle0, float frequency, int sines, float strikes, float displacement)
            : base(oscillatorAngle0, frequency)
        {
            _sines = sines;
            _strikes = strikes;
            _displacement = displacement;
        }

        public override float GenerateOscillation(float deltaTime, float? freqOverride = null)
        {
            // Check overriding frequency
            float l_frequency = freqOverride == null ? _frequency : freqOverride.Value;

            float l_oscillation = 0.0f;

            float l_sin = Mathf.Sin((l_frequency * deltaTime + _oscillatorAngle0) * Mathf.Deg2Rad);

            for (int i = 1; i < _sines; i++)
            {
                l_sin *= l_sin;
            }

            // Calculate oscillation
            l_oscillation = Mathf.Cos(_strikes * Mathf.PI * l_sin + _displacement);

            // Get new angle 0
            _oscillatorAngle0 += l_frequency * deltaTime;

            // If angle 0 is greater than 360, reset it substracting 360 to its value
            _oscillatorAngle0 = _oscillatorAngle0 >= 360.0f ? _oscillatorAngle0 - 360.0f : _oscillatorAngle0;

            return l_oscillation;
        }
    }
}
