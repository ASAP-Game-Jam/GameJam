using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Переменные для звуковых клипов
    public AudioClip mainTheme;
    public AudioClip blasterShot;
    public AudioClip cannonShot;
    public AudioClip tankShot;
    public AudioClip constructionSound;
    public AudioClip buttonClick;
    public AudioClip rocketLaunch;
    public AudioClip rocketExplosion;
    public AudioClip stickHit;

    // Словарь для хранения громкости каждого звука
    private Dictionary<AudioClip, float> soundVolumes;

    // Аудиоисточник для воспроизведения звуков
    private AudioSource audioSource;

    void Start()
    {
        // Инициализация аудиоисточника
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Инициализация громкости для каждого звука
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

        // Воспроизведение главной музыкальной темы на старте
        PlayMainTheme();
    }

    // Метод для воспроизведения главной музыкальной темы
    public void PlayMainTheme()
    {
        PlaySound(mainTheme, true); // Повторяется в цикле
    }

    // Метод для воспроизведения выстрела бластера
    public void PlayBlasterShot()
    {
        PlaySound(blasterShot);
    }

    // Метод для воспроизведения выстрела пушки
    public void PlayCannonShot()
    {
        PlaySound(cannonShot);
    }

    // Метод для воспроизведения выстрела танка
    public void PlayTankShot()
    {
        PlaySound(tankShot);
    }

    // Метод для воспроизведения звука строительства
    public void PlayConstructionSound()
    {
        PlaySound(constructionSound);
    }

    // Метод для воспроизведения звука нажатия клавиши
    public void PlayButtonClick()
    {
        PlaySound(buttonClick);
    }

    // Метод для воспроизведения запуска ракеты
    public void PlayRocketLaunch()
    {
        PlaySound(rocketLaunch);
    }

    // Метод для воспроизведения взрыва ракеты
    public void PlayRocketExplosion()
    {
        PlaySound(rocketExplosion);
    }

    // Метод для воспроизведения удара палкой
    public void PlayStickHit()
    {
        PlaySound(stickHit);
    }

    // Общий метод для воспроизведения звука
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

    // Метод для остановки текущего звука
    public void StopSound()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    // Метод для регулировки громкости конкретного звука
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