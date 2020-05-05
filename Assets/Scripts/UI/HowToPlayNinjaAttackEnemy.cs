using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI
{
	public class HowToPlayNinjaAttackEnemy : MonoBehaviour
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
        ///     Gets called when attack animation reaches a keyframe when the attack should damage enemies
        /// </summary>
        public void OnAttackAnimationEvent()
        {
            _howToPlayAttackEnemy.AttackAnimation();
        }

        /// <summary>
        ///     Gets called when attack animation ends, sets "is attacking" flag to false
        /// </summary>
        public void OnAttackEndAnimationEvent()
        {
            _howToPlayAttackEnemy.AttackAnimationFinished();   
        }
    }
}
