using Cinemachine;
using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Camera
{
    public sealed class CameraShake : MonoBehaviour
    {
        [SerializeField]
        private CinemachineVirtualCamera _cinemachineVirtualCamera;
        [SerializeField]
        private float _defaultShakeAmplitude = 0.15f;                           // Default shake amplitude
        [SerializeField]
        private float _defaultShakeFrequency = 3.0f;                            // Default shake frequency
        [SerializeField]
        private float _defaultShakeDuration = 0.5f;                             // Default shake time duration

        private CinemachineBasicMultiChannelPerlin _cinemachineBasicMultiChannelPerlin;

        private Coroutine _shakeCoroutine = null;

        #region Constants
        private const float IdleAmplitude = 0.0f;
        private const float IdleFrequency = 0.0f;
        #endregion

        // Use this for initialization
        private void Start()
        {
            if (_cinemachineVirtualCamera != null)
                _cinemachineBasicMultiChannelPerlin = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        /// <summary>
        ///     Starts camera shake effect with default values
        /// </summary>
        public void CameraShakeEffect()
        {
            // Stop ongoing coroutine
            if (_shakeCoroutine != null)
                StopCoroutine(_shakeCoroutine);
            // Start new coroutine
            _shakeCoroutine = StartCoroutine(CameraShakeEffectCoroutine(_defaultShakeAmplitude, _defaultShakeFrequency, _defaultShakeDuration));
        }

        /// <summary>
        ///     Starts camera shake effect with default values but custom duration
        /// </summary>
        /// <param name="duration">The duration of the shake</param>
        public void CameraShakeEffect(float duration)
        {
            // Stop ongoing coroutine
            if (_shakeCoroutine != null)
                StopCoroutine(_shakeCoroutine);
            // Start new coroutine
            _shakeCoroutine = StartCoroutine(CameraShakeEffectCoroutine(_defaultShakeAmplitude, _defaultShakeFrequency, duration));
        }

        /// <summary>
        ///     Starts camera shake effect with custom settings
        /// </summary>
        /// <param name="amplitude">The amplitude of the shake</param>
        /// <param name="frequency">The frequency of the shake</param>
        /// <param name="duration">The duration of the shake</param>
        public void CameraShakeEffect(float amplitude, float frequency, float duration)
        {
            // Stop ongoing coroutine
            if (_shakeCoroutine != null)
                StopCoroutine(_shakeCoroutine);
            // Start new coroutine
            _shakeCoroutine = StartCoroutine(CameraShakeEffectCoroutine(amplitude, frequency, duration));
        }

        /// <summary>
        ///     Coroutine for camera shake effect
        /// </summary>
        /// <param name="amplitude">The amplitude of the shake</param>
        /// <param name="frequency">The frequency of the shake</param>
        /// <param name="duration">The duration of the shake</param>
        /// <returns>Camera shake effect coroutine</returns>
        private IEnumerator CameraShakeEffectCoroutine(float amplitude, float frequency, float duration)
        {
            // Set perlin parameters
            _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = amplitude;
            _cinemachineBasicMultiChannelPerlin.m_FrequencyGain = frequency;
            // Wait coroutine for shake duration time
            yield return new WaitForSeconds(duration);
            // Set perlin parameters to idle
            _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = IdleAmplitude;
            _cinemachineBasicMultiChannelPerlin.m_FrequencyGain = IdleAmplitude;
            // Set coroutine variable to null
            _shakeCoroutine = null;
        }
    }
}
