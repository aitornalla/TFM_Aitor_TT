using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI
{
	public class LoadKunaiFX : MonoBehaviour
	{
		[SerializeField]
		private float _rotationSpeed;                                           // Kunai rotation speed

        // Use this for initialization
        private void Start()
		{
			StartCoroutine(LoadKunaiFXCoroutine(Time.realtimeSinceStartup));
		}

		//private void FixedUpdate()
		//{
		//	transform.Rotate(0.0f, 0.0f, Time.fixedDeltaTime * _rotationSpeed, Space.Self);
		//}

        /// <summary>
        ///     Coroutine for visual FX of rotating kunai
        /// </summary>
        /// <param name="lastTime">Last initial time of previous pass</param>
        /// <returns>The reference of the coroutine</returns>
        private IEnumerator LoadKunaiFXCoroutine(float lastTime)
        {
            // Assing initial time
			float l_time = Time.realtimeSinceStartup;
            // Calculate delta time since last pass
            float l_deltaTime = l_time - lastTime;
            // Rotate kunai
			transform.Rotate(0.0f, 0.0f, l_deltaTime * _rotationSpeed, Space.Self);
            // Yield coroutine
			yield return null;
            // Start coroutine again
			StartCoroutine(LoadKunaiFXCoroutine(l_time));
		}
	}
}
