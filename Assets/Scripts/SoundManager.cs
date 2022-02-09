using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SoundManager : MonoSingleTon<SoundManager>
{
    [SerializeField]private List<AudioClip> effectSounds = null;
    [SerializeField]private List<AudioClip> bgms = null;
    private AudioSource bgmAudio;
    private AudioSource effectAudio;

    bool IsMain
    {
        get
        {
            return SceneManager.GetActiveScene().ToString() == "Main";
        }
    }

    private void Awake()
    {
        SoundManager[] smanagers = FindObjectsOfType<SoundManager>();
        if (smanagers.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        bgmAudio = GetComponent<AudioSource>();
        effectAudio = transform.GetChild(0).GetComponent<AudioSource>();
    }

    private void Start()
    {
        //if (SceneManager.GetActiveScene().name == "Title")
        //{
        //    StartVolumeSetting();
        //}
    }

    public void VolumeSetting()
    {
        bgmAudio.volume = 0.5f;
        effectAudio.volume = 0.5f;
        bgmAudio.mute = false;
        effectAudio.mute = false;
    }

    public void BGMVolume(float value)
    {
        if (bgmAudio == null) return;
        bgmAudio.volume = value;

    }

    public void BGMMute(bool isMute)
    {
        bgmAudio.mute = isMute;
    }
    public void EffectMute(bool isMute)
    {
        effectAudio.mute = isMute;
    }

    public void EffectVolume(float value)
    {
        if (effectAudio == null) return;
        effectAudio.volume = value;
    }
    public void SetBGM(int bgmNum)
    {
        bgmAudio.Stop();
        bgmAudio.clip = bgms[bgmNum];
        bgmAudio.Play();
    }
    public void SetEffectSound(int effectNum)
    {
        effectAudio.Stop();

        effectAudio.clip = effectSounds[effectNum];
        effectAudio.Play();
    }
    public void StopBGM()
    {
        bgmAudio.Stop();
    }

    public float GetEffectSoundLength()
    {
        return effectAudio.clip.length;
    }

}
