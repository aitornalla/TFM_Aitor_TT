using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Player;
using UnityEngine;

namespace Assets.Scripts.EnemyController
{
	public class SamuraiHeavyBossKunai : MonoBehaviour
	{
		[SerializeField]
		private LayerMask _playerLayer;                                         // Player layer mask
		[SerializeField]
		private float _speed = 5.0f;                                            // Kunai speed
		[SerializeField]
		private int _damage = 5;                                                // Kunai damage

		// Use this for initialization
		private void Start()
		{

		}

		// Update is called once per frame
		private void Update()
		{

		}

        private void FixedUpdate()
        {
			transform.Translate(_speed * Time.fixedDeltaTime, 0.0f, 0.0f, Space.Self);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
			// If fireball hits the player
			if (_playerLayer == (_playerLayer | (1 << collision.gameObject.layer)))
			{
				// Get CharacterHealth component
				CharacterHealth l_characterHealth = collision.gameObject.GetComponent<CharacterHealth>();
				// Take damage
				l_characterHealth.TakeDamage(_damage);
				// Destroy kunai
				Destroy(this.gameObject);
			}
            else
            {
				// Destroy kunai
				Destroy(this.gameObject);
			}
		}
    }
}
