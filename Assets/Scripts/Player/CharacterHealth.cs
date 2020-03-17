using UnityEngine;
using System.Collections;
using Assets.Scripts.HealthBarController;

namespace Assets.Scripts.Player
{
    public class CharacterHealth : MonoBehaviour
    {
        [SerializeField]
        private int _playerMaxHealth = 100;
        [SerializeField]
        private int _playerHealth;
        [SerializeField]
        private HealthBar _healthBar;

        // Use this for initialization
        private void Start()
        {
            _playerHealth = _playerMaxHealth;

            if (_healthBar != null)
                _healthBar.SetStartHealth(_playerMaxHealth);
        }

        // Update is called once per frame
        private void Update()
        {
            // Debug
            if (_healthBar != null && Input.GetKeyDown(KeyCode.Space))
            {
                TakeDamage(10);
            }
        }

        public void TakeDamage(int damage)
        {
            _playerHealth = _playerHealth - damage < 0 ? 0 : _playerHealth - damage;
            _healthBar.SetHealth(_playerHealth);
        }
    }
}
