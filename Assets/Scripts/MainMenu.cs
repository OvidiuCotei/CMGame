using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Slider loadingBar;
    public Text loadingText;

    public void StartGame(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        if (loadingBar != null) loadingBar.gameObject.SetActive(true);
        if (loadingText != null) loadingText.gameObject.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            if (loadingBar != null)
                loadingBar.value = progress;

            if (loadingText != null)
                loadingText.text = "Loading... " + (progress * 100).ToString("F0") + "%";

            if (operation.progress >= 0.9f)
            {
                if (loadingText != null)
                    loadingText.text = "Press any key to continue...";

                if (Input.anyKeyDown)
                {
                    operation.allowSceneActivation = true;
                }
            }

            yield return null;
        }
    }
}