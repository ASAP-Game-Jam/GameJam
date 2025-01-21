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

    // Пул аудиоисточников для асинхронного воспроизведения
    private List<AudioSource> audioSources = new List<AudioSource>();

    // Аудиоисточник для воспроизведения звуков
    private AudioSource mainAudioSource;

    void Start()
    {
        // Инициализация основного аудиоисточника
        mainAudioSource = gameObject.AddComponent<AudioSource>();

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
        PlaySoundAsync(blasterShot);
    }

    // Метод для воспроизведения выстрела пушки
    public void PlayCannonShot()
    {
        PlaySoundAsync(cannonShot);
    }

    // Метод для воспроизведения выстрела танка
    public void PlayTankShot()
    {
        PlaySoundAsync(tankShot);
    }

    // Метод для воспроизведения звука строительства
    public void PlayConstructionSound()
    {
        PlaySoundAsync(constructionSound);
    }

    // Метод для воспроизведения звука нажатия клавиши
    public void PlayButtonClick()
    {
        PlaySoundAsync(buttonClick);
    }

    // Метод для воспроизведения запуска ракеты
    public void PlayRocketLaunch()
    {
        PlaySoundAsync(rocketLaunch);
    }

    // Метод для воспроизведения взрыва ракеты
    public void PlayRocketExplosion()
    {
        PlaySoundAsync(rocketExplosion);
    }

    // Метод для воспроизведения удара палкой
    public void PlayStickHit()
    {
        PlaySoundAsync(stickHit);
    }

    // Метод для воспроизведения звука асинхронно (не прерывая другие звуки)
    public void PlaySoundAsync(AudioClip clip)
    {
        if (clip != null)
        {
            AudioSource audioSource = GetAvailableAudioSource();
            audioSource.clip = clip;
            audioSource.loop = false;
            audioSource.volume = soundVolumes.ContainsKey(clip) ? soundVolumes[clip] : 1.0f;
            audioSource.Play();

            // Удаляем аудиоисточник из пула после завершения клипа
            StartCoroutine(ReleaseAudioSourceAfterPlaying(audioSource));
        }
        else
        {
            Debug.LogWarning("Audio clip is missing!");
        }
    }

    // Метод для получения доступного аудиоисточника из пула
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

    // Корутина для освобождения аудиоисточника после завершения воспроизведения
    private IEnumerator ReleaseAudioSourceAfterPlaying(AudioSource audioSource)
    {
        while (audioSource.isPlaying)
        {
            yield return null; // Ждем до тех пор, пока звук играет
        }
        audioSource.Stop();
        audioSource.clip = null;
    }

    // Общий метод для воспроизведения звука
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

    // Метод для остановки текущего звука
    public void StopSound()
    {
        if (mainAudioSource.isPlaying)
        {
            mainAudioSource.Stop();
        }
    }

    // Метод для остановки всех текущих звуков
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