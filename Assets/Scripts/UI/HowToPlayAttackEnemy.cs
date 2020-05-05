using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI
{
	public class HowToPlayAttackEnemy : MonoBehaviour
	{
		[SerializeField]
		private GameObject _holder;
		[SerializeField]
		private Transform _ninjaTransform;                                      // Reference to ninja transform
		[SerializeField]
		private Transform _enemyTransform;                                      // Reference to enemy transform
		[SerializeField]
		private Transform _startPosition;                                       // Reference to start position
		[SerializeField]
		private Transform _attackPosition;                                      // Reference to attack position
		[SerializeField]
		private float _ninjaSpeed = 5.0f;                                       // Ninja spped
		[SerializeField]
		private float _waitSomeTime = 2.0f;                                     // Waiting times in the animation

		private Animator _ninjaAnimator = null;                                 // Reference to ninja Animator component
		private Animator _enemyAnimator = null;                                 // Reference to enemy Animator component
		private bool _isNinjaAttackFinished = false;                            // Flag for ninja attack
		private bool _isEnemyDead = false;                                      // Flag for enemy death
		private Coroutine _attackAnimationCoroutine = null;                     // Reference to coroutine

        private void Awake()
        {
			// Get ninja Animator component
			_ninjaAnimator = _ninjaTransform.GetComponent<Animator>();
			// Get enemy Animator component
			_enemyAnimator = _enemyTransform.GetComponent<Animator>();
        }

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

        private void OnEnable()
        {
            // Set holder gameObject active
			_holder.SetActive(true);
			// Start coroutine
			_attackAnimationCoroutine = StartCoroutine(AttackAnimationCoroutine());
			// Prepear everything
			_ninjaTransform.position = _startPosition.position;
			_ninjaAnimator.Rebind();
			_enemyAnimator.Rebind();
			_isNinjaAttackFinished = false;
			_isEnemyDead = false;
		}

        private void OnDisable()
        {
			// Stop coroutine
			StopCoroutine(_attackAnimationCoroutine);
			// Set holder gameObject to inactive
            if (_holder != null)
                _holder.SetActive(false);
        }

        /// <summary>
        ///     Coroutine for attack animation
        /// </summary>
        /// <returns>Reference to the coroutine</returns>
        private IEnumerator AttackAnimationCoroutine()
        {
            // Start waiting for some time
			yield return new WaitForSeconds(_waitSomeTime);
			// Assing initial time
			float l_time = Time.realtimeSinceStartup;
            float l_lastTime = Time.realtimeSinceStartup;
			// Calculate delta time since last pass
			float l_deltaTime = l_time - l_lastTime;
			// Change ninja animator state
			_ninjaAnimator.SetTrigger("ToRun");
            // Move ninja towards attack point
			while (_ninjaTransform.position.x < _attackPosition.position.x)
            {
				_ninjaTransform.Translate(l_deltaTime * _ninjaSpeed, 0.0f, 0.0f, Space.Self);

				yield return null;

				l_deltaTime = Time.realtimeSinceStartup - l_lastTime;

				l_lastTime = Time.realtimeSinceStartup;
            }
            // Set ninja on attack point
			_ninjaTransform.position = _attackPosition.position;
			// Change ninja animator state
			_ninjaAnimator.SetTrigger("ToAttack");
            // Wait until ninja attack is finished
            while (!_isNinjaAttackFinished)
            {
				yield return null;
            }
			// Set flag
			_isNinjaAttackFinished = false;
			// Change ninja animator state
			_ninjaAnimator.SetTrigger("ToIdle");
            // Wait until enemy is dead
            while (!_isEnemyDead)
            {
				yield return null;
            }
			// Change flag
			_isEnemyDead = false;
			// Wait for some time
			yield return new WaitForSeconds(_waitSomeTime);
			// Set ninja to start position
			_ninjaTransform.position = _startPosition.position;
			// Change enemy animator state
			_enemyAnimator.SetTrigger("ToIdle");
			// Repeat again
			_attackAnimationCoroutine = StartCoroutine(AttackAnimationCoroutine());
		}

        /// <summary>
        ///     Called in the keyframe when the enemy has to get hit
        /// </summary>
        public void AttackAnimation()
        {
			_enemyAnimator.SetTrigger("ToDead");
        }

        /// <summary>
        ///     Called when ninja attack animation has finished
        /// </summary>
        public void AttackAnimationFinished()
        {
			_isNinjaAttackFinished = true;
        }

        /// <summary>
        ///     Called when enemy death animation has finished
        /// </summary>
        public void EnemyDeadAnimationFinished()
        {
			_isEnemyDead = true;
        }
	}
}
