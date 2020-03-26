using System;
using UnityEngine;

namespace Assets.Scripts.CustomClasses
{
    public enum EOscillatorFunction
    {
        CosFunction,
        CosSinFunction,
        CosSinSinSinSinFunction,
        Cos2SinFunction,
        Cos3SinFunction,
        Cos4SinFunction,
        Cos5SinFunction,
        Cos1_25ClampFunction,
        Cos1_50ClampFunction,
        Cos1_75ClampFunction,
        Cos2_00ClampFunction,
        SinFunction
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
        /// <param name="freqOverride">Frequency override</param>
        /// <returns>Increment of the oscillation</returns>
        public float Oscillate(float deltaTime, float? freqOverride = null)
        {
            // Check overriding frequency
            float l_frequency = freqOverride == null ? _frequency : freqOverride.Value;

            float l_oscillation = 0.0f;

            // Switch oscillator function
            switch(_oscillatorFunction)
            {
                case EOscillatorFunction.CosFunction:
                    l_oscillation = Mathf.Cos((l_frequency * deltaTime + _oscillatorAngle0) * Mathf.Deg2Rad);
                    break;
                case EOscillatorFunction.SinFunction:
                    l_oscillation = Mathf.Sin((l_frequency * deltaTime + _oscillatorAngle0) * Mathf.Deg2Rad);
                    break;
                case EOscillatorFunction.CosSinFunction:
                    l_oscillation = Mathf.Cos(2.0f * Mathf.PI * Mathf.Sin((l_frequency * deltaTime + _oscillatorAngle0) * Mathf.Deg2Rad));
                    break;
                case EOscillatorFunction.CosSinSinSinSinFunction:
                    {
                        float l_sin = Mathf.Sin((l_frequency * deltaTime + _oscillatorAngle0) * Mathf.Deg2Rad);

                        l_sin = Mathf.Sin(2.0f * Mathf.PI * l_sin);

                        l_oscillation = Mathf.Cos(2.0f * Mathf.PI * l_sin * l_sin);
                    }
                    break;
                case EOscillatorFunction.Cos2SinFunction:
                case EOscillatorFunction.Cos3SinFunction:
                case EOscillatorFunction.Cos4SinFunction:
                case EOscillatorFunction.Cos5SinFunction:
                    {
                        // Group same functions

                        string l_enumName = Enum.GetName(typeof(EOscillatorFunction), _oscillatorFunction);

                        int l_times = 0;

                        Int32.TryParse(l_enumName.Substring(l_enumName.IndexOf("Sin") - 1, 1), out l_times);

                        float l_sin = Mathf.Sin((l_frequency * deltaTime + _oscillatorAngle0) * Mathf.Deg2Rad);

                        for (int i = 1; i < l_times; i++)
                        {
                            l_sin *= l_sin;
                        }

                        l_oscillation = Mathf.Cos(2.0f * Mathf.PI * l_sin);
                    }
                    break;
                case EOscillatorFunction.Cos1_25ClampFunction:
                case EOscillatorFunction.Cos1_50ClampFunction:
                case EOscillatorFunction.Cos1_75ClampFunction:
                case EOscillatorFunction.Cos2_00ClampFunction:
                    {
                        string l_enumName = Enum.GetName(typeof(EOscillatorFunction), _oscillatorFunction);

                        int l_intPart = 0;

                        Int32.TryParse(l_enumName.Substring(l_enumName.IndexOf("Cos") + 3, l_enumName.IndexOf("_") - (l_enumName.IndexOf("Cos") + 3)), out l_intPart);

                        int l_decimalPart = 0;

                        Int32.TryParse(l_enumName.Substring(l_enumName.IndexOf("_") + 1, l_enumName.IndexOf("Clamp") - (l_enumName.IndexOf("_") + 1)), out l_decimalPart);

                        float l_times = (float)(l_intPart) + ((float)(l_decimalPart) / 100.0f);

                        l_oscillation = Mathf.Clamp(l_times * Mathf.Cos((l_frequency * deltaTime + _oscillatorAngle0) * Mathf.Deg2Rad), -1.0f, 1.0f);
                    }
                    break;
                default:
                    break;
            }

            // Get new angle 0
            _oscillatorAngle0 += l_frequency * deltaTime;

            // If angle 0 is greater than 360, reset it substracting 360 to its value
            _oscillatorAngle0 = _oscillatorAngle0 >= 360.0f ? _oscillatorAngle0 - 360.0f : _oscillatorAngle0;

            return l_oscillation;
        }
    }
}
