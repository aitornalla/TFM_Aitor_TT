using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
	public class LevelScoreCounter : MonoBehaviour
	{
		[SerializeField]
		private Text _scoreText;                                                // Reference to score Text component
        [SerializeField]
        private int _scoreIncrementStep;                                        // Score increment step for visual FX

		private int _totalScore;                                                // Holds player total score
		private int _currentScore;                                              // Current score for incrementing visual effect

		private Coroutine _incrementingScoreCoroutine = null;                   // Holds reference to the coroutine used for the incrementing visual effect

        public int TotalScore { get { return _totalScore; } }

		// Use this for initialization
		private void Start()
		{
            // Initialize player score to 0
			_totalScore = 0;
			_currentScore = 0;
            // Set score text
			_scoreText.text = _currentScore.ToString();
		}

		// Update is called once per frame
		//private void Update()
		//{
		//
		//}

        /// <summary>
        ///     Coroutine for visual FX incrementing score counter
        /// </summary>
        /// <returns>Reference to coroutine</returns>
        private IEnumerator IncrementingScoreFXCoroutine()
        {
            // Don't stop incrementing until the current score equals the total score
            while (_currentScore != _totalScore)
            {
                // Increment current score
				_currentScore += _scoreIncrementStep;
                // In case the sum is not even/odd set the current score correctly
				if (_currentScore > _totalScore)
					_currentScore = _totalScore;
                // Set score text
				_scoreText.text = _currentScore.ToString();
                // Yield coroutine
				yield return null;
            }
            // Unset coroutine reference
			_incrementingScoreCoroutine = null;
        }

        /// <summary>
        ///     Adds score to total
        /// </summary>
        /// <param name="score">Score to add</param>
        public void AddScore(int score)
        {
            // Add score to total
			_totalScore += score;
            // If the coroutine is working, don't call it again
            if (_incrementingScoreCoroutine == null)
				_incrementingScoreCoroutine = StartCoroutine(IncrementingScoreFXCoroutine());
        }
	}
}
