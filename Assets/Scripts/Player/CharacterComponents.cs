using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Player
{
    public sealed class CharacterComponents : MonoBehaviour
    {
        [Header("Components")]
        #region Components
        [SerializeField]
        private CharacterController2D _characterController2D;                   // CharacterController2D component
        [SerializeField]
        private CharacterInputController2D _characterInputController2D;         // CharacterInputController2D component
        [SerializeField]
	    private CharacterParams _characterParams;                               // Component to hold character parameters
        [SerializeField]
        private CharacterFlags _characterFlags;                                 // Component to hold character flags
        [SerializeField]
        private CharacterEvents _characterEvents;                               // Component to hold character events
        [SerializeField]
        private CharacterAudio _characterAudio;                                 // Component to hold character audio
        [SerializeField]
	    private Rigidbody2D _rigidbody2D;                                       // Rigidbody2D component of the gameObject
	    [SerializeField]
	    private CapsuleCollider2D _mainCapCollider2D;                           // Not-sliding CapsuleCollider2D component of the gameObject
	    [SerializeField]
	    private CapsuleCollider2D _slideCapCollider2D;                          // Sliding CapsuleCollider2D component of the gameObject
	    [SerializeField]
	    private CapsuleCollider2D _glideCapCollider2D;                          // Gliding CapsuleCollider2D component of the gameObject
	    [SerializeField]
	    private CapsuleCollider2D _attackCapCollider2D;                         // Attack CapsuleCollider2D component of the gameObject
        [SerializeField]
        private CapsuleCollider2D _throwCapCollider2D;                          // Throw CapsuleCollider2D component of the gameObject
        [SerializeField]
        private Animator _animator;                                             // Animator component
        [SerializeField]
	    private SpriteRenderer _spriteRenderer;                                 // SpriteRenderer component of the gameObject
        [SerializeField]
        private AudioSource _audioSource;                                       // AudioSource component
        #endregion

        [Header("Transform References")]
        #region Transform References
        [SerializeField]
        private Transform _groundCheck;                                         // A position marking where to check if the player is grounded
        [SerializeField]
        private Transform _ceilingCheck;                                        // A position marking where to check for ceilings
        [SerializeField]
        private Transform _attackCheck;                                         // A position marking where to check for attack
        [SerializeField]
        private Transform _throwCheck;                                          // A position marking where to check for throw
        #endregion

        #region Properties
        public CharacterController2D CharacterController2D
        {
            get
            {
                return _characterController2D;
            }
        }

        public CharacterInputController2D CharacterInputController2D
        {
            get
            {
                return _characterInputController2D;
            }
        }

        public CharacterParams CharacterParams
        {
            get
            {
				return _characterParams;
            }
        }

        public CharacterFlags CharacterFlags
        {
            get
            {
                return _characterFlags;
            }
        }

        public CharacterEvents CharacterEvents
        {
            get
            {
                return _characterEvents;
            }
        }

        public CharacterAudio CharacterAudio
        {
            get
            {
                return _characterAudio;
            }
        }

        public Rigidbody2D Rigidbody2D
        {
            get
            {
                return _rigidbody2D;
            }
        }

        public CapsuleCollider2D MainCapsuleCollider2D
        {
            get
            {
                return _mainCapCollider2D;
            }
        }

        public CapsuleCollider2D SlideCapsuleCollider2D
        {
            get
            {
                return _slideCapCollider2D;
            }
        }

        public CapsuleCollider2D GlideCapsuleCollider2D
        {
            get
            {
                return _glideCapCollider2D;
            }
        }

        public CapsuleCollider2D AttackCapsuleCollider2D
        {
            get
            {
                return _attackCapCollider2D;
            }
        }

        public CapsuleCollider2D ThrowCapsuleCollider2D
        {
            get
            {
                return _throwCapCollider2D;
            }
        }

        public Animator Animator
        {
            get
            {
                return _animator;
            }
        }

        public SpriteRenderer SpriteRenderer
        {
            get
            {
                return _spriteRenderer;
            }
        }

        public AudioSource AudioSource
        {
            get
            {
                return _audioSource;
            }
        }

        public Transform GroundCheck
        {
            get
            {
                return _groundCheck;
            }
        }

        public Transform CeilingCheck
        {
            get
            {
                return _ceilingCheck;
            }
        }

        public Transform AttackCheck
        {
            get
            {
                return _attackCheck;
            }
        }

        public Transform ThrowCheck
        {
            get
            {
                return _throwCheck;
            }
        }
        #endregion
    }
}
