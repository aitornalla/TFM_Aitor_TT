using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Scenes;
using Assets.Scripts.GameManagerController;

namespace Assets.Scripts.LevelEndController
{
	public class LevelEnd : MonoBehaviour
	{
		[SerializeField]
		private LayerMask _playerLayer;                             // Player layer to check conditions
		[SerializeField]
		private EGameScenes _gameScenes;                            // Game scene to load after player reaches level end

		// Use this for initialization
		private void Start()
		{

		}

		// Update is called once per frame
		private void Update()
		{

		}

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // If player gets to the level end ...
            if (_playerLayer == (_playerLayer | (1 << collision.gameObject.layer)))
            {
				// Load next scene
				GameManager.Instance.GameManagerState.StateChange(_gameScenes);
            }
        }
    }
}
