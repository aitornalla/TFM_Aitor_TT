using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.GameManagerController;
using Assets.Scripts.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.LevelEndController
{
	public class BossEnd : MonoBehaviour
	{
		[SerializeField]
		private GameObject _bossDefeatedBanner;                                 // Reference to boss defeated banner

		private AudioSource _audioSource = null;                                // Reference to AudioSource component

		private void Awake()
		{
			// Get AudioSource component
			_audioSource = GetComponent<AudioSource>();
		}

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

        public void BossEndProcess()
        {
			// Disable player control
			GameManager.Instance.PlayerInstance.GetComponent<CharacterFlags>().IsPlayerControlAllowed = false;

			// Unlock next level
			string l_levelName = SceneManager.GetActiveScene().name;
			GameManager.Instance.UnlockNextLevel(l_levelName);

			// Stop GameManager sound
			GameManager.Instance.AudioSource.Stop();

			// Play sound
			_audioSource.Play();

			// Set bos defeated banner active
			_bossDefeatedBanner.SetActive(true);
		}
	}
}
