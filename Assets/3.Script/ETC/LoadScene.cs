using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public static string sceneName;
    [SerializeField] Slider loadingBar;

    private void Start()
    {
        LoadNextScene();
    }

    public void LoadNextScene()
    {
        StartCoroutine(LoadSceneAsync());
    }
    IEnumerator LoadSceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;
        while (!asyncLoad.isDone)
        {
            loadingBar.value += 0.003f;
            if(loadingBar.value >= 1)
                asyncLoad.allowSceneActivation = true;
            yield return null;
        }
    }
}
