using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Player;
using UnityEngine;

namespace Assets.Scripts.Traps.FireballController
{
	public sealed class Fireball : MonoBehaviour
	{
		[SerializeField]
		private LayerMask _playerLayer;                             // Player layer to check conditions
		[SerializeField]
		private int _damage = 15;                                   // Damage taken by the player when hit with the fireball
		[SerializeField]
		private float _velocity = 5.0f;                             // Velocity of the fireball
		[SerializeField]
		private float _lifetime = 5.0f;                             // Fireball lifetime in seconds (s)

		private Animator _animator;                                 // Reference to the Animator component 
		private CapsuleCollider2D _capsuleCollider2D;               // Reference to the CapsuleCollider2D component
		private Coroutine _coroutine;                               // Variable to store coroutine reference
        private bool _isDestroying = false;                         // Flag for when the fireball is destroying
		private bool _firstCollision = false;                       // Flag to check for fist collision (should be containing collider) and not destroy de fireball

        #region Properties
        public int Damage
		{
			get
			{
				return _damage;
			}
			set
			{
				_damage = value;
			}
		}

        public float Velocity
		{
			get
			{
				return _velocity;
			}
			set
			{
				_velocity = value;
			}
		}

        public float Lifetime
		{
			get
			{
				return _lifetime;
			}
			set
			{
				_lifetime = value;
			}
		}
        #endregion

        private void Awake()
        {
			// Get Animator component
			_animator = GetComponent<Animator>();
			// Get CapsuleCollider2D component
			_capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        }

        // Use this for initialization
        private void Start()
		{
            // Start coroutine to destroy fireball
			_coroutine = StartCoroutine(LifetimeBeforeDestroy());
		}

		private void FixedUpdate()
		{
            if (!_isDestroying)
                // Move fireball
			    transform.Translate(-_velocity * Time.fixedDeltaTime, 0.0f, 0.0f, Space.Self);
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
				//// Get Rigidbody2D component
				//Rigidbody2D l_rigidbody2D = collision.gameObject.GetComponent<Rigidbody2D>();
				//// Force vector initialized
				//Vector2 l_force = Vector2.zero;
				//// Depending on a left/right collision, calculate force vector
				//if (Mathf.Sign(collision.gameObject.transform.position.x - transform.position.x) == 1.0f)
				//{
				//	l_force.x = _bumpForce * Mathf.Cos(_bumpForceAngle * Mathf.Deg2Rad);
				//	l_force.y = _bumpForce * Mathf.Sin(_bumpForceAngle * Mathf.Deg2Rad);
				//}
				//else
				//{
				//	l_force.x = _bumpForce * Mathf.Cos((_bumpForceAngle + 90.0f) * Mathf.Deg2Rad);
				//	l_force.y = _bumpForce * Mathf.Sin((_bumpForceAngle + 90.0f) * Mathf.Deg2Rad);
				//}
				//// Stop rigidbody before applying the force
				//l_rigidbody2D.velocity = Vector2.zero;
				//// Apply force
				//l_rigidbody2D.AddForce(l_force);
			}
            else
            {
                // First collision should be with containing collider
				if (!_firstCollision)
                {
					_firstCollision = true;
					return;
                }
            }

			// Disable collider component
			_capsuleCollider2D.enabled = false;

            // If capsule is not yet destroying
            if (!_isDestroying)
            {
                // Stop lifetime coroutine
				StopCoroutine(_coroutine);
				// Set fireball destroy animation
				_animator.SetTrigger("Destroy");
				// Set destroying flag
				_isDestroying = true;
			}
		}

        private IEnumerator LifetimeBeforeDestroy()
        {
            // Wait lifetime seconds before destroying de fireball
			yield return new WaitForSeconds(_lifetime);
            // Set fireball destroy animation
			_animator.SetTrigger("Destroy");
			// Set destroying flag
			_isDestroying = true;
        }

        // Called by animation event
        public void DestroyFireball()
        {
			// Destroy fireball after lifetime
			Destroy(gameObject);
        }
	}
}
