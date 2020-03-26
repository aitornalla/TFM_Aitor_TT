using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.CustomClasses;

namespace Assets.Scripts.MovingPlatformController
{
    public enum EPlatformLinearMovement
    {
        PlatformHorizontal,
        PlatformVertical
    }

	public class MovingPlatform : MonoBehaviour
	{
		[SerializeField]
		private LayerMask _playerLayer;                             // Player layer to check conditions
		[SerializeField]
		private EPlatformLinearMovement _platformLinearMovement;    // Platform linear movement type
        [SerializeField]
		private EOscillatorFunction _oscillatorFunction;            // Oscillator function to move the platform
        [SerializeField]
		private float _frequency = 10.0f;                           // Moving frequency
		[SerializeField]
		private float _movementSemiLength = 10.0f;                  // Half the total movement of the platform
        [SerializeField]
		private float _initialPosition = 0.0f;                      // Initial platform position (must be between -semilength and +semilength)
		[SerializeField]
		private bool _reverseInitialDirection = false;              // Flag for intial platform direction

		private Oscillator _oscillator;                             // Oscillator object
		private float _oscillatorAngle_0;                           // Initial oscillator angle
		private float _platformPosition_0;                          // Platform previous position

		// Use this for initialization
		private void Start()
		{
            // Calculate initial oscillator angle
			_oscillatorAngle_0 = Mathf.Acos(_initialPosition / _movementSemiLength) * Mathf.Rad2Deg;
            // If initial platform movement is to the right, recalculate initial oscillator angle
            if (_reverseInitialDirection)
            {
				_oscillatorAngle_0 = 360.0f - _oscillatorAngle_0;
            }
			// Instantiate new Oscillator object
			_oscillator = new Oscillator(_oscillatorAngle_0, _frequency, _oscillatorFunction);
            // Translate platform to the initial position
            switch(_platformLinearMovement)
            {
				case EPlatformLinearMovement.PlatformHorizontal:
					transform.Translate(_initialPosition, 0.0f, 0.0f);
					break;
				case EPlatformLinearMovement.PlatformVertical:
					transform.Translate(0.0f, _initialPosition, 0.0f);
					break;
				default:
					break;
            }
		}

		private void FixedUpdate()
		{
			// Calculate oscillator position
			float l_pos = _oscillator.Oscillate(Time.fixedDeltaTime);
            // Calculate increment to translate
			float l_increment = l_pos * _movementSemiLength - _platformPosition_0;
            // Translate
			switch (_platformLinearMovement)
			{
				case EPlatformLinearMovement.PlatformHorizontal:
					transform.Translate(l_increment, 0.0f, 0.0f);
					break;
				case EPlatformLinearMovement.PlatformVertical:
					transform.Translate(0.0f, l_increment, 0.0f);
					break;
				default:
					break;
			}
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
