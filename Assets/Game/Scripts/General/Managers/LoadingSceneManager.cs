using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Casual.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Managers
{
    public class LoadingSceneManager : MonoBehaviour
    {
        [SerializeField] private Image progressBar;

        private void Awake()
        {
            StartCoroutine(LoadScene());
        }

        private IEnumerator LoadScene()
        {
            progressBar.fillAmount = 0;
            var scene = SceneManager.LoadSceneAsync(1);
             while (!scene.isDone)
             {
                 progressBar.fillAmount = scene.progress;
                 yield return null;
             }
        }
    }
}
