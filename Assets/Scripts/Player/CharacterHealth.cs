﻿using UnityEngine;
using System.Collections;
using Assets.Scripts.HealthBarController;

namespace Assets.Scripts.Player
{
    public sealed class CharacterHealth : MonoBehaviour
    {
        [SerializeField]
        private int _playerMaxHealth = 100;
        [SerializeField]
        private int _playerHealth;
        [SerializeField]
        private float _damageBlinkDuration = 2.0f;
        [SerializeField]
        private float _smoothTime = 0.275f;

        [Header("Components")]
        #region Components
        [SerializeField]
        private HealthBar _healthBar;
        [SerializeField]
        private Animator _animator;
        [SerializeField]
        private SpriteRenderer _renderer;
        #endregion

        private Coroutine _damageBlinkCoroutine;
        private Color _defaultSpriteColor;

        // Use this for initialization
        private void Start()
        {
            _playerHealth = _playerMaxHealth;

            if (_healthBar != null)
                _healthBar.SetStartHealth(_playerMaxHealth);

            if (_renderer != null)
                _defaultSpriteColor = _renderer.color;
        }

        // Update is called once per frame
        private void Update()
        {
            // Debug
            if (_healthBar != null && Input.GetKeyDown(KeyCode.Return))
            {
                TakeDamage(10);
            }
        }

        public void TakeDamage(int damage)
        {
            // If player is already dead don't do anything (dead animation should've played already)
            if (_playerHealth == 0)
                return;

            // Set health
            _playerHealth = _playerHealth - damage < 0 ? 0 : _playerHealth - damage;
            _healthBar.SetHealth(_playerHealth);

            // Make damage visible to user by blinking the character in red
            DamageCharacterBlink();

            // Trigger dead animation if player health drops down to 0
            if (_playerHealth == 0)
            {
                if (_animator != null)
                    _animator.SetBool("PlayerDead", true);
            }
        }

        /// <summary>
        ///     Manages character blinking when damage is taken
        /// </summary>
        private void DamageCharacterBlink()
        {
            // If player health is 0, stop coroutine, set default sprite color and return
            if (_playerHealth == 0)
            {
                if (_damageBlinkCoroutine != null)
                    StopCoroutine(_damageBlinkCoroutine);

                _renderer.color = _defaultSpriteColor;

                return;
            }
            // Stop ongoing coroutine
            if (_damageBlinkCoroutine != null)
                StopCoroutine(_damageBlinkCoroutine);
            // Start new coroutine
            _damageBlinkCoroutine = StartCoroutine(DamageBlinkCoroutine());
        }

        /// <summary>
        ///     Coroutine to make character blink when damage is taken
        /// </summary>
        /// <returns></returns>
        private IEnumerator DamageBlinkCoroutine()
        {
            float l_startTime = Time.realtimeSinceStartup;

            while (Time.realtimeSinceStartup - l_startTime <= _damageBlinkDuration)
            {
                Color l_c = _renderer.color;
                l_c.g = 1.0f;
                l_c.b = 1.0f;
                _renderer.color = l_c;

                float l_f = 1.0f;
                float l_v = 0.0f;

                while (l_f > 0.01f)
                {
                    l_f = Mathf.SmoothDamp(l_f, 0.0f, ref l_v, _smoothTime);

                    l_c.g = l_f;
                    l_c.b = l_f;

                    _renderer.color = l_c;

                    yield return null;
                }
            }

            _renderer.color = _defaultSpriteColor;

            _damageBlinkCoroutine = null;
        }
    }
}
