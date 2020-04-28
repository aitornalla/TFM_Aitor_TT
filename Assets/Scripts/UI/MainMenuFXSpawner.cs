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
		private void Update()
		{

		}

		private IEnumerator KunaiMainMenuFXCoroutine()
		{
			float l_angle = Random.value * 360.0f;

			GameObject l_prefab = Instantiate(_kunaiMainMenuPrefab);

			l_prefab.transform.position = new Vector3(_kunaiSpawnRadii * Mathf.Cos(l_angle * Mathf.Deg2Rad), _kunaiSpawnRadii * Mathf.Sin(l_angle * Mathf.Deg2Rad), 0.0f);

			l_prefab.transform.Rotate(0.0f, 0.0f, l_angle + 180.0f);

			yield return new WaitForSeconds(_kunaiFXSpawnTimeLower + Random.value * (_kunaiFXSpawnTimeUpper - _kunaiFXSpawnTimeLower));

			StartCoroutine(KunaiMainMenuFXCoroutine());
		}
	}
}
