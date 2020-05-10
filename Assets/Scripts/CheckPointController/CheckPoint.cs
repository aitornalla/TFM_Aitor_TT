using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.GameManagerController;
using UnityEngine;

namespace Assets.Scripts.CheckPointController
{
	public sealed class CheckPoint : MonoBehaviour
	{
        [SerializeField]
        private LayerMask _playerLayer;                                         // Player layer to check conditions
        [SerializeField]
        private Transform _respawnPoint;                                        // Respawn transform
        [SerializeField]
        private GameObject _checkpointBanner;                                   // Checkpoint banner gameObject

		private ParticleSystem[] _particleSystems;                              // Array of ParticleSystem components from sakura tree
        private AudioSource _audioSource;                                       // Reference to AudioSource component
        private bool _isChecked = false;                                        // Flag for checking the checkpoint

        #region Properties
        public Transform RespawnPoint { get { return _respawnPoint; } }
        #endregion

        private void Awake()
        {
            // Get ParticleSystem components in children gameObjects
			_particleSystems = GetComponentsInChildren<ParticleSystem>();
            // Get AudioSource component
            _audioSource = GetComponent<AudioSource>();
            // Set AudioMixerGroup
            _audioSource.outputAudioMixerGroup = GameManager.Instance.AudioMixerController.MainAudioMixerGroups[0];
        }

        // Use this for initialization
        //private void Start()
		//{
        //
		//}

		// Update is called once per frame
		//private void Update()
		//{
        //
		//}

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // If player had activated the checkpoint, then return
            if (_isChecked)
                return;

            // If player gets to the checkpoint, play sakura tree particle system
            if (_playerLayer == (_playerLayer | (1 << collision.gameObject.layer)))
            {
                // Check flag
                _isChecked = true;

                // Play all particle systems
                for (int i = 0; i < _particleSystems.Length; i++)
                {
                    _particleSystems[i].Play();
                }

                // Enable checkpoint banner
                _checkpointBanner.SetActive(true);

                // Assign checkpoint respawn position to current GameManager respawn position
                GameManager.Instance.CurrentCheckPointSpawnPosition = _respawnPoint.position;

                // Play checkpoint sound
                _audioSource.Play();
            }
        }

        private void OnBecameVisible()
        {
            // Play particle system when visible to the camera
            if (_isChecked)
            {
                for (int i = 0; i < _particleSystems.Length; i++)
                {
                    _particleSystems[i].Play();
                }
            }
        }

        private void OnBecameInvisible()
        {
            // Pause particle system when not visible to the camera
            if (_isChecked)
            {
                for (int i = 0; i < _particleSystems.Length; i++)
                {
                    _particleSystems[i].Pause();
                }
            }
        }
    }
}
