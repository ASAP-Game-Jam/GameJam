using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // ѕеременные дл€ звуковых клипов
    public AudioClip mainTheme;
    public AudioClip blasterShot;
    public AudioClip cannonShot;
    public AudioClip tankShot;
    public AudioClip constructionSound;
    public AudioClip buttonClick;

    // јудиоисточник дл€ воспроизведени€ звуков
    private AudioSource audioSource;

    void Start()
    {
        // »нициализаци€ аудиоисточника
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // ¬оспроизведение главной музыкальной темы на старте
        PlayMainTheme();
    }

    // ћетод дл€ воспроизведени€ главной музыкальной темы
    public void PlayMainTheme()
    {
        PlaySound(mainTheme, true); // ѕовтор€етс€ в цикле
    }

    // ћетод дл€ воспроизведени€ выстрела бластера
    public void PlayBlasterShot()
    {
        PlaySound(blasterShot);
    }

    // ћетод дл€ воспроизведени€ выстрела пушки
    public void PlayCannonShot()
    {
        PlaySound(cannonShot);
    }

    // ћетод дл€ воспроизведени€ выстрела танка
    public void PlayTankShot()
    {
        PlaySound(tankShot);
    }

    // ћетод дл€ воспроизведени€ звука строительства
    public void PlayConstructionSound()
    {
        PlaySound(constructionSound);
    }

    // ћетод дл€ воспроизведени€ звука нажати€ клавиши
    public void PlayButtonClick()
    {
        PlaySound(buttonClick);
    }

    // ќбщий метод дл€ воспроизведени€ звука
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

    // ћетод дл€ остановки текущего звука
    public void StopSound()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
