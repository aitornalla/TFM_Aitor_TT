using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Player
{
	public sealed class CharacterParams : MonoBehaviour
	{
        #region Constants
        public const float GroundedRadius = 0.2f;       // Radius of the overlap circle to determine if grounded
        public const float CeilingRadius = 0.2f;        // Radius of the overlap circle to determine if the player can stand up
        #endregion


        #region Parameters
        [Header("Forces")]
        [SerializeField]
		private float _jumpForce = 525.0f;              // Amount of force added when the player jumps
		[SerializeField]
		private float _slideForce = 125.0f;             // Amount of force added when the player slides

        [Header("Speeds")]
        [SerializeField]
        private float _runSpeed = 40.0f;                // Player run speed
        [SerializeField]
        private float _glideHorizontalSpeed = 5.0f;     // Target gliding horizontal speed
        [SerializeField]
		private float _glideVerticalSpeedUp = 7.0f;     // Target gliding vertical speed up
        [SerializeField]
        private float _glideVerticalSpeedDown = -0.5f;  // Target gliding vertical speed down

        [Header("Movement Smoothing")]
        [SerializeField] [Range(0, 0.3f)]
		private float _movementSmoothing = 0.05f;       // How much to smooth out the movement
        [SerializeField] [Range(0, 0.3f)]
        private float _glideMovementSmoothing = 0.1f;   // How much to smooth out the movement

        [Space]
        [SerializeField]
		private bool _airControl = false;               // Whether or not a player can steer while jumping

        [Space]
        [SerializeField]
		private LayerMask _groundLayer;                 // A mask determining what is ground to the character
        #endregion

        #region Properties
        public float JumpForce
        {
            get
            {
                return _jumpForce;
            }
        }

		public float SlideForce
        {
            get
            {
                return _slideForce;
            }
        }

        public float RunSpeed
        {
            get
            {
                return _runSpeed;
            }
        }

        public float GlideHorizontalSpeed
        {
            get
            {
                return _glideHorizontalSpeed;
            }
        }

        public float GlideVerticalSpeedUp
        {
            get
            {
                return _glideVerticalSpeedUp;
            }
        }

        public float GlideVerticalSpeedDown
        {
            get
            {
                return _glideVerticalSpeedDown;
            }
        }

		public float MovementSmoothing
        {
            get
            {
                return _movementSmoothing;
            }
        }

        public float GlideMovementSmoothing
        {
            get
            {
                return _glideMovementSmoothing;
            }
        }

        public bool AirControl
        {
            get
            {
                return _airControl;
            }
        }

		public LayerMask GroundLayer
        {
            get
            {
                return _groundLayer;
            }
        }
		#endregion
	}
}
