using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI
{
	public class KunaiMainMenuFX : MonoBehaviour
	{
		[SerializeField]
		private float _kunaiSpeedLower = 10.0f;                                 // Lower kunai speed limit
		[SerializeField]
		private float _kunaiSpeedUpper = 20.0f;                                 // Upper kunai speed limit
		[SerializeField]
		private float _kunaiRotationSpeed = -1750.0f;                           // Kunai rotation speed

		private float _kunaiSpeed = 0.0f;
		private float _initialRotation = 0.0f;
		private bool _kunaiRotates = false;

		// Use this for initialization
		private void Start()
		{
			_kunaiSpeed = _kunaiSpeedLower + Random.value * (_kunaiSpeedUpper - _kunaiSpeedLower);

			_kunaiRotates = (int)(Random.value * 1000) % 2 == 0;

			if (_kunaiRotates)
			{
				Destroy(this.gameObject, 10.0f);

				_initialRotation = transform.eulerAngles.z;
			}
		}

		// Update is called once per frame
		private void Update()
		{
            if (_kunaiRotates)
            {
				transform.Translate(
                    _kunaiSpeed * Time.deltaTime * Mathf.Cos(_initialRotation * Mathf.Deg2Rad),
					_kunaiSpeed * Time.deltaTime * Mathf.Sin(_initialRotation * Mathf.Deg2Rad),
                    0.0f, Space.World);

				transform.Rotate(0.0f, 0.0f, _kunaiRotationSpeed * Time.deltaTime);
            }
            else
            {
				transform.Translate(_kunaiSpeed * Time.deltaTime, 0.0f, 0.0f, Space.Self);
			}
		}

        private void OnBecameInvisible()
        {
            if (!_kunaiRotates)
			    Destroy(this.gameObject);
        }
    }
}
