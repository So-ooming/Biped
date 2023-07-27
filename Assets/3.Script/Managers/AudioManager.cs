using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
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

    private void OnEnable()
    {
        isChange = false;
        incepVolume = audioSlider.value;
        incepresolCnt = resolutionCnt;
        incepFullScreen = isFullScreen;
    }

    private void OnDisable()
    {
        if (isChange)
        {
            audioSlider.value = incepVolume;
            resolutionCnt = incepresolCnt;
            isFullScreen = incepFullScreen;
            resolutionText.text = resolution[incepresolCnt];
            if (isFullScreen) screenText.text = "전체 화면";
            else screenText.text = "창 모드";
            isChange = false;
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
}
