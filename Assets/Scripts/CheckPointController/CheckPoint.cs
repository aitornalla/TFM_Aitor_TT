using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.GameManagerController;
using UnityEngine;

namespace Assets.Scripts.CheckPointController
{
	public sealed class CheckPoint : MonoBehaviour
	{
        [SerializeField]
        private LayerMask _playerLayer;                             // Player layer to check conditions
        [SerializeField]
        private Transform _respawnPoint;                            // Respawn transform

		private ParticleSystem[] _particleSystems;                  // Array of ParticleSystem components from sakura tree
        private bool _isChecked = false;                            // Flag for checking the checkpoint

        #region Properties
        public Transform RespawnPoint { get { return _respawnPoint; } }
        #endregion

        private void Awake()
        {
            // Get ParticleSystem components in children gameObjects
			_particleSystems = GetComponentsInChildren<ParticleSystem>();
        }

        // Use this for initialization
        private void Start()
		{

		}

		// Update is called once per frame
		private void Update()
		{

		}

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // If player had activated the checkpoint, then return
            if (_isChecked)
                return;

            // If player gets to the checkpoint, play sakura tree particle system
            if (_playerLayer == (_playerLayer | (1 << collision.gameObject.layer)))
            {
                for (int i = 0; i < _particleSystems.Length; i++)
                {
                    _particleSystems[i].Play();
                }

                // Check flag
                _isChecked = true;

                // Assign checkpoint respawn position to current GameManager respawn position
                GameManager.Instance.CurrentCheckPointSpawnPosition = _respawnPoint.position;
            }
        }
    }
}
