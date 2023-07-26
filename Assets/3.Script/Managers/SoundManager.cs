using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class SoundManager : MonoBehaviour
{
    #region 싱글톤
    public static SoundManager instance = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion
    public Sound[] BGM;     // 배경음 배열
    public Sound[] SFX;     // 효과음 배열

    public AudioSource BGMPlayer;
    public AudioSource[] SFXPlayer;


    private void Start()
    {
        AutoSetting();
        PlayBGM("MainBGM");
        PlaySFX("MenuEnable");
    }

    void AutoSetting()
    {
        BGMPlayer = transform.GetChild(0).GetComponent<AudioSource>();
        SFXPlayer = transform.GetChild(1).GetComponents<AudioSource>();
    }

    public void PlayBGM(string name)
    {
        foreach(Sound s in BGM)
        {
            if (s.name.Equals(name))
            {
                BGMPlayer.clip = s.clip;
                BGMPlayer.Play();
                return;
            }
        }
    }

    public void PlaySFX(string name)
    {
        foreach(Sound s in SFX)
        {
            if (s.name.Equals(name))
            {
                for(int i = 0; i< SFXPlayer.Length; i++)
                {
                    if (!SFXPlayer[i].isPlaying)
                    {
                        SFXPlayer[i].clip = s.clip;
                        SFXPlayer[i].Play();
                        return;
                    }
                }
            }
        }
    }
}
