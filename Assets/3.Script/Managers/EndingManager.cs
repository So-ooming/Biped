using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndingManager : MonoBehaviour
{
    int coin = 0, death = 0;
    float timer = 0;
    [SerializeField] Text coinText;
    [SerializeField] Text deathText;
    [SerializeField] Text timeText;

    void Start()
    {
        coin = PlayerPrefs.GetInt("Coin");
        death = PlayerPrefs.GetInt("Death");
        timer = PlayerPrefs.GetFloat("Time");

        SetResult();
        
    }

    void SetResult()
    {
        coinText.text = "<color=#ff0000>" + coin.ToString() + "</color>" + " / 65";
        deathText.text = "<color=#ff0000>" + death.ToString() + "</color>" + " / 5";
        timeText.text = "<color=#ff0000>" + (int)(timer / 60) + "\'" + (int)(timer % 60) + "\""
            + "</color>" + " / 05\'00\"";
        if(coin >= 65)
            coinText.text = "<color=#45B1FF>" + coin.ToString() + "</color>" + " / 65";

        if(death <= 5)
            deathText.text = "<color=#45B1FF>" + death.ToString() + "</color>" + " / 5";

        if(timer <= 300f)
            timeText.text = "<color=#45B1FF>" + (int)(timer / 60) + "\'" + (int)(timer % 60) + "\""
                            + "</color>" + " / 05\'00\"";
    }

    public void GoToMain()
    {
        SceneManager.LoadScene("Intro");
    }
}
