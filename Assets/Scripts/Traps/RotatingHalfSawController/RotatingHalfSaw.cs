using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.CustomClasses;
using Assets.Scripts.CustomClasses.OscillatorFunctions;
using Assets.Scripts.Player;
using UnityEngine;

namespace Assets.Scripts.Traps.RotatingHalfSawController
{
	public sealed class RotatingHalfSaw : MonoBehaviour
	{
		[SerializeField]
		private LayerMask _playerLayer;                                         // Player layer to check conditions
		[SerializeField]
		private int _damage = 15;                                               // Damage taken by the player when colliding with the rotating saw
		[SerializeField]
		private Transform _leftLimit;                                           // Movement left limit
        [SerializeField]
        private Transform _rightLimit;                                          // Movement right limit
		[SerializeField]
		private Transform _initialPosition;                                     // Initial saw position (one of the two limits)
		[SerializeField]
		private float _frequency = 10.0f;                                       // Moving frequency
		[SerializeField]
		private int _intensity = 1;                                             // Intensity of the oscillations
		[SerializeField]
		private float _strikes = 1.0f;                                          // Number of strikes per oscillation
        [SerializeField]
		private bool _reverseInitialDirection = false;                          // Flag for intial saw direction
		[SerializeField]
		private float _bumpForce = 400.0f;                                      // Force to push the player back when hit
		[SerializeField] [Range(0.0f, 90.0f)]
		private float _bumpForceAngle = 45.0f;                                  // Angle of bump force

		private Oscillator _oscillator;                                         // Oscillator object
		private float _sawPosition_0;                                           // Saw previous position
		private float _movementSemiLength;

		// Use this for initialization
		private void Start()
		{
			// Calculate total movement length
			_movementSemiLength = Mathf.Abs(_rightLimit.localPosition.x - _leftLimit.localPosition.x) / 2.0f;
			// Calculate initial oscillator angle
			float l_oscillatorAngle_0 = Mathf.Acos(_initialPosition.localPosition.x / _movementSemiLength) * Mathf.Rad2Deg;
			// If initial platform movement is to the right, recalculate initial oscillator angle
			if (_reverseInitialDirection)
			{
				l_oscillatorAngle_0 = 360.0f - l_oscillatorAngle_0;
			}
			// Instantiate new Oscillator object
			_oscillator = new Oscillator(l_oscillatorAngle_0, _frequency, new CosStrikeOscillatorFunction(l_oscillatorAngle_0, _frequency, _intensity, _strikes, _initialPosition.Equals(_rightLimit) ? 0.0f : Mathf.PI));
			// Translate saw to the initial position
			transform.Translate(_initialPosition.localPosition.x, 0.0f, 0.0f, Space.Self);
			// Assing initial saw position 0
			_sawPosition_0 = _initialPosition.localPosition.x;
		}

		private void FixedUpdate()
		{
			// Calculate oscillator position
			float l_pos = _oscillator.Oscillate(Time.fixedDeltaTime);
			// Calculate increment to translate
			float l_increment = l_pos * _movementSemiLength - _sawPosition_0;
            // Translate
			transform.Translate(l_increment, 0.0f, 0.0f, Space.Self);
			// Assign increment to the platform position 0 for next frame
			_sawPosition_0 += l_increment;
		}

        private void OnTriggerEnter2D(Collider2D collision)
        {
			// If player collides with the saw, take damage and get bumped away
			if (_playerLayer == (_playerLayer | (1 << collision.gameObject.layer)))
			{
				// Get CharacterHealth component
				CharacterHealth l_characterHealth = collision.gameObject.GetComponent<CharacterHealth>();
				// Take damage
				l_characterHealth.TakeDamage(_damage);
				// Get Rigidbody2D component
				Rigidbody2D l_rigidbody2D = collision.gameObject.GetComponent<Rigidbody2D>();
                // Force vector initialized
				Vector2 l_force = Vector2.zero;
                // Depending on a left/right collision, calculate force vector
                if (Mathf.Sign(collision.gameObject.transform.position.x - transform.position.x) == 1.0f)
                {
					l_force.x = _bumpForce * Mathf.Cos(_bumpForceAngle * Mathf.Deg2Rad);
					l_force.y = _bumpForce * Mathf.Sin(_bumpForceAngle * Mathf.Deg2Rad);
				}
                else
                {
					l_force.x = _bumpForce * Mathf.Cos((_bumpForceAngle + 90.0f) * Mathf.Deg2Rad);
					l_force.y = _bumpForce * Mathf.Sin((_bumpForceAngle + 90.0f) * Mathf.Deg2Rad);
				}
                // Stop rigidbody before applying the force
				l_rigidbody2D.velocity = Vector2.zero;
                // Apply force
				l_rigidbody2D.AddForce(l_force);
			}
		}
	}
}
