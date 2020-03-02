using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameManager
{
	public class GameManager : MonoBehaviour {

		private static GameManager _instance = null;

		#region Properties
		public GameManager Instance
		{
			get { return _instance; }
		}
		#endregion

		#region Awake
		void Awake () {

			// Singleton creation
			if (_instance == null) {

				_instance = this;

				DontDestroyOnLoad (gameObject);

			} else {

				DestroyImmediate (gameObject);

			}

		}
		#endregion

		#region Start
		// Use this for initialization
		void Start () {

		}
		#endregion

		#region Update
		// Update is called once per frame
		void Update () {

		}
		#endregion
	}	
}

