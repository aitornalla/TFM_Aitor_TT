using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;

namespace Assets.Scripts.CustomClasses
{
    public enum EAudioMixerVolume
    {
        Up,
        Down
    }

    public class AudioMixerController : MonoBehaviour
    {
        private AudioMixer _mainAudioMixer;
        private AudioMixerGroup[] _mainAudioMixerGroups;
        private float _minimumAudioMixerdB; // = -80.0f;
        private float _maximumAudioMixerdB; // = 20.0f;
        private float _volumeIncrement; // = 5.0f;
        private readonly string _audioMixerAssetPath = string.Join(Path.DirectorySeparatorChar.ToString(), new string[] { "AudioMixers", "MainAudioMixer" });
        private readonly string _amExposedParamVolume = "Volume";
        //private List<string> _amExposedParameters;

        #region Properties
        public AudioMixer MainAudioMixer { get { return _mainAudioMixer; } }
        public AudioMixerGroup[] MainAudioMixerGroups { get { return _mainAudioMixerGroups; } }
        public float MinimumAudioMixerd { get { return _minimumAudioMixerdB; } }
        public float MaximumAudioMixerd { get { return _maximumAudioMixerdB; } }
        #endregion

        #region Constructor
        //public AudioMixerController(AudioMixer audioMixer)
        //{
        //    // AudioMixer gameObject
        //    //_mainAudioMixer = Resources.Load<AudioMixer>(_audioMixerAssetPath);
        //    _mainAudioMixer = audioMixer;
        //    // Get AudioMixerGroups
        //    _mainAudioMixerGroups = _mainAudioMixer.FindMatchingGroups("Master");
        //    // Main AudioMixer minimum/maximum volume values
        //    _minimumAudioMixerdB = -80.0f;
        //    _maximumAudioMixerdB = 20.0f;
        //    // AudioMixer volume increment
        //    _volumeIncrement = 5.0f;
        //    // Get all AudioMixer exposed parameters
        //    _amExposedParameters = GetAudioMixerExposedParameters();
        //}
        #endregion

        #region Initializer
        public void InitAudioMixerController()
        {
            // AudioMixer gameObject
            _mainAudioMixer = Resources.Load<AudioMixer>(_audioMixerAssetPath);
            // Get AudioMixerGroups
            _mainAudioMixerGroups = _mainAudioMixer.FindMatchingGroups("Master");
            // Main AudioMixer minimum/maximum volume values
            _minimumAudioMixerdB = -80.0f;
            _maximumAudioMixerdB = 20.0f;
            // AudioMixer volume increment
            _volumeIncrement = 5.0f;
            // Get all AudioMixer exposed parameters
            //_amExposedParameters = GetAudioMixerExposedParameters();
        }
        #endregion

        #region Private Methods
        //private List<string> GetAudioMixerExposedParameters()
        //{
        //    List<string> l_paramsList = new List<string>();

        //    Array l_parameters = (Array)_mainAudioMixer.GetType().GetProperty("exposedParameters").GetValue(_mainAudioMixer, null);

        //    for (int i = 0; i < l_parameters.Length; i++)
        //    {
        //        var var_o = l_parameters.GetValue(i);
        //        string l_param = (string)var_o.GetType().GetField("name").GetValue(var_o);
        //        l_paramsList.Add(l_param);
        //    }

        //    return l_paramsList;
        //}
        #endregion

        #region Public Methods
        /// <summary>
        ///     Current AudioMixer volume in dB
        /// </summary>
        /// <returns>Volume in dB</returns>
        public float GetCurrentVolume()
        {
            float l_currentVol = 0.0f;

            _mainAudioMixer.GetFloat(_amExposedParamVolume, out l_currentVol);

            return l_currentVol;
        }

        /// <summary>
        ///     Current AudioMixer volume normalized in range 0 to 1
        /// </summary>
        /// <returns>Volume in range 0 to 1</returns>
        public float GetCurrentVolume01()
        {
            float l_currentVolNorm = 0.0f;
            float l_currentVol = 0.0f;

            // Get current AudioMixer volume
            _mainAudioMixer.GetFloat(_amExposedParamVolume, out l_currentVol);

            // Normalize in range 0 to 1
            l_currentVolNorm = (l_currentVol - _minimumAudioMixerdB) / (_maximumAudioMixerdB - _minimumAudioMixerdB);

            return l_currentVolNorm;
        }

        /// <summary>
        ///     Set AudioMixer volume up/down
        /// </summary>
        /// <param name="option">Volume up or down, see <see cref="EAudioMixerVolume"/></param>
        public void SetVolume(EAudioMixerVolume option)
        {
            float l_volIncrement = 0.0f;
            float l_currentVolume = 0.0f;

            // Get AudioMixer current volume
            _mainAudioMixer.GetFloat(_amExposedParamVolume, out l_currentVolume);

            if (l_currentVolume == _minimumAudioMixerdB && option == EAudioMixerVolume.Down)
            {
                return;
            }
            else if (l_currentVolume == _maximumAudioMixerdB && option == EAudioMixerVolume.Up)
            {
                return;
            }

            // Assign volume increment
            switch (option)
            {
                case EAudioMixerVolume.Up:
                    l_volIncrement = _volumeIncrement;
                    break;
                case EAudioMixerVolume.Down:
                    l_volIncrement = _volumeIncrement * (-1.0f);
                    break;
                default:
                    break;
            }

            // Assign new AudioMixerVolume
            _mainAudioMixer.SetFloat(_amExposedParamVolume, l_currentVolume + l_volIncrement);
        }
        #endregion
    }
}
