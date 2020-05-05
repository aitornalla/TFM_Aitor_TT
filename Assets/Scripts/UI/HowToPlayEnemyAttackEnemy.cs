using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI
{
	public class HowToPlayEnemyAttackEnemy : MonoBehaviour
	{
        [SerializeField]
        private HowToPlayAttackEnemy _howToPlayAttackEnemy;                     // Reference to component

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

        /// <summary>
        ///     Gets called when enemy dead animation ends
        /// </summary>
        public void EnemyDeadEndAnimationEvent()
        {
            _howToPlayAttackEnemy.EnemyDeadAnimationFinished();
        }
    }
}
