using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.GameManagerController;
using Assets.Scripts.Traps.FireballController;
using UnityEngine;

namespace Assets.Scripts.Traps.FireballHoleController
{
	public sealed class FireballHole : MonoBehaviour
	{
		[SerializeField]
		private GameObject _fireballPrefab;                         // Fireball prefab
		[SerializeField]
		private Transform _fireballSpawnPosition;                   // Fireball spawn position
        [SerializeField]
		private float _initialDelay = 0.0f;                         // Initial delay before start shooting
		[SerializeField]
		private float _delayBetweenShots = 1.0f;                    // Delay between shots

		[Header("Fireball properties")]
		[SerializeField]
		private int _fireballDamage = 15;
		[SerializeField]
		private float _fireballVelocity = 5.0f;
		[SerializeField]
		private float _fireballLifetime = 5.0f;

		private float _rotationZ = 0.0f;                            // Fireball hole rotation in world space

		// Use this for initialization
		private void Start()
		{
            // Get initial fireball hole rotation for fireball instantiation
			_rotationZ = transform.eulerAngles.z;
            // Start initial coroutine for delay and shooting
			StartCoroutine(InitialShootCoroutine());
		}

		// Update is called once per frame
		//private void Update()
		//{
        //
		//}

        /// <summary>
        ///     Waits for initial delay and starts shooting fireballs
        /// </summary>
        /// <returns>Initial shoot coroutine</returns>
        private IEnumerator InitialShootCoroutine()
        {
            if (_initialDelay > 0.0f)
            {
                // Wait for initial delay
				yield return new WaitForSeconds(_initialDelay);
            }

            // Start shooting fireballs
			StartCoroutine(ShootFireballCoroutine());
        }

        /// <summary>
        ///     Coroutine for shooting fireballs
        /// </summary>
        /// <returns>Shoot fireball coroutine</returns>
        private IEnumerator ShootFireballCoroutine()
        {
			// Instantiate fireball prefab
			GameObject l_prefab = Instantiate(_fireballPrefab, GameManager.Instance.PrefabContainer);
            // Set fireball to spawn position
			l_prefab.transform.position = _fireballSpawnPosition.position;
            // Set fireball rotation to match hole's rotation
			l_prefab.transform.Rotate(0.0f, 0.0f, _rotationZ);
            // Get Fireball component
			Fireball l_fireball = l_prefab.GetComponent<Fireball>();
            // Set fireball properties
			l_fireball.Damage = _fireballDamage;
			l_fireball.Velocity = _fireballVelocity;
			l_fireball.Lifetime = _fireballLifetime;
            // Wait between shots
			yield return new WaitForSeconds(_delayBetweenShots);
            // Start new coroutine
			StartCoroutine(ShootFireballCoroutine());
		}
	}
}
