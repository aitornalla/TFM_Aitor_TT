using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.GameManagerController;
using UnityEngine;

namespace Assets.Scripts.CustomClasses
{
	public class AudioPauser : MonoBehaviour
	{
		private AudioSource _audioSource = null;                                // Reference to AudioSource component
        private bool _wasPlaying = false;                                       // Flag to know if AudioSource was playing before pause

        private void Awake()
        {
			// Get AudioSource component
			_audioSource = GetComponent<AudioSource>();
        }

        // Use this for initialization
        void Start()
		{
			GameManager.Instance.OnPauseEvent.AddListener(OnGamePaused);
		}

		// Update is called once per frame
		//private void Update()
		//{
        //
		//}

        /// <summary>
        ///     Added to GameManager pause event. Pause/Resume AudioSource in gameObject
        /// </summary>
        /// <param name="pause">Is game paused?</param>
        public void OnGamePaused (bool pause)
        {
            if (pause)
            {
                if (_audioSource.isPlaying)
                {
                    _wasPlaying = true;

                    _audioSource.Pause();
                }
            }
            else
            {
                if (_wasPlaying)
                {
                    _wasPlaying = false;

                    _audioSource.Play();
                }
            }
        }
	}
}
