using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.GameManagerController;
using Assets.Scripts.Scenes;
using UnityEngine;

namespace Assets.Scripts.LevelEndController
{
	public class LevelCompletedBanner : MonoBehaviour
	{
		// Use this for initialization
		//void Start()
		//{
		//
		//}

		// Update is called once per frame
		//void Update()
		//{
		//
		//}

		public void OnLevelCompletedBannerEndAnimationEvent()
		{
            // Get scene from LevelEnd gameObject
			EGameScenes l_scene = GameObject.FindGameObjectWithTag("LevelEnd").GetComponent<LevelEnd>()._gameScenes;

			// Load next scene
			GameManager.Instance.GameManagerState.StateChange(l_scene);
		}
	}
}
