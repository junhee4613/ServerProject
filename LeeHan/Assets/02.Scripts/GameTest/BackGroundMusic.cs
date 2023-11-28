using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMusic : MonoBehaviour
{
    public AudioClip backgroundMusic; // Inspector���� �Ҵ��� ��� ���� ����� Ŭ��

    private AudioSource audioSource;

    void Start()
    {
        // AudioSource ���� ��Ҹ� �������ų� �߰��մϴ�.
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // ��� ������ �����ϰ� ����մϴ�.
        audioSource.clip = backgroundMusic;
        audioSource.loop = true; // �ݺ� ��� ����
        audioSource.Play();
    }

    // �ٸ� �޼��� ���� ������ �� �ֽ��ϴ�.
}
