using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.BackgroundController
{
	public sealed class BackgroundParallax : MonoBehaviour
	{
		[SerializeField]
		private Transform _camera;                                              // Main camera transform
		[SerializeField]
		private float _parallaxEffect;                                          // Parallax effect strength

		private float _length, _startPosition;                                  // Variables for processing parallax effect

		// Use this for initialization
		private void Start()
		{
			_startPosition = transform.position.x;

			_length = GetComponent<SpriteRenderer>().bounds.size.x;
		}

		// Update is called once per frame
		private void Update()
		{
			float l_temp = _camera.position.x * (1.0f - _parallaxEffect);

			float l_dist = _camera.position.x * _parallaxEffect;

			transform.position = new Vector3(_startPosition + l_dist, transform.position.y, transform.position.z);

			if (l_temp > _startPosition + _length)
            {
				_startPosition += _length;
            }

            if (l_temp < _startPosition - _length)
            {
				_startPosition -= _length;
            }
		}
    }
}
