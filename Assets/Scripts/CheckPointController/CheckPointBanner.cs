using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.CheckPointController
{
	public sealed class CheckPointBanner : MonoBehaviour
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

		public void OnCheckpointBannerEndAnimationEvent()
		{
			this.gameObject.SetActive(false);
		}
	}
}
