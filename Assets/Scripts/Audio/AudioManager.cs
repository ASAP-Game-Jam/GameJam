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

    // ��� ��������������� ��� ������������ ���������������
    private List<AudioSource> audioSources = new List<AudioSource>();

    // ������������� ��� ��������������� ������
    private AudioSource mainAudioSource;

    void Start()
    {
        // ������������� ��������� ��������������
        mainAudioSource = gameObject.AddComponent<AudioSource>();

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
        PlaySoundAsync(blasterShot);
    }

    // ����� ��� ��������������� �������� �����
    public void PlayCannonShot()
    {
        PlaySoundAsync(cannonShot);
    }

    // ����� ��� ��������������� �������� �����
    public void PlayTankShot()
    {
        PlaySoundAsync(tankShot);
    }

    // ����� ��� ��������������� ����� �������������
    public void PlayConstructionSound()
    {
        PlaySoundAsync(constructionSound);
    }

    // ����� ��� ��������������� ����� ������� �������
    public void PlayButtonClick()
    {
        PlaySoundAsync(buttonClick);
    }

    // ����� ��� ��������������� ������� ������
    public void PlayRocketLaunch()
    {
        PlaySoundAsync(rocketLaunch);
    }

    // ����� ��� ��������������� ������ ������
    public void PlayRocketExplosion()
    {
        PlaySoundAsync(rocketExplosion);
    }

    // ����� ��� ��������������� ����� ������
    public void PlayStickHit()
    {
        PlaySoundAsync(stickHit);
    }

    // ����� ��� ��������������� ����� ���������� (�� �������� ������ �����)
    public void PlaySoundAsync(AudioClip clip)
    {
        if (clip != null)
        {
            AudioSource audioSource = GetAvailableAudioSource();
            audioSource.clip = clip;
            audioSource.loop = false;
            audioSource.volume = soundVolumes.ContainsKey(clip) ? soundVolumes[clip] : 1.0f;
            audioSource.Play();

            // ������� ������������� �� ���� ����� ���������� �����
            StartCoroutine(ReleaseAudioSourceAfterPlaying(audioSource));
        }
        else
        {
            Debug.LogWarning("Audio clip is missing!");
        }
    }

    // ����� ��� ��������� ���������� �������������� �� ����
    private AudioSource GetAvailableAudioSource()
    {
        AudioSource availableSource = audioSources.Find(source => !source.isPlaying);
        if (availableSource == null)
        {
            availableSource = gameObject.AddComponent<AudioSource>();
            audioSources.Add(availableSource);
        }
        return availableSource;
    }

    // �������� ��� ������������ �������������� ����� ���������� ���������������
    private IEnumerator ReleaseAudioSourceAfterPlaying(AudioSource audioSource)
    {
        while (audioSource.isPlaying)
        {
            yield return null; // ���� �� ��� ���, ���� ���� ������
        }
        audioSource.Stop();
        audioSource.clip = null;
    }

    // ����� ����� ��� ��������������� �����
    private void PlaySound(AudioClip clip, bool loop = false)
    {
        if (clip != null)
        {
            if (!mainAudioSource.isPlaying || loop)
            {
                mainAudioSource.clip = clip;
                mainAudioSource.loop = loop;
                mainAudioSource.volume = soundVolumes.ContainsKey(clip) ? soundVolumes[clip] : 1.0f;
                mainAudioSource.Play();

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
        if (mainAudioSource.isPlaying)
        {
            mainAudioSource.Stop();
        }
    }

    // ����� ��� ��������� ���� ������� ������
    public void StopAllSounds()
    {
        foreach (var source in audioSources)
        {
            if (source.isPlaying)
            {
                source.Stop();
            }
        }
        if (mainAudioSource.isPlaying)
        {
            mainAudioSource.Stop();
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