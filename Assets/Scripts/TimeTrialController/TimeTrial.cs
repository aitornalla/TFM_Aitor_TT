using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.CustomClasses;
using Assets.Scripts.CustomClasses.OscillatorFunctions;
using Assets.Scripts.GameManagerController;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.TimeTrialController
{
	public class TimeTrial : MonoBehaviour
	{
		[SerializeField]
		private LayerMask _playerLayer;                                         // Player layer to check conditions
		[SerializeField]
		private Text _timeTrialText;                                            // Time trial UI text
		[SerializeField]
		private Image _timeTrialImage;                                          // Time trial sand clock UI image
		[SerializeField]
		private Sprite[] _sandClockSprites;                                     // Sandclock sprites for animation
        [SerializeField]
		private GameObject[] _gameObjectsToDisable;                             // GameObjects to disable when time trial

        [Header("Oscillator Params")]
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

		private Oscillator _oscillator;                                         // Oscillator object
		private float _gemPosition_0;                                           // Gem previous position
		private float _movementSemiLength;

		private System.Diagnostics.Stopwatch _stopwatch = null;                 // Stopwatch for time
		private bool _isTimeTrial = false;                                      // Flag for time trial
		private SpriteRenderer _spriteRenderer = null;                          // Reference to SpriteRenderer component
		//private ParticleSystem _particleSystem = null;                          // Reference to ParticleSystem component
		private CircleCollider2D _circleCollider2D = null;                      // Reference to CircleCollider2D component
		private Coroutine _sandClockAnimationCoroutine = null;                  // Reference to coroutine

		public bool IsTimeTrial { get { return _isTimeTrial; } }
        public TimeSpan TimeTrialElapsedTime { get { return _stopwatch.Elapsed; } }

		// Use this for initialization
		private void Start()
		{
            // Get SpriteRenderer component
			_spriteRenderer = GetComponent<SpriteRenderer>();
			// Get ParticleSystem component
			//_particleSystem = GetComponent<ParticleSystem>();
			// Get CircleCollider2D component
			_circleCollider2D = GetComponent<CircleCollider2D>();

			// Instantiate Stopwatch
			_stopwatch = new System.Diagnostics.Stopwatch();

            // Oscillator
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
			// Translate gem to the initial position
			transform.Translate(_initialPosition.localPosition.x, 0.0f, 0.0f, Space.Self);
			// Assing initial gem position 0
			_gemPosition_0 = _initialPosition.localPosition.x;
		}

		// Update is called once per frame
		private void Update()
		{
            if (_isTimeTrial && _stopwatch.IsRunning)
            {
                // Show elapsed time
                _timeTrialText.text =
				    _stopwatch.Elapsed.Minutes.ToString("D2") + ":" +
				    _stopwatch.Elapsed.Seconds.ToString("D2") + ":" +
				    _stopwatch.Elapsed.Milliseconds.ToString("D3");
			}
            else
            {
				// Calculate oscillator position
				float l_pos = _oscillator.Oscillate(Time.deltaTime);
				// Calculate increment to translate
				float l_increment = l_pos * _movementSemiLength - _gemPosition_0;
				// Translate
				transform.Translate(0.0f, l_increment, 0.0f, Space.Self);
				// Assign increment to the platform position 0 for next frame
				_gemPosition_0 += l_increment;
			}
		}

        private void OnTriggerEnter2D(Collider2D collision)
        {
			// If player falls on a static land spike instant death
			if (_playerLayer == (_playerLayer | (1 << collision.gameObject.layer)))
			{
				// Disable components
				_spriteRenderer.enabled = false;
				//_particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
				_circleCollider2D.enabled = false;
                // Disable gameObjects
                for (int i = 0; i < _gameObjectsToDisable.Length; i++)
                {
					_gameObjectsToDisable[i].SetActive(false);
				}
				// Set time trial flag
				_isTimeTrial = true;
				// Enable time trial UI text
				_timeTrialText.gameObject.SetActive(true);
				// Enable time trial UI image
				_timeTrialImage.gameObject.SetActive(true);
				// Start sand clock animation coroutine
				_sandClockAnimationCoroutine = StartCoroutine(SandClockAnimationCoroutine());
				// Start Stopwatch
				_stopwatch.Start();
			}
		}

        private IEnumerator SandClockAnimationCoroutine()
        {
            // Change sand clock sprite to animate
            for (int i = 1; i < _sandClockSprites.Length; i++)
            {
				_timeTrialImage.sprite = _sandClockSprites[i];

				yield return new WaitForSeconds(0.15f);
            }
            // Start coroutine again
			_sandClockAnimationCoroutine = StartCoroutine(SandClockAnimationCoroutine());
        }

        public void EndTimeTrial()
        {
			// Stop Stopwatch
			_stopwatch.Stop();

			// Stop sandclock animation coroutine
			StopCoroutine(_sandClockAnimationCoroutine);

			// Set sandclock full sprite
			_timeTrialImage.sprite = _sandClockSprites[0];

			// Update level time
			string l_levelName = SceneManager.GetActiveScene().name;

			GameManager.Instance.UpdateLevelTime(l_levelName, _stopwatch.Elapsed);
        }

		public void PauseTimeTrial()
		{
			// Pause Stopwatch
			_stopwatch.Stop();
		}

		public void ResumeTimeTrial()
        {
			// Resume Stopwatch
			_stopwatch.Start();
        }
	}
}
