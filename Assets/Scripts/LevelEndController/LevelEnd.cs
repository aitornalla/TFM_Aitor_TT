using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts.Scenes;
using Assets.Scripts.GameManagerController;
using Assets.Scripts.Player;
using Assets.Scripts.UI;
using Assets.Scripts.TimeTrialController;

namespace Assets.Scripts.LevelEndController
{
	public class LevelEnd : MonoBehaviour
	{
		public EGameScenes _gameScenes;                                         // Game scene to load after player reaches level end

		[SerializeField]
		private LayerMask _playerLayer;                                         // Player layer to check conditions
		[SerializeField]
		private GameObject _levelCompletedBanner;                               // Level completed banner gameObject
		[SerializeField]
		private GameObject _timeTrialCompletedBanner;                           // Time trial completed banner gameObject

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

		private void OnTriggerEnter2D(Collider2D collision)
        {
            // If player gets to the level end ...
            if (_playerLayer == (_playerLayer | (1 << collision.gameObject.layer)))
            {
				// Load next scene
				//GameManager.Instance.GameManagerState.StateChange(_gameScenes);

				// Disable player control
				GameManager.Instance.PlayerInstance.GetComponent<CharacterFlags>().IsPlayerControlAllowed = false;

				// Check for time trial
				TimeTrial l_timeTrial = GameManager.Instance.TimeTrialClock.GetComponent<TimeTrial>();

                // Whether is time trial or not
				if (l_timeTrial.IsTimeTrial)
                {
					ManageTimeTrialLevelEnd(l_timeTrial);
				}
                else
                {
					ManageLevelEnd();
                }
			}
        }

		private void ManageTimeTrialLevelEnd(TimeTrial timeTrial)
        {
            // Manage end of time trial
			timeTrial.EndTimeTrial();

            // Set time trial completed banner active
			_timeTrialCompletedBanner.SetActive(true);
		}

        private void ManageLevelEnd()
        {
			// Update level score
			string l_levelName = SceneManager.GetActiveScene().name;
			int l_levelScore = GameManager.Instance.LevelScoreCounter.GetComponent<LevelScoreCounter>().TotalScore;

			GameManager.Instance.UpdateLevelScore(l_levelName, l_levelScore);

			// Unlock next level
			GameManager.Instance.UnlockNextLevel(l_levelName);

			// Unlock level time trial
			GameManager.Instance.UnlockLevelTimeTrial(l_levelName);

			// Set level completed banner active
			_levelCompletedBanner.SetActive(true);
		}
    }
}
