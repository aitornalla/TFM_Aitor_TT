using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Player
{
	public class CharacterAudio : MonoBehaviour
	{
		[SerializeField]
		private CharacterComponents _characterComponents;                       // Reference to CharacterComponents component

		[Header("Attack")]
		[SerializeField]
		private AudioClip _audioClipAttack1;                                    // Attack1 audioclip
		[SerializeField]
		private AudioClip _audioClipAttack2;                                    // Attack2 audioclip
		[SerializeField]
		private AudioClip _audioClipAttack3;                                    // Attack3 audioclip

		[Header("Damage")]
		[SerializeField]
		private AudioClip _audioClipDamage1;                                    // Damage1 audioclip
		[SerializeField]
		private AudioClip _audioClipDamage2;                                    // Damage2 audioclip
		[SerializeField]
		private AudioClip _audioClipDamage3;                                    // Damage3 audioclip

		[Header("Dead")]
		[SerializeField]
		private AudioClip _audioClipDead;                                       // Dead audioclip

		[Header("Jump")]
        [SerializeField]
        private AudioClip _audioClipJump1;                                      // Jump1 audioclip
		[SerializeField]
		private AudioClip _audioClipJump2;                                      // Jump2 audioclip
        [SerializeField]
        private AudioClip _audioClipJump3;                                      // Jump3 audioclip

		[Header("Parachute")]
		[SerializeField]
		private AudioClip _audioClipOpenParachute;                              // Open parachute audioclip
		[SerializeField]
		private AudioClip _audioClipParachute1;                                 // Parachute1 audioclip
		[SerializeField]
		private AudioClip _audioClipParachute2;                                 // Parachute2 audioclip

		[Header("Slide")]
		[SerializeField]
		private AudioClip _audioClipSlide;                                      // Slide audioclip

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

		public void Attack()
		{
			float l_value = Random.value * 1000.0f;

			if (l_value < 333.0f)
			{
				_characterComponents.AudioSource.PlayOneShot(_audioClipAttack1);
			}

			if (l_value >= 333.0f && l_value < 666.0f)
			{
				_characterComponents.AudioSource.PlayOneShot(_audioClipAttack2);
			}

			if (l_value >= 666.0f)
			{
				_characterComponents.AudioSource.PlayOneShot(_audioClipAttack3);
			}
		}

		public void Damage()
		{
			float l_value = Random.value * 1000.0f;

			if (l_value < 333.0f)
			{
				_characterComponents.AudioSource.PlayOneShot(_audioClipDamage1);
			}

			if (l_value >= 333.0f && l_value < 666.0f)
			{
				_characterComponents.AudioSource.PlayOneShot(_audioClipDamage2);
			}

			if (l_value >= 666.0f)
			{
				_characterComponents.AudioSource.PlayOneShot(_audioClipDamage3);
			}
		}

        public void Dead()
        {
			_characterComponents.AudioSource.PlayOneShot(_audioClipDead);
        }

		public void Jump()
        {
			float l_value = Random.value * 1000.0f;

            if (l_value < 333.0f)
            {
				_characterComponents.AudioSource.PlayOneShot(_audioClipJump1);
            }

			if (l_value >= 333.0f && l_value < 666.0f)
			{
				_characterComponents.AudioSource.PlayOneShot(_audioClipJump2);
			}

			if (l_value >= 666.0f)
			{
				_characterComponents.AudioSource.PlayOneShot(_audioClipJump3);
			}
		}

		public void OpenParachute()
		{
			_characterComponents.AudioSource.PlayOneShot(_audioClipOpenParachute);
		}

		public void Slide()
		{
			_characterComponents.AudioSource.PlayOneShot(_audioClipSlide);
		}
	}
}
