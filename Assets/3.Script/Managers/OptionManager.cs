using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionManager : MonoBehaviour
{
    public AudioMixer masterMixer;
    public Slider audioSlider;
    public Text resolutionText;
    public Text screenText;
    public float volume;
    public int resolutionCnt = 0;
    public bool isFullScreen = true;
    public bool isChange = false;

    [SerializeField] float incepVolume;
    [SerializeField] int incepresolCnt;
    [SerializeField] bool incepFullScreen;

    string[] resolution = { "1080P", "900P", "720P" };

    private void Awake()
    {
        OptionDataLoad();
        transform.gameObject.SetActive(false);
    }
    
    private void OnEnable()
    {
        isChange = false;
        incepVolume = audioSlider.value;
        incepresolCnt = resolutionCnt;
        incepFullScreen = isFullScreen;
        SoundManager.instance.PlaySFX("MenuEnable");
        
        if (PlayerPrefs.GetFloat("Volume") != 0.0f) 
        { 
            audioSlider.value = PlayerPrefs.GetFloat("Volume");
            volume = audioSlider.value;
        }

        if (PlayerPrefs.GetString("Resolution") != null)
        {
            resolutionText.text = PlayerPrefs.GetString("Resolution");
            for(int i = 0; i < resolution.Length; i++)
            {
                if (PlayerPrefs.GetString("Resolution").Equals(resolution[i]))
                {
                    resolutionCnt = i;
                }
            }
        }

        if (PlayerPrefs.GetString("Display") != null)
        {
            screenText.text = PlayerPrefs.GetString("Display");
            if (screenText.text.Equals("전체 화면"))
            {
                isFullScreen = true;
            }
            else isFullScreen = false;
        }

        Submit();
    }

    private void OnDisable()
    {
        if (isChange)
        {
            audioSlider.value = incepVolume;
            resolutionCnt = incepresolCnt;
            isFullScreen = incepFullScreen;
            resolutionText.text = resolution[incepresolCnt];
        }
    }
    public void AudioControl()
    {
        isChange = true;
        volume = audioSlider.value;
    }

    public void ToggleAudioVolume()
    {
        AudioListener.volume = AudioListener.volume == 0 ? 1 : 0;
    }

    public void Submit()
    {
        isChange = false;
        if (volume == -40f) masterMixer.SetFloat("Master", -80);
        else masterMixer.SetFloat("Master", volume);

        switch (resolutionCnt)
        {
            case 0:
                Screen.SetResolution(1920, 1080, isFullScreen);
                break;
            case 1:
                Screen.SetResolution(1600, 900, isFullScreen);
                break;
            case 2:
                Screen.SetResolution(1280, 720, isFullScreen);
                break;
        }

        incepFullScreen = isFullScreen;
        incepVolume = volume;
        incepresolCnt = resolutionCnt;
        OptionDataSave();
    }

    public void ResolutionSetRight()
    {
        if (resolutionCnt < 2)
        {
            isChange = true;
            resolutionCnt++;
            resolutionText.text = resolution[resolutionCnt];

        }  
        else return;
    }

    public void ResolutionSetLeft()
    {
        if (resolutionCnt > 0) 
        {
            isChange = true;
            resolutionCnt--;
            resolutionText.text = resolution[resolutionCnt];
        } 
        else return;
    }

    public void ScreenSizeLeft()
    {
        isChange = true;
        isFullScreen = true;
        screenText.text = "전체 화면";
    }

    public void ScreenSizeRight()
    {
        isChange = true;
        isFullScreen = false;
        screenText.text = "창 모드";
    }

    public void OptionDataSave()
    {
        if (isFullScreen)
        {
            PlayerPrefs.SetString("Display", "전체 화면");
            screenText.text = "전체 화면";

        }

        else
        {
            PlayerPrefs.SetString("Display", "창 모드");
            screenText.text = "창 모드";
        }
        isChange = false;

        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.SetString("Resolution", resolution[resolutionCnt]);
    }

    public void OptionDataLoad()
    {
        if (PlayerPrefs.GetFloat("Volume") == -40f)
        {
            masterMixer.SetFloat("Master", -80f);
            audioSlider.value = -40f;
        }
        else
        {
            masterMixer.SetFloat("Master", PlayerPrefs.GetFloat("Volume"));
            audioSlider.value = PlayerPrefs.GetFloat("Volume");
        }

        if (PlayerPrefs.GetString("Display").Equals("전체 화면"))
            isFullScreen = true;
        else
            isFullScreen = false;

        if (PlayerPrefs.GetString("Resolution").Equals("1080P"))
        {
            Screen.SetResolution(1920, 1080, isFullScreen);
        }
        else if (PlayerPrefs.GetString("Resolution").Equals("900P"))
        {
            Screen.SetResolution(1600, 900, isFullScreen);
        }
        else
        {
            Screen.SetResolution(1280, 720, isFullScreen);
        }
    }

    public void AwakeForResolution()
    {
        if (PlayerPrefs.GetString("Resolution").Equals("1080P"))
        {
            Screen.SetResolution(1920, 1080, isFullScreen);
        }
    }

    private void OnApplicationQuit()
    {
        OptionDataSave();
    }
}
