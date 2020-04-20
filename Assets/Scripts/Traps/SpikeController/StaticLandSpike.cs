using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Player;

namespace Assets.Scripts.Traps.SpikeController
{
	public sealed class StaticLandSpike : MonoBehaviour
	{
		[SerializeField]
		private LayerMask _playerLayer;                                         // Player layer to check conditions

		// Use this for initialization
		//private void Start()
		//{
        //
		//}

		// Update is called once per frame
		//private void Update()
		//{
        //
		//}

		private void OnCollisionEnter2D(Collision2D collision)
		{
			// If player falls on a static land spike instant death
			if (_playerLayer == (_playerLayer | (1 << collision.gameObject.layer)))
			{
                // Get CharacterHealth component
				CharacterHealth l_characterHealth = collision.gameObject.GetComponent<CharacterHealth>();
                // Take damage equal to player max health
				l_characterHealth.TakeDamage(l_characterHealth.PlayerMaxHealth);
			}
		}
	}
}
