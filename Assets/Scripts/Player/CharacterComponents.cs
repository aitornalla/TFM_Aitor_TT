using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Player
{
    public sealed class CharacterComponents : MonoBehaviour
    {
        [Header("Components")]
	    #region Components
	    [SerializeField]
	    private CharacterParams _characterParams;       // Component to hold character parameters
        [SerializeField]
        private CharacterFlags _characterFlags;         // Component to hold character flags
        [SerializeField]
        private CharacterEvents _characterEvents;       // Component to hold character events
        [SerializeField]
	    private Rigidbody2D _rigidbody2D;               // Rigidbody2D component of the gameObject
	    [SerializeField]
	    private CapsuleCollider2D _mainCapCollider2D;   // Not-sliding CapsuleCollider2D component of the gameObject
	    [SerializeField]
	    private CapsuleCollider2D _slideCapCollider2D;  // Sliding CapsuleCollider2D component of the gameObject
	    [SerializeField]
	    private CapsuleCollider2D _glideCapCollider2D;  // Gliding CapsuleCollider2D component of the gameObject
	    [SerializeField]
	    private CapsuleCollider2D _attackCapCollider2D; // Attack CapsuleCollider2D component of the gameObject
	    [SerializeField]
	    private SpriteRenderer _spriteRenderer;         // SpriteRenderer component of the gameObject
        #endregion

        #region Properties
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

        public SpriteRenderer SpriteRenderer
        {
            get
            {
                return _spriteRenderer;
            }
        }
        #endregion
    }
}
