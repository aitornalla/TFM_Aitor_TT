using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Player
{
    public sealed class CharacterFlags : MonoBehaviour
    {
        #region Flags
        private bool _grounded;							// Whether or not the player is grounded
        private bool _facingRight = true;               // For determining which way the player is currently facing
        private bool _wasSliding = false;               // Was the player sliding in the previous frame?
        private bool _hasDoubleJumped = false;          // Flag for double jumping
        private bool _wasGliding = false;               // Is the player gliding?
        private bool _isAttacking = false;              // Is the player attacking?
        private bool _isThrowing = false;               // Is the player throwing a kunai?
        #endregion

        #region Properties
        public bool IsGrounded
        {
            get
            {
                return _grounded;
            }

            set
            {
                _grounded = value;
            }
        }

        public bool IsFacingRight
        {
            get
            {
                return _facingRight;
            }

            set
            {
                _facingRight = value;
            }
        }

        public bool WasSliding
        {
            get
            {
                return _wasSliding;
            }

            set
            {
                _wasSliding = value;
            }
        }

        public bool HasDoubleJumped
        {
            get
            {
                return _hasDoubleJumped;
            }

            set
            {
                _hasDoubleJumped = value;
            }
        }

        public bool WasGliding
        {
            get
            {
                return _wasGliding;
            }

            set
            {
                _wasGliding = value;
            }
        }

        public bool IsAttacking
        {
            get
            {
                return _isAttacking;
            }

            set
            {
                _isAttacking = value;
            }
        }

        public bool IsThrowing
        {
            get
            {
                return _isThrowing;
            }

            set
            {
                _isThrowing = value;
            }
        }
        #endregion
    }
}
