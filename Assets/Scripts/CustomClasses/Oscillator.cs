using System;
using UnityEngine;

namespace Assets.Scripts.CustomClasses
{
    public enum EOscillatorFunction
    {
        SinFunction,
        CosFunction,
        //CosSinFunction,
        //CosSinPIFunction,
        Cos5Sin1StrikeFunction,
        Cos5Sin2StrikeFunction,
        Cos5Sin3StrikeFunction,
        Cos5Sin4StrikeFunction,
        Cos6Sin1StrikeFunction,
        Cos6Sin2StrikeFunction,
        Cos6Sin3StrikeFunction,
        Cos6Sin4StrikeFunction,
        Cos7Sin1StrikeFunction,
        Cos7Sin2StrikeFunction,
        Cos7Sin3StrikeFunction,
        Cos7Sin4StrikeFunction,
        Cos8Sin1StrikeFunction,
        Cos8Sin2StrikeFunction,
        Cos8Sin3StrikeFunction,
        Cos8Sin4StrikeFunction,
        Cos5SinPI1StrikeFunction,
        Cos5SinPI2StrikeFunction,
        Cos5SinPI3StrikeFunction,
        Cos5SinPI4StrikeFunction,
        Cos6SinPI1StrikeFunction,
        Cos6SinPI2StrikeFunction,
        Cos6SinPI3StrikeFunction,
        Cos6SinPI4StrikeFunction,
        Cos7SinPI1StrikeFunction,
        Cos7SinPI2StrikeFunction,
        Cos7SinPI3StrikeFunction,
        Cos7SinPI4StrikeFunction,
        Cos8SinPI1StrikeFunction,
        Cos8SinPI2StrikeFunction,
        Cos8SinPI3StrikeFunction,
        Cos8SinPI4StrikeFunction,
        Cos1_25ClampFunction,
        Cos1_50ClampFunction,
        Cos1_75ClampFunction,
        Cos2_00ClampFunction
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
                //case EOscillatorFunction.CosSinFunction:
                //case EOscillatorFunction.CosSinPIFunction:
                //    {
                //        float l_pi = Enum.GetName(typeof(EOscillatorFunction), _oscillatorFunction).IndexOf("PI") >= 0 ? Mathf.PI : 0.0f;

                //        l_oscillation = Mathf.Cos(2.0f * Mathf.PI * Mathf.Sin((l_frequency * deltaTime + _oscillatorAngle0) * Mathf.Deg2Rad) + l_pi);
                //    }
                //    break;
                case EOscillatorFunction.Cos5Sin1StrikeFunction:
                case EOscillatorFunction.Cos5Sin2StrikeFunction:
                case EOscillatorFunction.Cos5Sin3StrikeFunction:
                case EOscillatorFunction.Cos5Sin4StrikeFunction:
                case EOscillatorFunction.Cos6Sin1StrikeFunction:
                case EOscillatorFunction.Cos6Sin2StrikeFunction:
                case EOscillatorFunction.Cos6Sin3StrikeFunction:
                case EOscillatorFunction.Cos6Sin4StrikeFunction:
                case EOscillatorFunction.Cos7Sin1StrikeFunction:
                case EOscillatorFunction.Cos7Sin2StrikeFunction:
                case EOscillatorFunction.Cos7Sin3StrikeFunction:
                case EOscillatorFunction.Cos7Sin4StrikeFunction:
                case EOscillatorFunction.Cos8Sin1StrikeFunction:
                case EOscillatorFunction.Cos8Sin2StrikeFunction:
                case EOscillatorFunction.Cos8Sin3StrikeFunction:
                case EOscillatorFunction.Cos8Sin4StrikeFunction:
                case EOscillatorFunction.Cos5SinPI1StrikeFunction:
                case EOscillatorFunction.Cos5SinPI2StrikeFunction:
                case EOscillatorFunction.Cos5SinPI3StrikeFunction:
                case EOscillatorFunction.Cos5SinPI4StrikeFunction:
                case EOscillatorFunction.Cos6SinPI1StrikeFunction:
                case EOscillatorFunction.Cos6SinPI2StrikeFunction:
                case EOscillatorFunction.Cos6SinPI3StrikeFunction:
                case EOscillatorFunction.Cos6SinPI4StrikeFunction:
                case EOscillatorFunction.Cos7SinPI1StrikeFunction:
                case EOscillatorFunction.Cos7SinPI2StrikeFunction:
                case EOscillatorFunction.Cos7SinPI3StrikeFunction:
                case EOscillatorFunction.Cos7SinPI4StrikeFunction:
                case EOscillatorFunction.Cos8SinPI1StrikeFunction:
                case EOscillatorFunction.Cos8SinPI2StrikeFunction:
                case EOscillatorFunction.Cos8SinPI3StrikeFunction:
                case EOscillatorFunction.Cos8SinPI4StrikeFunction:
                    {
                        // Get enum value name
                        string l_enumName = Enum.GetName(typeof(EOscillatorFunction), _oscillatorFunction);

                        // Calculate sinus times
                        int l_times = 0;

                        Int32.TryParse(l_enumName.Substring(l_enumName.IndexOf("Sin") - 1, 1), out l_times);

                        float l_sin = Mathf.Sin((l_frequency * deltaTime + _oscillatorAngle0) * Mathf.Deg2Rad);

                        for (int i = 1; i < l_times; i++)
                        {
                            l_sin *= l_sin;
                        }

                        // If function has PI ...
                        float l_pi = l_enumName.IndexOf("PI") >= 0 ? Mathf.PI : 0.0f;

                        // Calculate number of strikes
                        float l_strikes = 0.0f;

                        if (l_enumName.IndexOf("PI") >= 0)
                        {
                            float.TryParse(
                                l_enumName.Substring(l_enumName.IndexOf("PI") + 2, l_enumName.IndexOf("Strike") - (l_enumName.IndexOf("PI") + 2)),
                                out l_strikes);
                        }
                        else
                        {
                            float.TryParse(
                                l_enumName.Substring(l_enumName.IndexOf("Sin") + 3, l_enumName.IndexOf("Strike") - (l_enumName.IndexOf("Sin") + 3)),
                                out l_strikes);
                        }

                        // Calculate oscillation
                        l_oscillation = Mathf.Cos(l_strikes * Mathf.PI * l_sin + l_pi);
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
