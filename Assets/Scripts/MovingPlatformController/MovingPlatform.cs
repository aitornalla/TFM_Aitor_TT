using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.CustomClasses;
using Assets.Scripts.CustomClasses.OscillatorFunctions;

namespace Assets.Scripts.MovingPlatformController
{
    public sealed class MovingPlatform : MonoBehaviour
	{
		[SerializeField]
		private LayerMask _playerLayer;                             // Player layer to check conditions
        [SerializeField]
		private float _frequency = 10.0f;                           // Moving frequency
		[SerializeField]
		private float _movementSemiLength = 10.0f;                  // Half the total movement of the platform
        [SerializeField]
		private float _initialPosition = 0.0f;                      // Initial platform position (must be between -semilength and +semilength)
		[SerializeField] [Range(0.0f, 180.0f)]
		private float _platformDirection = 0.0f;                    // Platform direction in degrees
        [SerializeField]
		private bool _reverseInitialDirection = false;              // Flag for intial platform direction

		private Oscillator _oscillator;                             // Oscillator object
		private float _platformPosition_0;                          // Platform previous position

		// Use this for initialization
		private void Start()
		{
            // Calculate initial oscillator angle
			float l_oscillatorAngle_0 = Mathf.Acos(_initialPosition / _movementSemiLength) * Mathf.Rad2Deg;
            // If initial platform movement is to the right, recalculate initial oscillator angle
            if (_reverseInitialDirection)
            {
				l_oscillatorAngle_0 = 360.0f - l_oscillatorAngle_0;
            }
			// Instantiate new Oscillator object
			_oscillator = new Oscillator(l_oscillatorAngle_0, _frequency, new CosOscillatorFunction(l_oscillatorAngle_0, _frequency));
            // Translate platform to the initial position
            transform.Translate(
                _initialPosition * Mathf.Cos(_platformDirection * Mathf.Deg2Rad),
                _initialPosition * Mathf.Sin(_platformDirection * Mathf.Deg2Rad),
                0.0f);

            // Assing initial platform position 0
			_platformPosition_0 = _initialPosition;
		}

		private void FixedUpdate()
		{
			// Calculate oscillator position
			float l_pos = _oscillator.Oscillate(Time.fixedDeltaTime);
            // Calculate increment to translate
			float l_increment = l_pos * _movementSemiLength - _platformPosition_0;
            // Translate
			transform.Translate(
				l_increment * Mathf.Cos(_platformDirection * Mathf.Deg2Rad),
				l_increment * Mathf.Sin(_platformDirection * Mathf.Deg2Rad),
				0.0f);

			// Assign increment to the platform position 0 for next frame
			_platformPosition_0 += l_increment; 
		}

		private void OnCollisionEnter2D(Collision2D collision)
        {
			// If player jumps on the platform set its parent to platform gameObject
			if (_playerLayer == (_playerLayer | (1 << collision.gameObject.layer)))
			{
				collision.gameObject.transform.SetParent(this.transform);
			}
		}

		private void OnCollisionExit2D(Collision2D collision)
		{
			// If player jumps off the platform set its parent to null
			if (_playerLayer == (_playerLayer | (1 << collision.gameObject.layer)))
			{
				collision.gameObject.transform.SetParent(null);
			}
		}
	}
}
