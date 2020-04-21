using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.GameManagerController;
using UnityEngine;

namespace Assets.Scripts.DeathRespawnBlackPanelController
{
	public class DeathRespawnBlackPanel : MonoBehaviour
	{
		// Use this for initialization
		void Start()
		{
			this.gameObject.SetActive(false);
		}

		// Update is called once per frame
		//void Update()
		//{
        //
		//}

        public void OnDeathRespawnBlackPanelEndAnimationEvent()
        {
			// Calls GameManager to manage player death/respawn after death animation ends
			GameManager.Instance.ManagePlayerDeathAndRespawn();

			// Set gameObject to inactive
			//this.gameObject.SetActive(false);
		}
	}
}
