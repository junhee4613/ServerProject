using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMusic : MonoBehaviour
{
    public AudioClip backgroundMusic; // Inspector에서 할당할 배경 음악 오디오 클립

    private AudioSource audioSource;

    void Start()
    {
        // AudioSource 구성 요소를 가져오거나 추가합니다.
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // 배경 음악을 설정하고 재생합니다.
        audioSource.clip = backgroundMusic;
        audioSource.loop = true; // 반복 재생 설정
        audioSource.Play();
    }

    // 다른 메서드 등을 정의할 수 있습니다.
}
