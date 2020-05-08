using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.EnemyController
{
	public class EnemyPointToPointAttacking : MonoBehaviour, IEnemy
	{
		[SerializeField]
		private Transform _leftLimit;                                           // Left limit movement
		[SerializeField]
		private Transform _rightLimit;                                          // Right limit movement
        [SerializeField]
        private Transform _slashPoint;                                          // Slash point
        [SerializeField]
        private LayerMask _playerLayerMask;                                     // Player layer mask
        [SerializeField]
        private Slider _healthSlider;                                           // Enemy health slider
        [SerializeField]
        private float _idleTime = 5.0f;                                         // Time for the enemy to be idle
        [SerializeField]
		private int _attackLoops = 0;                                           // Number of attack loops going from one limit to the other
        [SerializeField]
        private int _attackDamage = 25;                                         // Enemy attack damage
        [SerializeField]
        private float _attackForce = 300.0f;                                    // Back force applied to player when hit
        [SerializeField]
        private float _slashOverlapCircleRadius = 0.2f;                         // Slash overlap circle radius
        [SerializeField]
        private float _runSpeed = 4.0f;                                         // Enemy run speed
        [SerializeField]
        private int _maxHealth = 35;                                            // Enemy max health

        private bool _facingRight = true;                                       // Current facing direction
        private bool _isHurt = false;                                           // Flag for enemy hurt
        private int _health = 0;                                                // Enemy health
        private int _loopsLeft = 0;                                             // Loops left for the run slashing state
        private EEnemyStates _enemyState = EEnemyStates.Idle;                   // Holds the current enemy state
		private Animator _animator = null;                                      // Reference to Animator component
        private SpriteRenderer _spriteRenderer = null;                          // Reference to SpriteRenderer component
        private CapsuleCollider2D _capsuleCollider2D = null;                    // Reference to CapsuleCollider2D component
        private Coroutine _idleStateCoroutine = null;                           // Reference to coroutine

        private void Awake()
        {
            // Get Animator component
			_animator = GetComponent<Animator>();
            // Get SpriteRenderer component
            _spriteRenderer = GetComponent<SpriteRenderer>();
            // Get CapsuleCollider2D component
            _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        }

        // Use this for initialization
        private void Start()
		{
            _health = _maxHealth;

            _healthSlider.normalizedValue = (float)_health / (float)_maxHealth;
		}

        // Update is called once per frame
        //private void Update()
        //{
        //    
        //}

        private void FixedUpdate()
        {
            // Switch between the different states
            switch (_enemyState)
            {
                case EEnemyStates.Dead:
                    DeadState();
                    break;
                case EEnemyStates.Hurt:
                    HurtState();
                    break;
                case EEnemyStates.Idle:
                    IdleState();
                    break;
                case EEnemyStates.RunSlash:
                    RunSlashingState();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        ///     Dead state
        /// </summary>
        private void DeadState()
        {

        }

        /// <summary>
        ///     Hurt state
        /// </summary>
        private void HurtState()
        {

        }

        /// <summary>
        ///     Idle state
        /// </summary>
        private void IdleState()
        {
            // Start idle state coroutine once
            if (_idleStateCoroutine == null)
                _idleStateCoroutine = StartCoroutine(IdleStateCoroutine());
        }

        /// <summary>
        ///     Idle state coroutine waits for a period of time
        /// </summary>
        /// <returns>The reference to the coroutine</returns>
        private IEnumerator IdleStateCoroutine()
        {
            // stay sometime in idle state before changing to run slashing
            yield return new WaitForSeconds(_idleTime);
            // Manage flip
            FlipFacingDirection();
            // Set run slashing loops left
            _loopsLeft = _attackLoops;
            // Change state
            _enemyState = EEnemyStates.RunSlash;
            // Change animator state
            _animator.SetTrigger("ToRunSlashing");
            // Set variable to null
            _idleStateCoroutine = null;
        }

        /// <summary>
        ///     Run slashing state
        /// </summary>
        private void RunSlashingState()
        {
            // Set speed direction
            float l_runSpeed = _facingRight ? _runSpeed : _runSpeed * (-1.0f);
            // Move enemy
            transform.Translate(l_runSpeed * Time.fixedDeltaTime, 0.0f, 0.0f, Space.Self);
            // Whether the enemy is facing right or not...
            if (_facingRight)
            {
                // If right limit is reached
                if (transform.position.x > _rightLimit.position.x)
                {
                    // Put enemy in right limit position
                    transform.position = new Vector3(_rightLimit.position.x, transform.position.y, transform.position.z);
                    // Set remaining lopps
                    _loopsLeft--;
                    // If remaining loops are 0
                    if (_loopsLeft == 0)
                    {
                        // Change state
                        _enemyState = EEnemyStates.Idle;
                        // Change animator state
                        _animator.SetTrigger("ToIdle");

                        return;
                    }
                    // Manage flip
                    FlipFacingDirection();
                }
            }
            else
            {
                // If left limit is reached
                if (transform.position.x < _leftLimit.position.x)
                {
                    // Put enemy in left limit position
                    transform.position = new Vector3(_leftLimit.position.x, transform.position.y, transform.position.z);
                    // Manage flip
                    FlipFacingDirection();
                }
            }
        }

        /// <summary>
        ///     Manage flip
        /// </summary>
        private void FlipFacingDirection()
        {
            // Switch facing direction
            _facingRight = !_facingRight;
            // Flip player sprite in X axis
            _spriteRenderer.flipX = !_spriteRenderer.flipX;
            // Flip collider
            _capsuleCollider2D.offset = new Vector2(-_capsuleCollider2D.offset.x, _capsuleCollider2D.offset.y);
            // Flip slash point position
            _slashPoint.localPosition = new Vector3(-_slashPoint.localPosition.x, _slashPoint.localPosition.y, _slashPoint.localPosition.z);
        }

        /// <summary>
        ///     Enemy takes damage
        /// </summary>
        /// <param name="damage">Damage</param>
        public void TakeDamage(int damage)
        {
            // Only hurt enemy when in idle state
            if (_enemyState != EEnemyStates.Idle)
                return;

            // Take damage
            _health = _health - damage < 0 ? 0 : _health - damage;

            // Set health slider
            _healthSlider.normalizedValue = (float)_health / (float)_maxHealth;

            // Cancel coroutine
            if (_idleStateCoroutine != null)
            {
                StopCoroutine(_idleStateCoroutine);

                _idleStateCoroutine = null;
            }

            // Manage state depending on remaining health
            if (_health == 0)
            {
                // Disable collider
                _capsuleCollider2D.enabled = false;
                // Change state
                _enemyState = EEnemyStates.Dead;
                // Change animator state
                _animator.SetTrigger("IsDead");
            }
            else
            {
                // Set flag
                _isHurt = true;
                // Change state
                _enemyState = EEnemyStates.Hurt;
                // Change animator state
                _animator.SetBool("IsHurt", true);
            }
        }

        /// <summary>
        ///     Gets called when enemy dead animation ends
        /// </summary>
        public void EnemyDeadEndAnimationEvent()
        {
            // Destroy gameObject
            Destroy(this.gameObject);
        }

        /// <summary>
        ///     Gets called when enemy hurt animations ends
        /// </summary>
        public void EnemyHurtEndAnimationEvent()
        {
            // Set flag
            _isHurt = false;
            // Manage flip
            FlipFacingDirection();
            // Set run slashing loops left
            _loopsLeft = _attackLoops;
            // Change state
            _enemyState = EEnemyStates.RunSlash;
            // Change animator state
            _animator.SetBool("IsHurt", false);
        }

        /// <summary>
        ///     Gets called in the run slashing keyframe animation when it should damage the player
        /// </summary>
        public void EnemySlashAnimationEvent()
        {
            // Check if player has been hit
            Collider2D l_collider = Physics2D.OverlapCircle(_slashPoint.position, _slashOverlapCircleRadius, _playerLayerMask);
            // If player has been hit...
            if (l_collider != null)
            {
                // Apply damage
                l_collider.GetComponent<CharacterHealth>().TakeDamage(_attackDamage);
                // Apply force
                float l_hForce = transform.position.x > l_collider.transform.position.x ? -_attackForce : _attackForce;
                float l_vForce = _attackForce;

                l_collider.GetComponent<Rigidbody2D>().AddForce(new Vector2(l_hForce, l_vForce));
            }
        }
    }
}
