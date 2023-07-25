using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region 싱글톤 (Awake)
    public static GameManager instance = null;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion
    public bool isPause = false;
    public GameObject LastDoor;
    public GameObject CoinUI;
    public GameObject PauseUI;
    public int coinCnt = 0;
    public int deathCnt = 0;
    public float timer = 0f;
    public Text coinText;
    public Image whitePanel;
    public Animator coinAnim;

    private void Start()
    {
        coinText = CoinUI.transform.GetChild(0).GetChild(0).GetComponent<Text>();
        coinAnim = CoinUI.transform.GetComponent<Animator>();
    }


    private void Update()
    {
        if(!isPause)
            timer += Time.deltaTime;
        coinText.text = coinCnt.ToString();
        if (Input.GetKeyDown(KeyCode.Escape) && !PauseUI.activeSelf)
        {
            ActivePauseUI();
        }

        else if (Input.GetKeyDown(KeyCode.Escape) && PauseUI.activeSelf)
        {
            InactivePauseUI();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && PauseUI.transform.GetChild(3).gameObject.activeSelf)
        {
            BackPauseUI();
        }
    }

    public void Ending()
    {
        LastDoor.transform.GetComponent<Animator>().enabled = true;
        PlayerPrefs.SetInt("Coin", coinCnt);
        PlayerPrefs.SetInt("Death", deathCnt);
        PlayerPrefs.SetFloat("Time", timer);

        StartCoroutine("WhiteRust_co");
    }

    IEnumerator WhiteRust_co()
    {
        yield return new WaitForSeconds(6f);
        for(int i = 0; i < 500; i++)
        {
            whitePanel.color += new Color(0, 0, 0, Time.deltaTime);
            yield return null;
        }
        GoToEndingScene();
    }

    public void GoToEndingScene()
    {
        SceneManager.LoadScene("Ending");
    }

    public void ActivePauseUI()
    {
        PauseUI.SetActive(true);
        Debug.Log("왜 안 켜져 이거");
        isPause = true;
        CoinUI.SetActive(false);
    }

    public void InactivePauseUI()
    {
        PauseUI.SetActive(false);
        Debug.Log("안 켜지는 이유"); 
        isPause = false;
        CoinUI.SetActive(true);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Intro");
    }

    public void MainmenuUI()
    {
        PauseUI.transform.GetChild(2).gameObject.SetActive(false);
        PauseUI.transform.GetChild(3).gameObject.SetActive(true);
    }

    public void BackPauseUI()
    {
        PauseUI.transform.GetChild(3).gameObject.SetActive(false);
        PauseUI.transform.GetChild(2).gameObject.SetActive(true);
    }
}
