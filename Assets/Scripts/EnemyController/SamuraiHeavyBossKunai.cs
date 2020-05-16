using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Player;
using UnityEngine;

namespace Assets.Scripts.EnemyController
{
	public class SamuraiHeavyBossKunai : MonoBehaviour
	{
		[SerializeField]
		private LayerMask _playerLayer;                                         // Player layer mask
		[SerializeField]
		private float _speed = 5.0f;                                            // Kunai speed
		[SerializeField]
		private int _damage = 5;                                                // Kunai damage
		[SerializeField]
		private ParticleSystem _trailFarticleSystem;                            // Reference to trail ParticleSystem component
		[SerializeField]
		private Transform _finalTrailPoint;                                     // Referende to the point where the trail disappear
        [SerializeField]
		private AudioSource _audioSource;                                       // Reference to AudioSource component
        [SerializeField]
		private AudioClip[] _swishAudioClips;                                   // Swish sounds

		private bool _waitingToDestroy = false;                                 // Flag for destroying the gameObject

		// Use this for initialization
		private void Start()
		{
			PlaySwishSound();
		}

		// Update is called once per frame
		private void Update()
		{
            
		}

        private void FixedUpdate()
        {
            if (!_waitingToDestroy)
			    transform.Translate(_speed * Time.fixedDeltaTime, 0.0f, 0.0f, Space.Self);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
			// If fireball hits the player
			if (_playerLayer == (_playerLayer | (1 << collision.gameObject.layer)))
			{
				// Get CharacterHealth component
				CharacterHealth l_characterHealth = collision.gameObject.GetComponent<CharacterHealth>();
				// Take damage
				l_characterHealth.TakeDamage(_damage);
			}
			// Destroy kunai
			//Destroy(this.gameObject);

            // Set destroy flag to true
            _waitingToDestroy = true;
            // Disable SpriteRenderer component
            GetComponent<SpriteRenderer>().enabled = false;
            // Disable CapsuleCollider2D component
            GetComponent<CapsuleCollider2D>().enabled = false;
            // Move trail gameObject to its final point
            _trailFarticleSystem.transform.position = _finalTrailPoint.position;
            // Start destroy coroutine
            StartCoroutine(DestroyCoroutine());
		}

        /// <summary>
        ///     Plays swish sound
        /// </summary>
		private void PlaySwishSound()
		{
			float l_value = Random.value * 1000.0f;

			if (l_value < 333.0f)
			{
				_audioSource.PlayOneShot(_swishAudioClips[0]);
			}

			if (l_value >= 333.0f && l_value < 666.0f)
			{
				_audioSource.PlayOneShot(_swishAudioClips[1]);
			}

			if (l_value >= 666.0f)
			{
				_audioSource.PlayOneShot(_swishAudioClips[2]);
			}
		}

        /// <summary>
        ///     Coroutine that waits for the trail particle system to finish before destroying the gameObject
        /// </summary>
        /// <returns></returns>
        private IEnumerator DestroyCoroutine()
        {
            while (_trailFarticleSystem.isPlaying)
            {
				yield return null;
            }
			// Destroy kunai
			Destroy(this.gameObject);
		}
    }
}
