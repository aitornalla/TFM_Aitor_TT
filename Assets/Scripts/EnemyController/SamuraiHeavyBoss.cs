using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.GameManagerController;
using Assets.Scripts.LevelEndController;
using Assets.Scripts.Player;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace Assets.Scripts.EnemyController
{
	public class SamuraiHeavyBoss : MonoBehaviour, IEnemy
	{
        [Header("Fase 1")]
		[SerializeField]
		private Transform _f1LeftLimit;                                         // F1 left limit movement
		[SerializeField]
		private Transform _f1RightLimit;                                        // F1 right limit movement

        [Header("Fase 2")]
		[SerializeField]
		private Transform _f2PlatformPosition;                                  // F2 boss platform position
		[SerializeField]
		private Transform _f2KunaiSpawnerPosition;                              // F2 boss kunai spawner position
		[SerializeField]
		private GameObject _f2FirstPlatform;                                    // First platform in F2 to go up
		[SerializeField]
		private GameObject[] _f2OtherPlatforms;                                 // Other platforms in F2
        [SerializeField]
		private GameObject _f2MiddleSpikes;                                     // Middle spikes F2
		[SerializeField]
		private GameObject _f2LeftSpikes;                                       // Left spikes F2
		[SerializeField]
		private GameObject _f2RightSpikes;                                      // Right spikes F2
		[SerializeField]
		private GameObject _f2LeftPlatformSpikes;                               // Left platform spikes F2
		[SerializeField]
		private GameObject _f2RightPlatformSpikes;                              // Right platform spikes F2
		[SerializeField]
		private GameObject _kunaiPrefab;                                        // Kunai prefab

		[Header("Fase 3")]
		[SerializeField]
		private Transform _f3LeftLimit;                                         // F3 left limit
		[SerializeField]
		private Transform _f3RightLimit;                                        // F3 right limit
		[SerializeField]
		private Transform[] _f3ReappearPoints;                                  // F3 reappear points
        [SerializeField]
		private Sprite _f3AttackSprite;                                         // F3 attack sprite

		[Header("Slash points")]
        [SerializeField]
		private Transform[] _slashPoints;                                       // Slash point 1

		[Header("Player")]
		[SerializeField]
		private Transform _player;                                              // Reference to player transform

		[Header("Params")]
		[SerializeField]
		private LayerMask _playerLayer;                                         // Player layer mask
		[SerializeField]
		private float _f1AlertTime = 2.0f;                                      // Time for the boss to be alert F1
		[SerializeField]
		private float _f1IdleTime = 5.0f;                                       // Time for the boss to be idle F1
		[SerializeField]
		private float _f2AlertTime = 2.0f;                                      // Time for the boss to be alert F2
		[SerializeField]
		private float _f2IdleTime = 7.0f;                                       // Time for the boss to be idle F2
		[SerializeField]
		private float _f2DistanceAttackTime = 10.0f;                            // Time for boss distance attack F2
		[SerializeField]
		private float _f2VerticalKunaiAttackDeltaTime = 0.5f;                   // Time between vertical kunai attacks F2
		[SerializeField]
		private int _f2SpikesReappearTimes = 5;                                 // Times before the spikes reappear F2
        [SerializeField]
		private float _f3AlertTime = 2.0f;                                      // Time for the boss to be alert F3
		[SerializeField]
		private float _f3IdleTime = 5.0f;                                       // Time for the boss to be idle F3
        [SerializeField]
		private float _f3AttackWaitTime = 1.5f;                                 // Wait time between attack fases
        [SerializeField]
		private float _bossVanishTime = 2.0f;                                   // Time it takes to vanish the boss
        [SerializeField]
		private int _attackLoops = 3;                                           // Number of attack loops going from one limit to the other
		[SerializeField]
		private int _attackDamage = 35;                                         // Boss attack damage
		[SerializeField]
		private float _attackForce = 400.0f;                                    // Back force applied to player when hit
		[SerializeField]
		private float _slashOverlapCircleRadius = 0.2f;                         // Slash overlap circle radius
		[SerializeField]
		private float _runSpeed = 12.5f;                                        // Boss run speed
		[SerializeField]
		private float _flySpeed = 20.0f;                                        // Boss fly speed
        [SerializeField]
		private int _maxHits = 6;                                               // Boss hits before death

        [Header("Sliders")]
		[SerializeField]
		private Slider _attackSlider;                                           // Boss attack slider
		[SerializeField]
		private Slider _healthSlider;                                           // Boss health slider

		[Header("Boss end")]
		[SerializeField]
		private BossEnd _bossEnd;                                               // Reference to BossEnd component

		[Header("Sounds")]
		[SerializeField]
		private AudioClip[] _attackAudioClips;                                  // Boss attack audio clips
		[SerializeField]
		private AudioClip _deathAudioClip;                                      // Boss death audio clip
		[SerializeField]
		private AudioClip[] _hurtAudioClips;                                    // Boss hurt audio clips
		[SerializeField]
		private AudioClip _laughAudioClip;                                      // Boss laugh audio clip

		private bool _facingRight = true;                                       // Current facing direction
		private bool _isHurt = false;                                           // Flag for boss hurt
        private int _hitsLeft = 0;                                              // Boss hits left
		private int _loopsLeft = 0;                                             // Loops left for the run slashing state
		private Vector3 _attackTargetPosition = Vector3.zero;                   // Attack target position               
        private EBossFases _bossFase = EBossFases.Fase1;                        // Holds the current boss fase
        private EEnemyStates _enemyState = EEnemyStates.Alert;                  // Holds the current boss state
		private Animator _animator = null;                                      // Reference to Animator component
		private SpriteRenderer _spriteRenderer = null;                          // Reference to SpriteRenderer component
		private CapsuleCollider2D _capsuleCollider2D = null;                    // Reference to CapsuleCollider2D component
		private AudioSource _audioSource = null;                                // Reference to AudioSource component

		private Coroutine _alertStateCoroutine = null;                          // Reference to alert state coroutine
		private Coroutine _idleStateCoroutine = null;                           // Reference to idle state coroutine
		private Coroutine _f1ToF2StateCoroutine = null;                         // Reference to F1 to F2 state coroutine
		private Coroutine _f2VerticalKunaiCoroutine = null;                     // reference to F2 vertical kunai attack coroutine
		private Coroutine _f2SpikesReappearCoroutine = null;                    // Reference to F2 spikes reappearing spikes
		private Coroutine _f2ToF3StateCoroutine = null;                         // Reference to F2 to F3 state coroutine
		private Coroutine _f3WaitBetweenAttackFasesCoroutine = null;            // Reference to F3 wait between attack fases
		private Coroutine _f3VanishReappearCoroutine = null;                    // Reference to F3 vanish and reappear boss

		private void Awake()
		{
			// Get Animator component
			_animator = GetComponent<Animator>();
			// Get SpriteRenderer component
			_spriteRenderer = GetComponent<SpriteRenderer>();
			// Get CapsuleCollider2D component
			_capsuleCollider2D = GetComponent<CapsuleCollider2D>();
			// Get AudioSource component
			_audioSource = GetComponent<AudioSource>();
		}

		// Use this for initialization
		private void Start()
		{
            // Set boss hots left
			_hitsLeft = _maxHits;
			// Set attack slider
			_attackSlider.normalizedValue = 0.0f;
			// Set health slider
			_healthSlider.normalizedValue = (float)_hitsLeft / (float)_maxHits;
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
				case EEnemyStates.Alert:
					AlertState();
					break;
				case EEnemyStates.Attack:
					AttackState();
					break;
				case EEnemyStates.Dead:
					DeadState();
					break;
				case EEnemyStates.DistanceAttack:
					DistanceAttackState();
					break;
				case EEnemyStates.Hurt:
					HurtState();
					break;
				case EEnemyStates.Idle:
					IdleState();
					break;
				case EEnemyStates.None:
					NoneState();
					break;
				case EEnemyStates.RunSlash:
					RunSlashingState();
					break;
				default:
					break;
			}
		}

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // If is not Fase 3, don't apply
			if (_bossFase != EBossFases.Fase3)
				return;

            // If is not attack state, don't apply
			if (_enemyState != EEnemyStates.Attack)
				return;

			// Boss hits the player
			if (_playerLayer == (_playerLayer | (1 << collision.gameObject.layer)))
			{
				// Apply damage
				collision.gameObject.GetComponent<CharacterHealth>().TakeDamage(_attackDamage);
                // Translate player upwards a litle bit
				collision.transform.Translate(0.0f, 1.0f, 0.0f, Space.Self);
				// Apply force
				float l_hForce = transform.position.x > collision.transform.position.x ? _attackForce : -_attackForce;
				float l_vForce = _attackForce;
				// Add force to push away the player
				collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(l_hForce, l_vForce));
			}
		}

		#region States
		public void AlertState()
        {
			switch (_bossFase)
			{
				case EBossFases.Fase1:
					{
						// Start alert state coroutine once
						if (_alertStateCoroutine == null)
							_alertStateCoroutine = StartCoroutine(AlertStateCoroutine());
					}
					break;
				case EBossFases.Fase2:
					{
						// Start alert state coroutine once
						if (_alertStateCoroutine == null)
							_alertStateCoroutine = StartCoroutine(AlertStateCoroutine());
					}
					break;
				case EBossFases.Fase3:
					{
						// Start alert state coroutine once
						if (_alertStateCoroutine == null)
							_alertStateCoroutine = StartCoroutine(AlertStateCoroutine());
					}
					break;
			}
		}

        public void AttackState()
        {
            // Get attack target position
            if (_attackTargetPosition == Vector3.zero)
            {
				float l_x =
                    _player.position.x > _f3RightLimit.position.x ?
                        _f3RightLimit.position.x :
                        (_player.position.x < _f3LeftLimit.position.x ? _f3LeftLimit.position.x : _player.position.x);
				float l_y = _f3LeftLimit.position.y;

				_attackTargetPosition = new Vector3(l_x, l_y, 0.0f);
                // Flip is needed
				if (transform.position.x < _attackTargetPosition.x && !_facingRight)
					FlipFacingDirection();
				// Flip is needed
				if (transform.position.x > _attackTargetPosition.x && _facingRight)
					FlipFacingDirection();
                // Play attack sound
				PlayRandomSound();
			}
			// Attack
			// Distance to move
			float l_dist = _flySpeed * Time.fixedDeltaTime;
			// Save boss previous position before moving
			Vector3 l_previousPosition = transform.position;
			// Move boss towards attack target position
			transform.position = Vector3.MoveTowards(transform.position, _attackTargetPosition, l_dist);
			// Check if boss has reached attack target position
			if (Mathf.Abs(transform.position.x - l_previousPosition.x) < 0.01f &&
				Mathf.Abs(transform.position.y - l_previousPosition.y) < 0.01f)
			{
				// Put boss on attack target position
				transform.position = _attackTargetPosition;
				// Put attack target position to zero
				_attackTargetPosition = Vector3.zero;
				// Change state
				_enemyState = EEnemyStates.None;
				// Loops left
				_loopsLeft--;
				// Attack bar
				_attackSlider.normalizedValue = (float)_loopsLeft / (float)_attackLoops;
				// Check loops left
				if (_loopsLeft == 0)
				{
					// Change state
					_enemyState = EEnemyStates.Idle;
					// Enable animator
					_animator.enabled = true;
					_animator.SetTrigger("ToIdle");
				}
			}
		}

		public void DeadState()
        {

        }

        public void DistanceAttackState()
        {
			// Start vertical kunai coroutine once
			if (_f2VerticalKunaiCoroutine == null)
				_f2VerticalKunaiCoroutine = StartCoroutine(F2VerticalKunaiCoroutine());
		}

        public void HurtState()
        {

        }

        public void IdleState()
        {
            switch(_bossFase)
            {
				case EBossFases.Fase1:
                    {
						// Start idle state coroutine once
						if (_idleStateCoroutine == null)
							_idleStateCoroutine = StartCoroutine(IdleStateCoroutine());
					}
					break;
				case EBossFases.Fase2:
					{
						// Start idle state coroutine once
						if (_idleStateCoroutine == null)
							_idleStateCoroutine = StartCoroutine(IdleStateCoroutine());
					}
					break;
				case EBossFases.Fase3:
					{
						// Start idle state coroutine once
						if (_idleStateCoroutine == null)
							_idleStateCoroutine = StartCoroutine(IdleStateCoroutine());
					}
					break;
            }
		}

		public void NoneState()
        {
			switch (_bossFase)
			{
				case EBossFases.Fase1:
					break;
				case EBossFases.Fase2:
					break;
				case EBossFases.Fase3:
					{
						// Start coroutine once
						if (_f3WaitBetweenAttackFasesCoroutine == null &&
							_f2ToF3StateCoroutine == null &&
							_f3VanishReappearCoroutine == null)
							_f3WaitBetweenAttackFasesCoroutine = StartCoroutine(F3WaitBetweenAttackFasesCoroutine(_f3AttackWaitTime));
					}
					break;
			}
		}

		public void RunSlashingState()
        {
			// Set speed direction
			float l_runSpeed = _facingRight ? _runSpeed : _runSpeed * (-1.0f);
			// Move enemy
			transform.Translate(l_runSpeed * Time.fixedDeltaTime, 0.0f, 0.0f, Space.Self);
			// Whether the enemy is facing right or not...
			if (_facingRight)
			{
				// If right limit is reached
				if (transform.position.x > _f1RightLimit.position.x)
				{
					// Put enemy in right limit position
					transform.position = new Vector3(_f1RightLimit.position.x, transform.position.y, transform.position.z);
					// Set remaining lopps
					_loopsLeft--;
					// Set attack slider value
					_attackSlider.normalizedValue = (float)_loopsLeft / (float)_attackLoops;
					// If remaining loops are 0
					if (_loopsLeft == 0)
					{
						// Change state
						_enemyState = EEnemyStates.Idle;
						// Change animator state
						_animator.SetTrigger("ToIdle");
						// Cancel sound invoking
						CancelInvoke("PlayRandomSound");

						return;
					}
					// Manage flip
					FlipFacingDirection();
				}
			}
			else
			{
				// If left limit is reached
				if (transform.position.x < _f1LeftLimit.position.x)
				{
					// Put enemy in left limit position
					transform.position = new Vector3(_f1LeftLimit.position.x, transform.position.y, transform.position.z);
					// Manage flip
					FlipFacingDirection();
				}
			}
			// Invoke attack sounds
			if (!IsInvoking("PlayRandomSound"))
				InvokeRepeating("PlayRandomSound", 0.1f, 1.5f);
		}
		#endregion

		#region Coroutines
		/// <summary>
		///     Alert state coroutine waits for a period of time
		/// </summary>
		/// <returns>The reference to the coroutine</returns>
		private IEnumerator AlertStateCoroutine()
		{
			// Before waiting
			switch (_bossFase)
			{
				case EBossFases.Fase1:
					break;
				case EBossFases.Fase2:
					{
                        // Enable spikes
						_f2LeftSpikes.SetActive(true);
						_f2RightSpikes.SetActive(true);
						_f2LeftPlatformSpikes.SetActive(true);
						_f2RightPlatformSpikes.SetActive(true);
                        // Disable platform
						_f2FirstPlatform.SetActive(false);
					}
					break;
				case EBossFases.Fase3:
					break;
				default:
					break;
			}
			// Set attack slider value to 0
			_attackSlider.normalizedValue = 0.0f;
			// stay sometime in alert state before changing to run slashing
            // Recharge attack bar
			float l_waitedTime = 0.0f;
			float l_alertTime = _bossFase == EBossFases.Fase1 ? _f1AlertTime : (_bossFase == EBossFases.Fase2 ? _f2AlertTime : _f3AlertTime);
			while (l_waitedTime < l_alertTime)
            {
				yield return new WaitForSeconds(0.1f);

				l_waitedTime += 0.1f;

				if (l_waitedTime > l_alertTime)
					l_waitedTime = l_alertTime;

				_attackSlider.normalizedValue = l_waitedTime / l_alertTime;
            }
			//
            switch(_bossFase)
            {
				case EBossFases.Fase1:
                    {
						// Manage flip
						FlipFacingDirection();
						// Set run slashing loops left
						_loopsLeft = _attackLoops;
						// Change state and animator
						_enemyState = EEnemyStates.RunSlash;
						_animator.SetTrigger("ToRunSlashing");
					}
					break;
				case EBossFases.Fase2:
                    {
                        // Change state
						_enemyState = EEnemyStates.DistanceAttack;
					}
					break;
				case EBossFases.Fase3:
                    {
						// Set attack loops left
						_loopsLeft = _attackLoops;
						// Change state
						_enemyState = EEnemyStates.Attack;
					}
					break;
				default:
					break;
			}
			// Clear coroutine reference
			_alertStateCoroutine = null;
		}

		/// <summary>
		///     Idle state F1 coroutine waits for a period of time
		/// </summary>
		/// <returns>The reference to the coroutine</returns>
		private IEnumerator IdleStateCoroutine()
		{
			// Before waiting
			switch (_bossFase)
			{
				case EBossFases.Fase1:
					break;
				case EBossFases.Fase2:
					{
                        // Disable spikes
						_f2LeftSpikes.SetActive(false);
						_f2RightSpikes.SetActive(false);
						_f2LeftPlatformSpikes.SetActive(false);
						_f2RightPlatformSpikes.SetActive(false);
                        // Enable platform
                        _f2FirstPlatform.SetActive(true);
					}
					break;
				case EBossFases.Fase3:
					break;
				default:
					break;
			}
			// Set attack slider value to 0
			_attackSlider.normalizedValue = 0.0f;
			// stay sometime in idle state before changing to run slashing
			// Recharge attack bar
			float l_waitedTime = 0.0f;
			float l_idleTime = _bossFase == EBossFases.Fase1 ? _f1IdleTime : (_bossFase == EBossFases.Fase2 ? _f2IdleTime : _f3IdleTime);
			while (l_waitedTime < l_idleTime)
			{
				yield return new WaitForSeconds(0.1f);

				l_waitedTime += 0.1f;

				if (l_waitedTime > l_idleTime)
					l_waitedTime = l_idleTime;

				_attackSlider.normalizedValue = l_waitedTime / l_idleTime;
			}
			
			//
			switch (_bossFase)
			{
				case EBossFases.Fase1:
					{
						// Manage flip
						FlipFacingDirection();
						// Set run slashing loops left
						_loopsLeft = _attackLoops;
						// Change state and animator
						_enemyState = EEnemyStates.RunSlash;
						_animator.SetTrigger("ToRunSlashing");
					}
					break;
				case EBossFases.Fase2:
					{
                        // Change state
						_enemyState = EEnemyStates.None;
                        // Start spikes reappearing
						_f2SpikesReappearCoroutine = StartCoroutine(F2SpikesReappearCoroutine());
					}
					break;
				case EBossFases.Fase3:
					{
						// Change state
						_enemyState = EEnemyStates.None;
						// Start spikes reappearing
						_f3VanishReappearCoroutine = StartCoroutine(F3VanishReappearCoroutine());
					}
					break;
				default:
					break;
			}
			// Clear coroutine reference
			_idleStateCoroutine = null;
		}

		/// <summary>
		///     Changes from Fase 1 to Fase 2
		/// </summary>
		/// <returns>The reference to the coroutine</returns>
		private IEnumerator F1ToF2StateCoroutine()
        {
			// Make boss vanish
			yield return StartCoroutine(MakeBossVanish(_bossVanishTime));
			// Move boss gameObject to F2 position
			transform.position = new Vector3(_f2PlatformPosition.position.x, _f2PlatformPosition.position.y, transform.position.z);
			// Wait for some time
			yield return new WaitForSeconds(2.0f);
            // Make platforms appear
            for (int i = 0; i < _f2OtherPlatforms.Length; i++)
            {
				_f2OtherPlatforms[i].SetActive(true);
			}
            // Make spikes appear
			_f2LeftSpikes.SetActive(true);
			_f2RightSpikes.SetActive(true);
			_f2LeftPlatformSpikes.SetActive(true);
			_f2RightPlatformSpikes.SetActive(true);
			// Make boss appear
			yield return StartCoroutine(MakeBossReappear(_bossVanishTime));
			// Change state
			_enemyState = EEnemyStates.Alert;
            // Clear coroutine reference
			_f1ToF2StateCoroutine = null;
		}

		/// <summary>
		///     F2 Vertical attack coroutine
		/// </summary>
		/// <returns>The reference to the coroutine</returns>
		private IEnumerator F2VerticalKunaiCoroutine()
		{
			float l_waitedTime = 0.0f;

            while (l_waitedTime < _f2DistanceAttackTime)
            {
				GameObject l_verticalKunai = Instantiate(_kunaiPrefab, GameManager.Instance.PrefabContainer);

				l_verticalKunai.transform.position = new Vector3(_player.position.x, _f2KunaiSpawnerPosition.position.y, 0.0f);
				l_verticalKunai.transform.Rotate(0.0f, 0.0f, -90.0f, Space.Self);

				l_waitedTime += _f2VerticalKunaiAttackDeltaTime;

				_attackSlider.normalizedValue = Mathf.Clamp01(1.0f - l_waitedTime / _f2DistanceAttackTime);

				yield return new WaitForSeconds(_f2VerticalKunaiAttackDeltaTime);
            }
			// Change state
			_enemyState = EEnemyStates.Idle;
			// Change animator
			_animator.SetTrigger("ToIdle");
			// Clear coroutine reference
			_f2VerticalKunaiCoroutine = null;
		}

		/// <summary>
		///     F2 spikes reappearing coroutine
		/// </summary>
		/// <returns>The reference to the coroutine</returns>
		private IEnumerator F2SpikesReappearCoroutine()
		{
			// Vanish boss
			_spriteRenderer.enabled = false;
			_capsuleCollider2D.enabled = false;
			_attackSlider.normalizedValue = 0.0f;
            //_attackSlider.gameObject.SetActive(false);
			//_healthSlider.gameObject.SetActive(false);
            // Disable spikes colliders
			ManageSpikesCollidersTriggers(false);
			// Reappearing process
			float l_iter = 0;
            // Process
			while (l_iter < _f2SpikesReappearTimes)
			{
				// Enable spikes
				ManageSpikes(true);

				yield return new WaitForSeconds(0.75f);

				// Disable spikes
				ManageSpikes(false);

				yield return new WaitForSeconds(0.25f);

				l_iter++;
			}
			// Enable spikes colliders
			ManageSpikesCollidersTriggers(true);
			// Enable spikes
			ManageSpikes(true);
			// Disable platform
			_f2FirstPlatform.SetActive(false);
			// Wait some time
			yield return new WaitForSeconds(2.0f);
			// Disable middle spike
			_f2MiddleSpikes.SetActive(false);
			// Reappear boss
			_spriteRenderer.enabled = true;
			_capsuleCollider2D.enabled = true;
			//_attackSlider.gameObject.SetActive(true);
			//_healthSlider.gameObject.SetActive(true);
			// Change state
			_enemyState = EEnemyStates.DistanceAttack;
			// Change animator
			AnimatorStateInfo l_animatorStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
			if (!l_animatorStateInfo.IsName("SamuraiHeavyBoss_Alert"))
				_animator.SetTrigger("ToAlert");
			// Recharge attack bar
			_attackSlider.normalizedValue = 1.0f;
			// Clear coroutine reference
			_f2SpikesReappearCoroutine = null;
		}

		/// <summary>
		///     Changes from Fase 2 to Fase 3
		/// </summary>
		/// <returns>The reference to the coroutine</returns>
		private IEnumerator F2ToF3StateCoroutine()
		{
			// Make boss vanish
			yield return StartCoroutine(MakeBossVanish(_bossVanishTime));
			// Disable spikes colliders
			ManageSpikesCollidersTriggers(false);
			// Disable platform
			_f2FirstPlatform.SetActive(false);
			// Reappearing process
			float l_iter = 0;
			// Process
			while (l_iter < _f2SpikesReappearTimes)
			{
				// Enable spikes
				ManageSpikes(true);

				yield return new WaitForSeconds(0.75f);

				// Disable spikes
				ManageSpikes(false);

				yield return new WaitForSeconds(0.25f);

				l_iter++;
			}
			// Enable spikes colliders
			ManageSpikesCollidersTriggers(true);
			// Enable spikes
			ManageSpikes(true);
			// Wait some time
			yield return new WaitForSeconds(2.0f);
			// Disable spikes
			ManageSpikes(false);
			// Disable other platforms
			for (int i = 0; i < _f2OtherPlatforms.Length; i++)
            {
				_f2OtherPlatforms[i].SetActive(false);
            }
            // Wait some time
            yield return new WaitForSeconds(2.0f);
			// Rebind and disable animator
			_animator.Rebind();
			_animator.enabled = false;
			// Change to attack sprite
			_spriteRenderer.sprite = _f3AttackSprite;
			// Choose random reappear position
			transform.position = RandomReappearPosition();
			// Flip is needed
			if (transform.position.x > 0.0f)
			{
				FlipFacingDirection();
			}
			// Make boss reappear
			yield return StartCoroutine(MakeBossReappear(_bossVanishTime));
			// Change state
			_enemyState = EEnemyStates.Alert;
			// Clear coroutine reference
			_f2ToF3StateCoroutine = null;
		}

		/// <summary>
		///     Waits between attack fases in F3
		/// </summary>
		/// <returns>The reference to the coroutine</returns>
		private IEnumerator F3WaitBetweenAttackFasesCoroutine(float waitTime)
        {
            // Enable animator
			_animator.enabled = true;
            // Wait some time
			yield return new WaitForSeconds(waitTime);
            // Rebind and disable animator
			_animator.Rebind();
			_animator.enabled = false;
            // Change sprite
			_spriteRenderer.sprite = _f3AttackSprite;
			// Change state
			_enemyState = EEnemyStates.Attack;
			// Clear coroutine reference
			_f3WaitBetweenAttackFasesCoroutine = null;
		}

		/// <summary>
		///     Makes boss vanish and reappear in F3
		/// </summary>
		/// <returns>The reference to the coroutine</returns>
		private IEnumerator F3VanishReappearCoroutine()
        {
			// Change animator state
			_animator.SetTrigger("ToAlert");
			// Make boss vanish
			yield return StartCoroutine(MakeBossVanish(_bossVanishTime));
			// Rebind and disable animator
			_animator.Rebind();
			_animator.enabled = false;
			// Change to attack sprite
			_spriteRenderer.sprite = _f3AttackSprite;
			// Choose random reappear position
			transform.position = RandomReappearPosition();
			// Flip is needed
			if (transform.position.x > 0.0f && _facingRight)
			{
				FlipFacingDirection();
			}
            if (transform.position.x < 0.0f && !_facingRight)
			{
				FlipFacingDirection();
			}
			// Make boss reappear
			yield return StartCoroutine(MakeBossReappear(_bossVanishTime));
			// Change state
			_enemyState = EEnemyStates.Alert;
			// Clear coroutine reference
			_f3VanishReappearCoroutine = null;
		}

		/// <summary>
		///     Makes the boss vanish/fade out
		/// </summary>
		/// <param name="vanishTime">Vanish time duration</param>
		/// <returns>The reference to the coroutine</returns>
		private IEnumerator MakeBossVanish(float vanishTime)
        {
			// Make boss vanish
			float l_waitedTime = 0.0f;
			Color l_color = _spriteRenderer.color;
			// Disable boss bars
			_attackSlider.normalizedValue = 0.0f;
			//_attackSlider.gameObject.SetActive(false);
			//_healthSlider.gameObject.SetActive(false);
			// Play laugh sound
			_audioSource.PlayOneShot(_laughAudioClip);
			// Vanishing process
			while (l_waitedTime < vanishTime)
			{
				yield return new WaitForSeconds(0.1f);

				l_waitedTime += 0.1f;

				l_color.a = Mathf.Clamp01(1.0f - l_waitedTime / vanishTime);

				_spriteRenderer.color = l_color;
			}
			// Set alpha to 0
			l_color.a = 0.0f;
			_spriteRenderer.color = l_color;
		}

		/// <summary>
		///     Makes the boss reappear/fade in
		/// </summary>
		/// <param name="vanishTime">Reappear time duration</param>
		/// <returns>The reference to the coroutine</returns>
		private IEnumerator MakeBossReappear(float reappearTime)
        {
			// Make boss appear
			float l_waitedTime = 0.0f;
			Color l_color = _spriteRenderer.color;
			// Appear process
			while (l_waitedTime < reappearTime)
			{
				yield return new WaitForSeconds(0.1f);

				l_waitedTime += 0.1f;

				l_color.a = Mathf.Clamp01(l_waitedTime / reappearTime);

				_spriteRenderer.color = l_color;
			}
			// Set alpha to 1
			l_color.a = 1.0f;
			_spriteRenderer.color = l_color;
			// Enable boss bars
			//_attackSlider.gameObject.SetActive(true);
			//_healthSlider.gameObject.SetActive(true);
		}
		#endregion

		#region Private methods
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
			// Flip slash points position
			for (int i = 0; i < _slashPoints.Length; i++)
            {
				_slashPoints[i].localPosition = new Vector3(-_slashPoints[i].localPosition.x, _slashPoints[i].localPosition.y, _slashPoints[i].localPosition.z);
			}
		}

        /// <summary>
        ///     Enables or disables spikes
        /// </summary>
        /// <param name="enable">Enable/Disable</param>
        private void ManageSpikes(bool enable)
        {
			_f2MiddleSpikes.SetActive(enable);
			_f2LeftSpikes.SetActive(enable);
			_f2RightSpikes.SetActive(enable);
			_f2LeftPlatformSpikes.SetActive(enable);
			_f2RightPlatformSpikes.SetActive(enable);
		}

        /// <summary>
        ///     Manage spikes colliders and triggers
        /// </summary>
        /// <param name="enable">Enable/Disable</param>
        private void ManageSpikesCollidersTriggers(bool enable)
        {
			_f2MiddleSpikes.GetComponent<TilemapCollider2D>().enabled = enable;
			_f2LeftSpikes.GetComponent<TilemapCollider2D>().enabled = enable;
			_f2RightSpikes.GetComponent<TilemapCollider2D>().enabled = enable;
			_f2LeftPlatformSpikes.GetComponent<TilemapCollider2D>().enabled = enable;
			_f2RightPlatformSpikes.GetComponent<TilemapCollider2D>().enabled = enable;
			_f2MiddleSpikes.GetComponent<CompositeCollider2D>().isTrigger = !enable;
			_f2LeftSpikes.GetComponent<CompositeCollider2D>().isTrigger = !enable;
			_f2RightSpikes.GetComponent<CompositeCollider2D>().isTrigger = !enable;
			_f2LeftPlatformSpikes.GetComponent<CompositeCollider2D>().isTrigger = !enable;
			_f2RightPlatformSpikes.GetComponent<CompositeCollider2D>().isTrigger = !enable;
		}

        /// <summary>
        ///     Returns a random position from array of positions
        /// </summary>
        /// <returns>Position</returns>
        private Vector3 RandomReappearPosition()
        {
			int l_random = (int)(_f3ReappearPoints.Length * Random.value);

			if (l_random == _f3ReappearPoints.Length)
				l_random--;

			return new Vector3(_f3ReappearPoints[l_random].position.x, _f3ReappearPoints[l_random].position.y, _f3ReappearPoints[l_random].position.z);
        }

        /// <summary>
        ///     Plays a random sound depending on the enemy state
        /// </summary>
        private void PlayRandomSound()
        {
			AudioClip[] audioClips = null;

            switch(_enemyState)
            {
				case EEnemyStates.Hurt:
					audioClips = _hurtAudioClips;
					break;
				case EEnemyStates.Attack:
				case EEnemyStates.RunSlash:
					audioClips = _attackAudioClips;
					break;
				default:
					break;
            }

			if (audioClips == null)
				return;

			float l_value = Random.value * 1000.0f;

			if (l_value < 333.0f)
			{
				_audioSource.PlayOneShot(audioClips[0]);
			}

			if (l_value >= 333.0f && l_value < 666.0f)
			{
				_audioSource.PlayOneShot(audioClips[1]);
			}

			if (l_value >= 666.0f)
			{
				_audioSource.PlayOneShot(audioClips[2]);
			}
		}
        #endregion

        #region Public methods
        /// <summary>
        ///     Boss takes hit
        /// </summary>
        public void TakeDamage(int damage)
		{
			// Only hurt boss when in idle state
			if (_enemyState != EEnemyStates.Idle)
				return;

			// Take damage
			_hitsLeft--;

			// Set health slider
			_healthSlider.normalizedValue = (float)_hitsLeft / (float)_maxHits;

			// Cancel coroutine
			if (_idleStateCoroutine != null)
			{
				StopCoroutine(_idleStateCoroutine);

				_idleStateCoroutine = null;
			}

			// Manage state depending on remaining health
			if (_hitsLeft == 0)
			{
				// Attack bar
				_attackSlider.normalizedValue = 0.0f;
				// Disable collider
				_capsuleCollider2D.enabled = false;
				// Change state
				_enemyState = EEnemyStates.Dead;
				// Change animator state
				_animator.SetTrigger("IsDead");
				// Play dead sound
				_audioSource.PlayOneShot(_deathAudioClip);
			}
			else
			{
				// Set flag
				_isHurt = true;
				// Change state
				_enemyState = EEnemyStates.Hurt;
				// Change animator state
				_animator.SetBool("IsHurt", true);
				// Play damage sound
				PlayRandomSound();
			}
		}
		#endregion

		#region Animation events
		/// <summary>
		///     Gets called when enemy dead animation ends
		/// </summary>
		public void BossDeadEndAnimationEvent()
		{
			_bossEnd.BossEndProcess();
		}

		/// <summary>
		///     Gets called when boss hurt animations ends
		/// </summary>
		public void BossHurtEndAnimationEvent()
		{
			// Set flag
			_isHurt = false;
			// Manage flip
			//FlipFacingDirection();
			// Set run slashing loops left
			//_loopsLeft = _attackLoops;
			// Change state
			_enemyState = EEnemyStates.Alert;
            // Change boss fase
			if (_hitsLeft > 4)
            {

			}
            if (_hitsLeft <= 4 && _hitsLeft > 2)
            {
				if (_bossFase != EBossFases.Fase2)
                {
                    // Change fase
					_bossFase = EBossFases.Fase2;
                    // Change state
					_enemyState = EEnemyStates.None;
                    // Start fase 1 to fase 2 coroutine
					_f1ToF2StateCoroutine = StartCoroutine(F1ToF2StateCoroutine());
                }
                else
                {
					// Change state
					_enemyState = EEnemyStates.None;
					// Start reappearing spikes
					_f2SpikesReappearCoroutine = StartCoroutine(F2SpikesReappearCoroutine());
				}
            }
            if (_hitsLeft <= 2 && _hitsLeft > 0)
            {
				if (_bossFase != EBossFases.Fase3)
				{
					// Change fase
					_bossFase = EBossFases.Fase3;
					// Change state
					_enemyState = EEnemyStates.None;
					// Start fase 2 to fase 3 coroutine
					_f2ToF3StateCoroutine = StartCoroutine(F2ToF3StateCoroutine());
				}
				else
				{
					// Change state
					_enemyState = EEnemyStates.None;
					// Start reappearing spikes
					_f3VanishReappearCoroutine = StartCoroutine(F3VanishReappearCoroutine());
				}
			}
			// Change animator state
			_animator.SetBool("IsHurt", false);
		}

		/// <summary>
		///     Gets called in the run slashing keyframe animation when it should damage the player
		/// </summary>
		public void BossSlashAnimationEvent()
		{
			// Go through slash points
			for (int i = 0; i < _slashPoints.Length; i++)
			{
				// Check if player has been hit
				Collider2D l_collider = Physics2D.OverlapCircle(_slashPoints[i].position, _slashOverlapCircleRadius, _playerLayer);
				// If player has been hit...
				if (l_collider != null)
				{
					// Apply damage
					l_collider.GetComponent<CharacterHealth>().TakeDamage(_attackDamage);
					// Translate player upwards a litle bit
					l_collider.transform.Translate(0.0f, 1.0f, 0.0f, Space.Self);
                    // Apply force
					float l_hForce = transform.position.x > l_collider.transform.position.x ? _attackForce : -_attackForce;
					float l_vForce = _attackForce;
                    // Add force to push away the player
					l_collider.GetComponent<Rigidbody2D>().AddForce(new Vector2(l_hForce, l_vForce));
					// If player is hit once the go back
					return;
				}
				// Play attack sound
				//_audioSource.PlayOneShot(_audioClipAttack);
			}
		}
		#endregion
	}
}
