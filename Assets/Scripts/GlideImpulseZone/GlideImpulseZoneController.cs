using Assets.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GlideImpulseZone
{
	public sealed class GlideImpulseZoneController : MonoBehaviour
	{
        [SerializeField]
		private LayerMask _playerLayer;                 // Player layer to check conditions
        [SerializeField]
        private AreaEffector2D _areaEffector2D;         // AreaEffector2D attached to this gameObject
        [SerializeField]
		private bool _isPlayerIn = false;               // Flag to chech whether the player is in the area or not

        private CharacterFlags _characterFlags;         // CharacterFlags component to get glide flag

		// Use this for initialization
		//private void Start()
		//{
        //
		//}

		// Update is called once per frame
		private void Update()
		{
            if (_isPlayerIn && _characterFlags != null)
            {
                // If player was gliding, enable area effector
                if (_characterFlags.WasGliding)
                {
                    if (!_areaEffector2D.enabled)
                        _areaEffector2D.enabled = true;
                }
                // If player was not gliding, disable area effector
                if (!_characterFlags.WasGliding)
                {
                    if (_areaEffector2D.enabled)
                        _areaEffector2D.enabled = false;
                }
            }
		}

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // If player enters the area ...
            if (_playerLayer == (_playerLayer | (1 << collision.gameObject.layer)))
            {
                // Check flag to true
				_isPlayerIn = true;
                // Get component from player gameObject
                _characterFlags = collision.gameObject.GetComponent<CharacterFlags>();
                // Check flag to true
                if (_characterFlags != null)
                    _characterFlags.IsInGlideImpulseZone = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            // If player leaves the area ...
            if (_playerLayer == (_playerLayer | (1 << collision.gameObject.layer)))
            {
                // Check flag to false
                _isPlayerIn = false;
                // Check flag to false
                if (_characterFlags != null)
                    _characterFlags.IsInGlideImpulseZone = false;
                // Put reference to null
                _characterFlags = null;
                // Player leaves area, disable component
                _areaEffector2D.enabled = false;
            }
        }
    }
}
