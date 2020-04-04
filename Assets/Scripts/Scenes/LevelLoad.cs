using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Assets.Scripts.GameManagerController;

namespace Assets.Scripts.Scenes
{
	public class LevelLoad : MonoBehaviour
	{
        [SerializeField]
        private Slider _loadSlider;                                             // Reference to slider object in the scene

        /// <summary>
        ///     Loads next game level asynchrnously
        /// </summary>
        public void LoadLevelSceneAsync()
        {
            StartCoroutine(LoadLevelSceneAsyncCoroutine());
        }

        /// <summary>
        ///     Coroutine to load next game level asynchronously
        /// </summary>
        /// <returns></returns>
        private IEnumerator LoadLevelSceneAsyncCoroutine()
        {
            // First wait for frame to end
            yield return new WaitForEndOfFrame();

            // Get name of next scene to load
            string l_nextScene = string.Empty;

            GameManager.Instance.GameScenesDictionary.TryGetValue(GameManager.Instance.LevelLoadNextScene, out l_nextScene);

            // Start loading next scene asynchronously
            AsyncOperation l_asyncOperation = SceneManager.LoadSceneAsync(l_nextScene);

            // Update progress bar
            while (!l_asyncOperation.isDone)
            {
                float l_progress = Mathf.Clamp01(l_asyncOperation.progress / 0.9f);

                _loadSlider.value = l_progress;

                //Debug.Log("Level scene load progress: " + l_progress);

                yield return null;
            }
        }
    }
}
