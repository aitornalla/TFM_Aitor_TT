using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.AnimatedSpringboardController
{
	public class AnimatedSpringboard : MonoBehaviour
	{
		[SerializeField]
		private LayerMask _playerLayer;                 // Player layer to check conditions
		[SerializeField]
		private float _springForce = 1000.0f;           // Spring force

		private Animator _animator;

        private void Awake()
        {
			_animator = gameObject.GetComponent<Animator>();
        }

        // Use this for initialization
        private void Start()
		{

		}

		// Update is called once per frame
		private void Update()
		{

		}

        private void OnTriggerEnter2D(Collider2D collision)
        {
			// If player jumps on the springboard
			if (_playerLayer == (_playerLayer | (1 << collision.gameObject.layer)))
			{
				// Get RigidBody2D component
				Rigidbody2D l_rigidbody2D = collision.GetComponent<Rigidbody2D>();

                if (l_rigidbody2D != null)
                {
					// Set velocity y-component to 0.0f
					l_rigidbody2D.velocity = new Vector2(l_rigidbody2D.velocity.x, 0.0f);
					// Add force
					l_rigidbody2D.AddForce(new Vector2(0.0f, _springForce));

                    if (_animator != null)
                    {
						_animator.SetTrigger("Activate");
                    }
				}
			}
        }
    }
}
