using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI
{
	public class MainMenuFXSpawner : MonoBehaviour
	{
		[SerializeField]
		private GameObject _kunaiMainMenuPrefab;                                // Reference to kunai main menu prefab
		[SerializeField]
		private float _kunaiSpawnRadii = 15.0f;                                 // Kunai spawn radii
		[SerializeField]
		private float _kunaiFXSpawnTimeLower = 3.0f;                            // Time between kunai spawns (lower limit)
		[SerializeField]
		private float _kunaiFXSpawnTimeUpper = 6.0f;                            // Time between kunai spawns (upper limit)


		// Use this for initialization
		private void Start()
		{
			StartCoroutine(KunaiMainMenuFXCoroutine());
		}

		// Update is called once per frame
		//private void Update()
		//{
        //
		//}

        /// <summary>
        ///     Coroutine that spawns a kunai in random intervals
        /// </summary>
        /// <returns>The coroutine reference</returns>
		private IEnumerator KunaiMainMenuFXCoroutine()
		{
            // Get a random direction angle
			float l_angle = Random.value * 360.0f;
            // Instantiate the kunai prefab
			GameObject l_prefab = Instantiate(_kunaiMainMenuPrefab);
            // Set the kunai gameObject in the correct position
			l_prefab.transform.position = new Vector3(_kunaiSpawnRadii * Mathf.Cos(l_angle * Mathf.Deg2Rad), _kunaiSpawnRadii * Mathf.Sin(l_angle * Mathf.Deg2Rad), 0.0f);
            // Rotate the kunai gameObject in the correct direction
			l_prefab.transform.Rotate(0.0f, 0.0f, l_angle + 180.0f);
            // Yield coroutine a random time
			yield return new WaitForSeconds(_kunaiFXSpawnTimeLower + Random.value * (_kunaiFXSpawnTimeUpper - _kunaiFXSpawnTimeLower));
            // Start coroutine again
			StartCoroutine(KunaiMainMenuFXCoroutine());
		}
	}
}
