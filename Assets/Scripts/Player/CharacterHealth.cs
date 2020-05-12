using UnityEngine;
using System.Collections;
using Assets.Scripts.HealthBarController;
using Assets.Scripts.Camera;
using Assets.Scripts.GameManagerController;

namespace Assets.Scripts.Player
{
    public sealed class CharacterHealth : MonoBehaviour
    {
        [SerializeField]
        private int _playerMaxLifes = 5;
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
        private CharacterComponents _characterComponents;
        [SerializeField]
        private HealthBar _healthBar;
        [SerializeField]
        private CameraShake _cameraShake;
        #endregion

        private Coroutine _damageBlinkCoroutine;
        private Color _defaultSpriteColor;

        #region Properties
        public int PlayerMaxLifes
        {
            get
            {
                return _playerMaxLifes;
            }
        }

        public int PlayerMaxHealth
        {
            get
            {
                return _playerMaxHealth;
            }
        }
        #endregion

        // Use this for initialization
        private void Start()
        {
            _playerHealth = _playerMaxHealth;

            if (_healthBar != null)
                _healthBar.SetStartHealth(_playerMaxHealth);

            if (_characterComponents.SpriteRenderer != null)
                _defaultSpriteColor = _characterComponents.SpriteRenderer.color;
        }

        // Update is called once per frame
        //private void Update()
        //{
            // Debug
            //if (_healthBar != null && Input.GetKeyDown(KeyCode.Return))
            //{
            //    TakeDamage(10);
            //}
        //}

        public void TakeDamage(int damage)
        {
            // If player is already dead don't do anything (dead animation should've played already)
            if (_playerHealth == 0)
                return;

            // Damage cannot be negative
            if (damage < 0)
                return;

            // Set health
            _playerHealth = _playerHealth - damage < 0 ? 0 : _playerHealth - damage;
            _healthBar.SetHealth(_playerHealth);

            // Make damage visible to user by blinking the character in red
            DamageCharacterBlink();

            // Trigger camera shake effect
            _cameraShake.CameraShakeEffect();

            // Change character flag and trigger dead animation if player health drops down to 0
            if (_playerHealth == 0)
            {
                // Set dead flag to true
                GetComponent<CharacterFlags>().IsDead = true;
                // Trigger animation
                if (_characterComponents.Animator != null)
                    _characterComponents.Animator.SetBool("PlayerDead", true);
                // Play dead sound
                _characterComponents.CharacterAudio.Dead();
            }
            else
            {
                // Play damage sound
                _characterComponents.CharacterAudio.Damage();
            }
        }

        public void RestoreHealth(int health)
        {
            // Health cannot be negative
            if (health < 0)
                return;

            // Set health
            _playerHealth = _playerHealth + health > _playerMaxHealth ? _playerMaxHealth : _playerHealth + health;
            _healthBar.SetHealth(_playerHealth);
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

                _characterComponents.SpriteRenderer.color = _defaultSpriteColor;

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
        /// <returns>Damage blink coroutine</returns>
        private IEnumerator DamageBlinkCoroutine()
        {
            float l_startTime = Time.realtimeSinceStartup;

            while (Time.realtimeSinceStartup - l_startTime <= _damageBlinkDuration)
            {
                Color l_c = _characterComponents.SpriteRenderer.color;
                l_c.g = 1.0f;
                l_c.b = 1.0f;
                _characterComponents.SpriteRenderer.color = l_c;

                float l_f = 1.0f;
                float l_v = 0.0f;

                while (l_f > 0.01f)
                {
                    l_f = Mathf.SmoothDamp(l_f, 0.0f, ref l_v, _smoothTime);

                    l_c.g = l_f;
                    l_c.b = l_f;

                    _characterComponents.SpriteRenderer.color = l_c;

                    yield return null;
                }
            }

            _characterComponents.SpriteRenderer.color = _defaultSpriteColor;

            _damageBlinkCoroutine = null;
        }

        /// <summary>
        ///     Called from player death animation event
        /// </summary>
        public void OnPlayerDeathEndAnimationEvent()
        {
            // Calls GameManager to manage player death/respawn after death animation ends
            //GameManager.Instance.ManagePlayerDeathAndRespawn();

            // Starts transition from death to respawn the player
            // The transition gameObject will call the GameManager to handle the player death/respawn once its animation ends
            GameManager.Instance.DeathRespawnBlackPanel.SetActive(true);
        }
    }
}
