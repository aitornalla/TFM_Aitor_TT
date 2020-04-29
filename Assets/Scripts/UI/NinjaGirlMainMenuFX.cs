using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI
{
	public class NinjaGirlMainMenuFX : MonoBehaviour
	{
		[SerializeField]
		private Transform _upperPosition;                                       // Initial upper position
		[SerializeField]
		private Transform _lowerPosition;                                       // Initial lower position
		[SerializeField]
		private Transform _rightPosition;                                       // Initial right position
		[SerializeField]
		private float _upperMovementDistance = 7.5f;                            // Upper distance to move sprite
		[SerializeField]
		private float _lowerMovementDistance = 7.5f;                            // Lower distance to move sprite
		[SerializeField]
		private float _rightMovementDistance = 8.0f;                            // Right distance to move sprite
		[SerializeField]
		private float _spriteSpeed = 10.0f;                                     // Speed to make the sprite move
        [SerializeField]
		private float _timeOnScreen = 5.0f;                                     // The time the FX is on the screen
		[SerializeField]
		private float _timeToRepeat = 5.0f;                                     // The time before repeating again de FX


		private SpriteRenderer _spriteRenderer;                                 // Sprite renderer component

		private void Awake()
        {
			_spriteRenderer = GetComponent<SpriteRenderer>();
        }

        // Use this for initialization
        private void Start()
		{
            StartCoroutine(NinjaGirlMainMenuFXCoroutine(1, true));
		}

		// Update is called once per frame
		//private void Update()
		//{
        //
		//}

        private IEnumerator NinjaGirlMainMenuFXCoroutine(int lastPosition, bool facingRight)
        {
            // If sprite was not facing right direction, put it facing right direction
			if (!facingRight)
			{
				_spriteRenderer.flipX = !_spriteRenderer.flipX;
			}
            // Rotate sprite to its original rotation
			switch (lastPosition)
			{
				case 0:
					transform.Rotate(0.0f, 0.0f, -180.0f);
					break;
				case 1:
					break;
				case 2:
					transform.Rotate(0.0f, 0.0f, -90.0f);
					break;
				default:
					break;
			}
            // Get rando value from 0.0f to 1.0f
            float l_value = Random.value;
            // Get a position depending on the random value
			int l_pos = l_value <= 1.0f / 3.0f ? 0 : (l_value > 2.0f / 3.0f ? 2 : 1);
            // Assing movement distance according to the position
			float l_dist = l_pos == 0 ? _upperMovementDistance : (l_pos == 1 ? _lowerMovementDistance : _rightMovementDistance);
            // Put sprite facing right or left (random)
            bool l_facingRight = (int)(Random.value * 1000) % 2 == 0;
            // Change sprite facing direction
            if (!l_facingRight)
            {
				_spriteRenderer.flipX = !_spriteRenderer.flipX;
            }
            //
			float l_initialPos = 0.0f;
            // Switch behaviour depending on the position
			switch (l_pos)
			{
				case 0:
					{
                        // Move and rotate sprite to the current position and rotation
						transform.position = _upperPosition.position;
						transform.Rotate(0.0f, 0.0f, 180.0f);
                        // Assing initial position reference
                        l_initialPos = transform.position.y;
                        // Move sprite to make it appear on the screen
						while (l_initialPos - l_dist < transform.position.y)
						{
							transform.Translate(0.0f, _spriteSpeed * Time.deltaTime, 0.0f, Space.Self);

							yield return null;
						}
						// Assing initial position reference
						l_initialPos = transform.position.y;
                        // Wait some time before making the sprite hide again
						yield return new WaitForSeconds(_timeOnScreen);
                        // Move the sprite to make it hide
						while (l_initialPos + l_dist > transform.position.y)
						{
							transform.Translate(0.0f, -_spriteSpeed * Time.deltaTime, 0.0f, Space.Self);

							yield return null;
						}
					}
					break;

				case 1:
					{
						// Move and rotate sprite to the current position and rotation
						transform.position = _lowerPosition.position;
						// Assing initial position reference
						l_initialPos = transform.position.y;
						// Move sprite to make it appear on the screen
						while (l_initialPos + l_dist > transform.position.y)
						{
							transform.Translate(0.0f, _spriteSpeed * Time.deltaTime, 0.0f, Space.Self);

							yield return null;
						}
						// Assing initial position reference
						l_initialPos = transform.position.y;
						// Wait some time before making the sprite hide again
						yield return new WaitForSeconds(_timeOnScreen);
						// Move the sprite to make it hide
						while (l_initialPos - l_dist < transform.position.y)
						{
							transform.Translate(0.0f, -_spriteSpeed * Time.deltaTime, 0.0f, Space.Self);

							yield return null;
						}
					}
					break;

				case 2:
					{
						// Move and rotate sprite to the current position and rotation
						transform.position = _rightPosition.position;
						transform.Rotate(0.0f, 0.0f, 90.0f);
						// Assing initial position reference
						l_initialPos = transform.position.x;
						// Move sprite to make it appear on the screen
						while (l_initialPos - l_dist < transform.position.x)
						{
							transform.Translate(0.0f, _spriteSpeed * Time.deltaTime, 0.0f, Space.Self);

							yield return null;
						}
						// Assing initial position reference
						l_initialPos = transform.position.x;
						// Wait some time before making the sprite hide again
						yield return new WaitForSeconds(_timeOnScreen);
						// Move the sprite to make it hide
						while (l_initialPos + l_dist > transform.position.x)
						{
							transform.Translate(0.0f, -_spriteSpeed * Time.deltaTime, 0.0f, Space.Self);

							yield return null;
						}
					}
					break;

				default:
					break;
			}
            // Wait some time before repeating all over again
			yield return new WaitForSeconds(_timeToRepeat);
            // Repeat again
			StartCoroutine(NinjaGirlMainMenuFXCoroutine(l_pos, l_facingRight));
		}
	}
}
