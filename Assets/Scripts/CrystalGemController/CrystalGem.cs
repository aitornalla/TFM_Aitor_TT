using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.CustomClasses;
using Assets.Scripts.CustomClasses.OscillatorFunctions;
using Assets.Scripts.GameManagerController;
using Assets.Scripts.Player;
using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.CrystalGemController
{
    public enum ECrystalGemType
    {
		RedHeart,
        Silver,
		Turquoise,
        Yellow
    }

	public sealed class CrystalGem : MonoBehaviour
	{
		[SerializeField]
		private ECrystalGemType _crystalGemType;                                // Type of gem
		[SerializeField]
		private int _score;                                                     // Score of the crystal gem
        [SerializeField]
		private LayerMask _playerLayer;                                         // Player layer to check conditions
		[SerializeField]
		private Transform _upLimit;                                             // Movement up limit
		[SerializeField]
		private Transform _downLimit;                                           // Movement down limit
		[SerializeField]
		private Transform _initialPosition;                                     // Initial gem position (one of the two limits)
		[SerializeField]
		private float _frequency = 10.0f;                                       // Moving frequency
		[SerializeField]
		private float _bMovementParam = 1.0f;                                   // Parameter for oscillating movement
		[SerializeField]
		private bool _reverseInitialDirection = false;                          // Flag for intial gem direction

		private ParticleSystem _particleSystem = null;                          // ParticleSystem component
		private SpriteRenderer _spriteRenderer = null;                          // SpriteRenderer component
		private CapsuleCollider2D _capsuleCollider2D = null;                    // CapsuleCollider2D component

        private Oscillator _oscillator;                                         // Oscillator object
		private float _gemPosition_0;                                           // Gem previous position
		private float _movementSemiLength;
		private bool _isDestroying = false;                                     // Flag for when the gem is destroying
        private bool _isVisible = false;                                        // If gameObject no visible, don't update (save resources)

        private void Awake()
        {
			// Get ParticleSystem component reference
			_particleSystem = GetComponent<ParticleSystem>();
			// Get SpriteRenderer component reference
			_spriteRenderer = GetComponent<SpriteRenderer>();
			// Get CapsuleCollider2D component reference
			_capsuleCollider2D = GetComponent<CapsuleCollider2D>();
		}

        // Use this for initialization
        private void Start()
		{
			// Calculate total movement length
			_movementSemiLength = Mathf.Abs(_upLimit.localPosition.y - _downLimit.localPosition.y) / 2.0f;
			// Calculate initial oscillator angle
			float l_oscillatorAngle_0 = Mathf.Acos(_initialPosition.localPosition.y / _movementSemiLength) * Mathf.Rad2Deg;
			// If initial platform movement is to the right, recalculate initial oscillator angle
			if (_reverseInitialDirection)
			{
				l_oscillatorAngle_0 = 360.0f - l_oscillatorAngle_0;
			}
			// Instantiate new Oscillator object
			_oscillator = new Oscillator(l_oscillatorAngle_0, _frequency, new CosSqrtbOscillatorFunction(l_oscillatorAngle_0, _frequency, _bMovementParam));
			// Translate saw to the initial position
			transform.Translate(_initialPosition.localPosition.x, 0.0f, 0.0f, Space.Self);
			// Assing initial saw position 0
			_gemPosition_0 = _initialPosition.localPosition.x;
		}

		private void Update()
		{
			if (!_isVisible || _isDestroying)
				return;

			// Calculate oscillator position
			float l_pos = _oscillator.Oscillate(Time.deltaTime);
			// Calculate increment to translate
			float l_increment = l_pos * _movementSemiLength - _gemPosition_0;
			// Translate
			transform.Translate(0.0f, l_increment, 0.0f, Space.Self);
			// Assign increment to the platform position 0 for next frame
			_gemPosition_0 += l_increment;
		}

        private void OnTriggerEnter2D(Collider2D collision)
        {
			// If player touches the gem ...
			if (_playerLayer == (_playerLayer | (1 << collision.gameObject.layer)))
			{
                // Apply gem effect
                switch (_crystalGemType)
                {
					case ECrystalGemType.RedHeart:
                        {
							CharacterHealth l_characterHealth = collision.GetComponent<CharacterHealth>();
							l_characterHealth.RestoreHealth(l_characterHealth.PlayerMaxHealth / 5);
                        }
                        break;

					case ECrystalGemType.Silver:
					case ECrystalGemType.Turquoise:
					case ECrystalGemType.Yellow:
						GameManager.Instance.LevelScoreCounter.GetComponent<LevelScoreCounter>().AddScore(_score);
						break;

					default:
						break;
                }

                // Disable SpriteRenderer component
				_spriteRenderer.enabled = false;
                // Disable CapsuleCollider2D component
                _capsuleCollider2D.enabled = false;
                // Set destroying flag to true
				_isDestroying = true;
                // Play particle system effect
				_particleSystem.Play();
				// Destroy gem after particle system ends playing
				StartCoroutine(WaitForDestroyCoroutine());
			}
		}

        private void OnBecameVisible()
        {
			_isVisible = true;
        }

        private void OnBecameInvisible()
        {
			_isVisible = false;
		}

        /// <summary>
        ///     Coroutine that destroys the gameObject after particle system effect finishes
        /// </summary>
        /// <returns>WaitForDestroyCoroutine</returns>
        private IEnumerator WaitForDestroyCoroutine()
        {
            // Wait for particle system to finish playing
            while (_particleSystem.isPlaying)
            {
				yield return null;
            }

            // Destroy gameObject
			Destroy(gameObject);
        }
    }
}
