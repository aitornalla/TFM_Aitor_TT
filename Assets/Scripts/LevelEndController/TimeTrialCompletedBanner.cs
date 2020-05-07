using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.GameManagerController;
using Assets.Scripts.Scenes;
using Assets.Scripts.TimeTrialController;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.LevelEndController
{
	public class TimeTrialCompletedBanner : MonoBehaviour
	{
		[SerializeField]
		private GameObject _levelTimeStats;                                     // Level time stats UI gameObject
		[SerializeField]
		private Text _yourTimeText;                                             // Your time UI text
		[SerializeField]
		private Text _firstTimeText;                                            // First time UI text
		[SerializeField]
		private Text _secondTimeText;                                           // Second time UI text
		[SerializeField]
		private Text _thirdTimeText;                                            // Third time UI text

		private GameObject _blinkTimeStat = null;

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

		public void OnLevelCompletedBannerEndAnimationEvent()
		{
			// Get TimeTrial
			TimeTrial l_timeTrial = GameManager.Instance.TimeTrialClock.GetComponent<TimeTrial>();
			// Set your time text
			_yourTimeText.text =
				l_timeTrial.TimeTrialElapsedTime.Minutes.ToString("D2") + ":" +
				l_timeTrial.TimeTrialElapsedTime.Seconds.ToString("D2") + ":" +
				l_timeTrial.TimeTrialElapsedTime.Milliseconds.ToString("D3");
			// Retrieve level times
			string l_levelName = SceneManager.GetActiveScene().name;
			string[] l_levelTimes = GameManager.Instance.RetrieveLevelTimes(l_levelName);
			// Set level times
			if (l_levelTimes != null)
			{
				_firstTimeText.text = l_levelTimes[0];
				_secondTimeText.text = l_levelTimes[1];
				_thirdTimeText.text = l_levelTimes[2];
			}
			// Set level time stats UI active
			_levelTimeStats.SetActive(true);
			// Start coroutine
			StartCoroutine(DisplayLevelTimeStatsCoroutine());
			// Start coroutine
			if (_firstTimeText.text.Equals(_yourTimeText.text))
            {
				_blinkTimeStat = _firstTimeText.gameObject;

				StartCoroutine(BlinkLevelTimeCoroutine());

				return;
            }
            //
			if (_secondTimeText.text.Equals(_yourTimeText.text))
			{
				_blinkTimeStat = _secondTimeText.gameObject;

				StartCoroutine(BlinkLevelTimeCoroutine());

				return;
			}
            //
			if (_thirdTimeText.text.Equals(_yourTimeText.text))
			{
				_blinkTimeStat = _thirdTimeText.gameObject;

				StartCoroutine(BlinkLevelTimeCoroutine());

				return;
			}
		}

        private IEnumerator DisplayLevelTimeStatsCoroutine()
        {
			yield return new WaitForSeconds(5.0f);

			// Get scene from LevelEnd gameObject
			EGameScenes l_scene = GameObject.FindGameObjectWithTag("LevelEnd").GetComponent<LevelEnd>()._gameScenes;

			// Load next scene
			GameManager.Instance.GameManagerState.StateChange(l_scene);
		}

        private IEnumerator BlinkLevelTimeCoroutine()
        {
			yield return new WaitForSeconds(0.75f);

			_blinkTimeStat.SetActive(false);

			yield return new WaitForSeconds(0.25f);

			_blinkTimeStat.SetActive(true);

			StartCoroutine(BlinkLevelTimeCoroutine());
        }
	}
}
