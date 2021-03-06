﻿using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.GameManagerController;
using UnityEngine;

namespace Assets.Scripts.AnimatedSpringboardController
{
	public sealed class AnimatedSpringboard : MonoBehaviour
	{
		[SerializeField]
		private LayerMask _playerLayer;                                         // Player layer to check conditions
		[SerializeField]
		private float _springForce = 1000.0f;                                   // Spring force
		[SerializeField]
		private float _angleForce = 90.0f;                                      // Angle used to apply the force (in degrees)
        [SerializeField]
        private bool _totalVelocityCancelation = false;                         // Flag for total velocity cancellation before applying spring force
        [SerializeField]
        private AudioClip _jumpSound = null;                                    // Jump sound effect

        private Animator _animator;                                             // Animator component
        private Transform _arrowChild;                                          // Child gameObject arrow
        private const float ArrowRotation_0 = 45.0f;                            // Initial sprite angle
        private AudioSource _audioSource = null;                                // Reference to AudioSource component

        private void Awake()
        {
            // Get animator component
			_animator = gameObject.GetComponent<Animator>();
            // Get AudioSource component
            _audioSource = GetComponent<AudioSource>();
            // Assign AudioMixerGroup for AudioSource component
            _audioSource.outputAudioMixerGroup =
                GameManager.Instance.AudioMixerController.MainAudioMixerGroups[0];
            // Get arrow child
            _arrowChild = gameObject.transform.GetChild(0);
        }

        // Use this for initialization
        private void Start()
		{
            // Rotates arrow sprite to match spring force direction
            // Counters the rotation of parent gameObject
            _arrowChild.Rotate(0.0f, 0.0f, _angleForce + ArrowRotation_0 - transform.rotation.eulerAngles.z);
		}

		// Update is called once per frame
		//private void Update()
		//{
        //
		//}

        private void OnTriggerEnter2D(Collider2D collision)
        {
			// If player jumps on the springboard ...
			if (_playerLayer == (_playerLayer | (1 << collision.gameObject.layer)))
			{
				// Get RigidBody2D component
				Rigidbody2D l_rigidbody2D = collision.GetComponent<Rigidbody2D>();

                if (l_rigidbody2D != null)
                {
                    if (_totalVelocityCancelation)
                    {
                        // Set x/y velocity components to zero before applying spring force
                        l_rigidbody2D.velocity = Vector2.zero;
                    }
                    else
                    {
                        // Cancel velocity component in the same direction as spring force
                        l_rigidbody2D.velocity = CancelVelocityOnForceDirection(l_rigidbody2D.velocity);
                    }

					// Add force
					float l_anlgeRad = Mathf.Deg2Rad * _angleForce;
					Vector2 l_force = new Vector2(_springForce * Mathf.Cos(l_anlgeRad), _springForce * Mathf.Sin(l_anlgeRad));
					l_rigidbody2D.AddForce(l_force);

                    // Trigger animated springboard animation
                    if (_animator != null)
                    {
						_animator.SetTrigger("Activate");
                    }

                    // Play jump sound effect
                    _audioSource.PlayOneShot(_jumpSound);
				}
			}
        }

        /// <summary>
        ///     Cancels velocity component in the same direction as the spring force
        /// </summary>
        /// <param name="rbVelocity">Velocity vector</param>
        /// <returns>Velocity vector transformed with canceled component</returns>
        private Vector2 CancelVelocityOnForceDirection(Vector2 rbVelocity)
        {
            Vector2 l_canceledVel = Vector2.zero;
            float l_alpha = 0.0f; //(_angleForce - 90.0f) * Mathf.Deg2Rad;
            float l_beta_raw = Mathf.Atan2(Mathf.Abs(rbVelocity.y), Mathf.Abs(rbVelocity.x));
            float l_beta = 0.0f;

            // Set beta angle correctly from raw beta angle
            if (Mathf.Sign(rbVelocity.x) == 1.0f && Mathf.Sign(rbVelocity.y) == 1.0f)
            {
                l_beta = l_beta_raw;
            }

            if (Mathf.Sign(rbVelocity.x) == -1.0f && Mathf.Sign(rbVelocity.y) == 1.0f)
            {
                l_beta = Mathf.PI - l_beta_raw;
            }

            if (Mathf.Sign(rbVelocity.x) == -1.0f && Mathf.Sign(rbVelocity.y) == -1.0f)
            {
                l_beta = Mathf.PI + l_beta_raw;
            }

            if (Mathf.Sign(rbVelocity.x) == 1.0f && Mathf.Sign(rbVelocity.y) == -1.0f)
            {
                l_beta = 2.0f * Mathf.PI - l_beta_raw;
            }

            ///

            if (Mathf.Sign(Mathf.Cos(_angleForce)) == -1.0f && Mathf.Sign(Mathf.Cos(l_beta)) == 1.0f)
            {
                l_alpha = (_angleForce - 90.0f) * Mathf.Deg2Rad;
            }

            if (Mathf.Sign(Mathf.Cos(_angleForce)) == -1.0f && Mathf.Sign(Mathf.Cos(l_beta)) == -1.0f)
            {
                l_alpha = (_angleForce + 90.0f) * Mathf.Deg2Rad;
            }

            if (Mathf.Sign(Mathf.Cos(_angleForce)) == 1.0f && Mathf.Sign(Mathf.Cos(l_beta)) == 1.0f)
            {
                l_alpha = (_angleForce + 270.0f) * Mathf.Deg2Rad;
            }

            if (Mathf.Sign(Mathf.Cos(_angleForce)) == 1.0f && Mathf.Sign(Mathf.Cos(l_beta)) == -1.0f)
            {
                l_alpha = (_angleForce + 90.0f) * Mathf.Deg2Rad;
            }

            ///

            Vector2 l_vF = new Vector2(rbVelocity.magnitude * Mathf.Cos(l_alpha + l_beta), 0.0f);

            l_canceledVel = new Vector2(l_vF.magnitude * Mathf.Cos(l_alpha), l_vF.magnitude * Mathf.Sin(l_alpha));

            return l_canceledVel;
        }
    }
}
