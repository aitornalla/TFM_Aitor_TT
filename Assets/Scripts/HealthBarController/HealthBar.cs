using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.HealthBarController
{
    [RequireComponent(typeof(Slider))]
    public class HealthBar : MonoBehaviour
    {
        [SerializeField]
        private Slider _slider;
        [SerializeField]
        private Gradient _gradient;
        [SerializeField]
        private Image _fill;

        private float _health;
        private float _targetHealth;
        private float _velocity = 0.0f;
        [SerializeField]
        private float _smoothTime = 0.25f;

        private void Update()
        {
            // Smooth slider movement
            _health = Mathf.SmoothDamp(_health, _targetHealth, ref _velocity, _smoothTime);
            // Set health value
            _slider.value = _health;
            // Set fill color to slider value
            _fill.color = _gradient.Evaluate(_slider.normalizedValue);
        }

        /// <summary>
        ///     Set start health
        /// </summary>
        /// <param name="maxHealth">Max health</param>
        /// <param name="startHealth">Starting health. If set to a value, <paramref name="startNoMaxHealth"/> must be <code>true</code></param>
        /// <param name="startNoMaxHealth"><code>true</code> if starting health is no equal to <paramref name="maxHealth"/>, <code>false</code> by default</param>
        public void SetStartHealth(int maxHealth, int startHealth = 0, bool startNoMaxHealth = false)
        {
            // Set start health
            _health = startNoMaxHealth ? startHealth : maxHealth;
            _targetHealth = startNoMaxHealth ? startHealth : maxHealth;
            // Set slider maxValue
            _slider.maxValue = maxHealth;
            // Set start slider value
            _slider.value = startNoMaxHealth ? startHealth : maxHealth;
            // Set start fill color
            _fill.color = _gradient.Evaluate(_slider.normalizedValue);
        }

        /// <summary>
        ///     Set health value to healthbar
        /// </summary>
        /// <param name="health">Current health</param>
        public void SetHealth(int health)
        {
            _targetHealth = health;
        }
    }
}
