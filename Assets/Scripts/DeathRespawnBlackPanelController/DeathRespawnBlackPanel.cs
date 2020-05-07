using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.GameManagerController;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.DeathRespawnBlackPanelController
{
	public class DeathRespawnBlackPanel : MonoBehaviour
	{
		[SerializeField]
		private GameObject _playerUIHealthBar;
		[SerializeField]
		private GameObject _playerUILifesText;
		[SerializeField]
		private GameObject _playerUIScoreText;
		[SerializeField]
		private Text _noMoreLifesUIText;
		[SerializeField]
		private string _noMoreLifesText;
		[SerializeField]
		private float _timespanForNoMoreLifesText;
		[SerializeField]
		private float _timeBeforeChangingScene;

		// Use this for initialization
		private void Start()
		{
			this.gameObject.SetActive(false);
		}

		// Update is called once per frame
		//private void Update()
		//{
		//
		//}

        private IEnumerator NoMoreLifesTextCoroutine()
        {
            // Show no more lifes text on the screen
            foreach (char c in _noMoreLifesText)
            {
				_noMoreLifesUIText.text += c.ToString();

				yield return new WaitForSeconds(_timespanForNoMoreLifesText / _noMoreLifesText.Length);
            }

            // Wait before changing scene
			yield return new WaitForSeconds(_timeBeforeChangingScene);

			// Calls GameManager to manage player death/respawn after death animation ends
			GameManager.Instance.ManagePlayerDeathAndRespawn();
		}

		public void OnDeathRespawnBlackPanelStartAnimationEvent()
		{
			// If no more lifes left, set health bar and player lifes text to not active
			if (GameManager.Instance.PlayerLifes - 1 == 0)
			{
				_playerUIHealthBar.SetActive(false);
				_playerUILifesText.SetActive(false);
				_playerUIScoreText.SetActive(false);
			}
		}

		public void OnDeathRespawnBlackPanelEndAnimationEvent()
        {
			// If no more lifes left
			if (GameManager.Instance.PlayerLifes - 1 == 0)
			{
				StartCoroutine(NoMoreLifesTextCoroutine());

				return;
            }

			// Calls GameManager to manage player death/respawn after death animation ends
			GameManager.Instance.ManagePlayerDeathAndRespawn();

			// Set gameObject to inactive
			//this.gameObject.SetActive(false);
		}
	}
}
