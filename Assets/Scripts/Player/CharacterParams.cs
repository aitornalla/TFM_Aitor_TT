using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Player
{
	public sealed class CharacterParams : MonoBehaviour
	{
        [Header("Parameters")]
        #region Parameters
        [SerializeField]
		private float _jumpForce = 525.0f;              // Amount of force added when the player jumps
		[SerializeField]
		private float _slideForce = 125.0f;             // Amount of force added when the player slides
		[SerializeField]
		private float _glideSpeed = -0.5f;              // Target gliding speed
		[SerializeField]
		private float _runSpeed = 40.0f;                // Player run speed
		[SerializeField] [Range(0, 0.3f)]
		private float _movementSmoothing = 0.05f;       // How much to smooth out the movement
		[SerializeField]
		private bool _airControl = false;               // Whether or not a player can steer while jumping
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

		public float GlideSpeed
        {
            get
            {
                return _glideSpeed;
            }
        }

		public float RunSpeed
        {
            get
            {
                return _runSpeed;
            }
        }

		public float MovementSmoothing
        {
            get
            {
                return _movementSmoothing;
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
