using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.GameManagerController;
using UnityEngine;

namespace Assets.Scripts.Scenes
{
	public class IntroScene : MonoBehaviour
	{
        /// <summary>
        ///     Called by animation event end of intro scene. Changes scene to game main menu
        /// </summary>
		public void OnIntroSceneAnimationEnd()
        {
			GameManager.Instance.GameManagerState.StateChange();
		}
	}
}
