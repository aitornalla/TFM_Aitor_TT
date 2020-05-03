using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI
{
	public class LoadKunaiFX : MonoBehaviour
	{
		[SerializeField]
		private float _rotationSpeed;

        // Use this for initialization
        private void Start()
		{
			StartCoroutine(LoadKunaiFXCoroutine(Time.realtimeSinceStartup));
		}

		//private void FixedUpdate()
		//{
		//	transform.Rotate(0.0f, 0.0f, Time.fixedDeltaTime * _rotationSpeed, Space.Self);
		//}

        private IEnumerator LoadKunaiFXCoroutine(float lastTime)
        {
			float l_time = Time.realtimeSinceStartup;
			float l_deltaTime = l_time - lastTime;

			transform.Rotate(0.0f, 0.0f, l_deltaTime * _rotationSpeed, Space.Self);

			yield return null;

			StartCoroutine(LoadKunaiFXCoroutine(l_time));
		}
	}
}
