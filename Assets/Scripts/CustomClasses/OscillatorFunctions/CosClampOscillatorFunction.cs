﻿using System;
using UnityEngine;

namespace Assets.Scripts.CustomClasses.OscillatorFunctions
{
    /// <summary>
    ///     Returns oscillations of function: Clamp(mult * cos(x), -1.0f, 1.0f)
    /// </summary>
    public sealed class CosClampOscillatorFunction : OscillatorFunction
    {
        private float _mult = 1.0f;         // Function parameter

        /// <summary>
        ///     Returns oscillations of function: Clamp(mult * cos(x), -1.0f, 1.0f)
        /// </summary>
        /// <param name="oscillatorAngle0">Initial oscillator angle</param>
        /// <param name="frequency">Frequency of the oscillation</param>
        /// <param name="mult">Function parameter</param>
        public CosClampOscillatorFunction(float oscillatorAngle0, float frequency, float mult) : base(oscillatorAngle0, frequency)
        {
            _mult = mult;
        }

        public override float GenerateOscillation(float deltaTime, float? freqOverride = null)
        {
            // Check overriding frequency
            float l_frequency = freqOverride == null ? _frequency : freqOverride.Value;

            float l_oscillation = 0.0f;

            l_oscillation = Mathf.Clamp(_mult * Mathf.Cos((l_frequency * deltaTime + _oscillatorAngle0) * Mathf.Deg2Rad), -1.0f, 1.0f);

            // Get new angle 0
            _oscillatorAngle0 += l_frequency * deltaTime;

            // If angle 0 is greater than 360, reset it substracting 360 to its value
            _oscillatorAngle0 = _oscillatorAngle0 >= 360.0f ? _oscillatorAngle0 - 360.0f : _oscillatorAngle0;

            return l_oscillation;
        }
    }
}
