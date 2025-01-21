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
    public AudioClip rocketLaunch;
    public AudioClip rocketExplosion;
    public AudioClip stickHit;

    // ������� ��� �������� ��������� ������� �����
    private Dictionary<AudioClip, float> soundVolumes;

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

        // ������������� ��������� ��� ������� �����
        soundVolumes = new Dictionary<AudioClip, float>
        {
            { mainTheme, 1.0f },
            { blasterShot, 1.0f },
            { cannonShot, 1.0f },
            { tankShot, 1.0f },
            { constructionSound, 1.0f },
            { buttonClick, 1.0f },
            { rocketLaunch, 1.0f },
            { rocketExplosion, 1.0f },
            { stickHit, 1.0f }
        };

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

    // ����� ��� ��������������� ������� ������
    public void PlayRocketLaunch()
    {
        PlaySound(rocketLaunch);
    }

    // ����� ��� ��������������� ������ ������
    public void PlayRocketExplosion()
    {
        PlaySound(rocketExplosion);
    }

    // ����� ��� ��������������� ����� ������
    public void PlayStickHit()
    {
        PlaySound(stickHit);
    }

    // ����� ����� ��� ��������������� �����
    private void PlaySound(AudioClip clip, bool loop = false)
    {
        if (clip != null)
        {
            if (!audioSource.isPlaying || loop)
            {
                audioSource.clip = clip;
                audioSource.loop = loop;
                audioSource.volume = soundVolumes.ContainsKey(clip) ? soundVolumes[clip] : 1.0f;
                audioSource.Play();
            }
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

    // ����� ��� ����������� ��������� ����������� �����
    public void SetVolume(AudioClip clip, float volume)
    {
        if (soundVolumes.ContainsKey(clip))
        {
            soundVolumes[clip] = Mathf.Clamp(volume, 0f, 1f);
        }
        else
        {
            Debug.LogWarning("Audio clip not found in volume settings!");
        }
    }
}