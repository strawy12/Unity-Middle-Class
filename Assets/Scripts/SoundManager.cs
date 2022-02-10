using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SoundManager : MonoSingleTon<SoundManager>
{
    [SerializeField] private List<AudioClip> effectSounds = null;
    [SerializeField] private List<AudioClip> bgms = null;
    [SerializeField] private List<AudioClip> easterEggEffectSounds = null;
    private AudioSource bgmAudio;
    private AudioSource effectAudio;

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
        VolumeSetting();
    }

    public void VolumeSetting()
    {
        bgmAudio.volume = DataManager.Inst.CurrentPlayer.bgmSoundVolume;
        effectAudio.volume = DataManager.Inst.CurrentPlayer.effectSoundVolume;
        bgmAudio.mute = false;
        effectAudio.mute = false;
    }

    public void BGMVolume(float value)
    {
        if (bgmAudio == null) return;
        bgmAudio.volume = value;
        DataManager.Inst.CurrentPlayer.bgmSoundVolume = value;
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
        DataManager.Inst.CurrentPlayer.effectSoundVolume = value;
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

    public void SetEsterEggEffectSound(int effectNum)
    {
        effectAudio.Stop();

        effectAudio.clip = easterEggEffectSounds[effectNum];
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
