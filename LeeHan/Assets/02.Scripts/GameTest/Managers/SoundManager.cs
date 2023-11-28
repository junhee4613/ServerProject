using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioSource[] sfxSources;

    void Start()
    {
        // 초기화: 모든 SFX AudioSource를 배열에 추가
        sfxSources = GetComponentsInChildren<AudioSource>();
    }

    public void SetMusicVolume(float volume)
    {
        if (musicSource != null)
        {
            musicSource.volume = volume;
        }
    }

    public void SetSFXVolume(float volume)
    {
        foreach (var sfxSource in sfxSources)
        {
            sfxSource.volume = volume;
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        foreach (var sfxSource in sfxSources)
        {
            if (!sfxSource.isPlaying)
            {
                sfxSource.clip = clip;
                sfxSource.Play();
                break;
            }
        }
    }
}
