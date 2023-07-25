using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject firstScene;
    [SerializeField] GameObject secondScene;
    [SerializeField] GameObject thirdScene;
    [SerializeField] GameObject ExitUI;

    void Update()
    {
        if((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return)) && firstScene.activeSelf)
        {
            firstScene.SetActive(false);
            secondScene.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Escape) && secondScene.activeSelf && !ExitUI.activeSelf)
        {
            secondScene.SetActive(false);
            firstScene.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Escape) && thirdScene.activeSelf)
        {
            thirdScene.SetActive(false);
            secondScene.SetActive(true);
        }

        if(ExitUI.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            ExitUI_OK();
        }

        if (ExitUI.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            ExitUI_Cancel();
        }

        if(Input.GetKeyDown(KeyCode.Return) && thirdScene.activeSelf)
        {
            GameStart();
        }
    }

    public void LoadThirdScene()
    {
        secondScene.SetActive(false);
        thirdScene.SetActive(true);
    }

    public void GameExit()
    {
        ExitUI.SetActive(true);
    }

    public void GameStart()
    {
        LoadScene.sceneName = "Stage_1";
        SceneManager.LoadScene("Loading");
    }

    public void ExitUI_OK()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }

    public void ExitUI_Cancel()
    {
        ExitUI.SetActive(false);
    }

    public void ThirdToSecondScene()
    {
        thirdScene.SetActive(false);
        secondScene.SetActive(true);
    }
}
