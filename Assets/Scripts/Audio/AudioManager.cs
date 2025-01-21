using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // ���������� ��� �������� ������
    public AudioClip mainTheme;
    public AudioClip blasterShot;
    public AudioClip cannonShot;
    public AudioClip tankShot;
    public AudioClip constructionSound;
    public AudioClip buttonClick;

    // ������������� ��� ��������������� ������
    private AudioSource audioSource;

    void Start()
    {
        // ������������� ��������������
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // ��������������� ������� ����������� ���� �� ������
        PlayMainTheme();
    }

    // ����� ��� ��������������� ������� ����������� ����
    public void PlayMainTheme()
    {
        PlaySound(mainTheme, true); // ����������� � �����
    }

    // ����� ��� ��������������� �������� ��������
    public void PlayBlasterShot()
    {
        PlaySound(blasterShot);
    }

    // ����� ��� ��������������� �������� �����
    public void PlayCannonShot()
    {
        PlaySound(cannonShot);
    }

    // ����� ��� ��������������� �������� �����
    public void PlayTankShot()
    {
        PlaySound(tankShot);
    }

    // ����� ��� ��������������� ����� �������������
    public void PlayConstructionSound()
    {
        PlaySound(constructionSound);
    }

    // ����� ��� ��������������� ����� ������� �������
    public void PlayButtonClick()
    {
        PlaySound(buttonClick);
    }

    // ����� ����� ��� ��������������� �����
    private void PlaySound(AudioClip clip, bool loop = false)
    {
        if (clip != null)
        {
            audioSource.clip = clip;
            audioSource.loop = loop;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Audio clip is missing!");
        }
    }

    // ����� ��� ��������� �������� �����
    public void StopSound()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
