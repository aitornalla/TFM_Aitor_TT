using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.CustomClasses;
using Assets.Scripts.Player;
using UnityEngine;

namespace Assets.Scripts.Traps.SwingingSpikeController
{
	public sealed class SwingingSpike : MonoBehaviour
	{
		[SerializeField]
		private LayerMask _playerLayer;                     // Player layer to check conditions
		[SerializeField]
		private int _damage = 15;                           // Damage taken by the player when colliding with the spike
		[SerializeField]
		private Transform _nut;                             // Nut gameObject to rotate around
		[SerializeField]
		private Transform _ropechain;                       // gameObject with rope/chain elements to rotate
		[SerializeField] [Range(180.0f, 270.0f)]
		private float _angleLimit1 = 210.0f;                // Limit angle used to apply the force (in degrees)
		[SerializeField] [Range(270.0f, 360.0f)]
		private float _angleLimit2 = 330.0f;                // Limit angle used to apply the force (in degrees)
		[SerializeField]
		private float _frequency = 10.0f;                   // Swinging frequency
		[SerializeField]
		private float _bumpForce = 400.0f;                  // Force to push the player back when hit
		[SerializeField] [Range(0.0f, 90.0f)]
		private float _bumpForceAngle = 45.0f;              // Angle of bump force


		private Oscillator _oscillator;                     // Oscillator object
		private float _oscillatorAngle_0 = 180.0f;          // Initial oscillator angle
		private const float InitialSpriteRot = 270.0f;      // Initial sprite angle
		private float _angle;                               // Current angle
		private float _angle_0;                             // Angle previous value

		// Use this for initialization
		private void Start()
		{
            // Rotate spike and rope/chain to initial angle
            transform.RotateAround(_nut.transform.position, Vector3.forward, _angleLimit1 - InitialSpriteRot);
			_ropechain.RotateAround(_nut.transform.position, Vector3.forward, _angleLimit1 - InitialSpriteRot);
			// Assing first limit to initial angle
			_angle_0 = _angleLimit1;
			// Instantiate new Oscillator object
			_oscillator = new Oscillator(_oscillatorAngle_0, _frequency, EOscillatorFunction.CosFunction);
		}

		private void FixedUpdate()
		{
			// Calculate oscillator position
			float l_pos = _oscillator.Oscillate(Time.fixedDeltaTime);
			// Calculate percentage within angle limits
			float l_percent = Mathf.Abs(l_pos - (-1.0f)) / 2.0f;
			// Calculate new angle
			_angle = _angleLimit1 - (_angleLimit1 - _angleLimit2) * l_percent;
			// Calculate increment to rotate
			float l_increment = _angle - _angle_0;
			// Rotate arrow
			transform.RotateAround(_nut.transform.position, Vector3.forward, l_increment);
			_ropechain.transform.RotateAround(_nut.transform.position, Vector3.forward, l_increment);
			// Assign current angle to angle 0 for next update
			_angle_0 = _angle;
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
