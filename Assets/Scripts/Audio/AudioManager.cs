using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SoundLimit
{
    public SoundType type;
    [Tooltip("0 = без ограничений")]
    public int maxSimultaneous;
}
public enum SoundType
{
    MainTheme, BlasterShot, BombLaunch, BombExplosion, CannonShot, TankShot,
    ConstructionSound, ButtonClick, RocketLaunch,
    RocketExplosion, StickHit
}

public static class AudioEvents
{
    public static event Action<SoundType> OnPlaySound;
    public static void Play(SoundType type) => OnPlaySound?.Invoke(type);
}

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [Header("Main")]
    public AudioClip mainTheme;
    public AudioClip constructionSound;
    public AudioClip buttonClick;

    [Header("Ally")]
    public AudioClip cannonShot;
    public AudioClip rocketLaunch;
    public AudioClip rocketExplosion;
    public AudioClip bombLaunch;
    public AudioClip bombExplosion;

    [Header("Enemy")]
    public AudioClip blasterShot;
    public AudioClip tankShot;
    public AudioClip stickHit;

    [Header("Sound Limits (0 = no limit)")]
    [SerializeField] private SoundLimit[] soundLimits;

    private Dictionary<SoundType, AudioClip> clipMap;
    private Dictionary<AudioClip, float> soundVolumes;
    private List<AudioSource> pool;
    private AudioSource musicSource;

    // Новые поля для лимитов и учёта активных источников
    private Dictionary<SoundType, int> _maxSimultaneous;
    private Dictionary<SoundType, List<AudioSource>> _activeSources;

    private void Awake()
    {
        // 1. Заполняем карту типа → клип
        clipMap = new Dictionary<SoundType, AudioClip>
        {
            { SoundType.MainTheme,        mainTheme },
            { SoundType.ConstructionSound,constructionSound },
            { SoundType.ButtonClick,      buttonClick },
            { SoundType.CannonShot,       cannonShot },
            { SoundType.RocketLaunch,     rocketLaunch },
            { SoundType.RocketExplosion,  rocketExplosion },
            { SoundType.BlasterShot,      blasterShot },
            { SoundType.TankShot,         tankShot },
            { SoundType.StickHit,         stickHit }
        };

        // 2. Инициализируем громкости
        soundVolumes = new Dictionary<AudioClip, float>();
        foreach (var kv in clipMap)
            if (kv.Value != null)
                soundVolumes[kv.Value] = 1f;
        if (mainTheme != null) soundVolumes[mainTheme] = 0.6f;

        // 3. Пул и источник музыки
        pool = new List<AudioSource>();
        musicSource = GetComponent<AudioSource>();
        musicSource.playOnAwake = false;

        // 4. Собираем лимиты
        _maxSimultaneous = new Dictionary<SoundType, int>();
        foreach (var sl in soundLimits)
            _maxSimultaneous[sl.type] = sl.maxSimultaneous;

        // 5. Готовим хранилище активных источников по типу
        _activeSources = new Dictionary<SoundType, List<AudioSource>>();
        foreach (SoundType t in Enum.GetValues(typeof(SoundType)))
            _activeSources[t] = new List<AudioSource>();

        // 6. Подписываемся на канал событий
        AudioEvents.OnPlaySound += HandlePlaySound;
    }

    private void Start()
    {
        AudioEvents.Play(SoundType.MainTheme);
    }

    private void OnDestroy()
    {
        AudioEvents.OnPlaySound -= HandlePlaySound;
    }

    private void HandlePlaySound(SoundType type)
    {
        if (type == SoundType.MainTheme)
            PlayMusic();
        else
            PlaySFX(type);
    }

    private void PlayMusic()
    {
        if (!clipMap.TryGetValue(SoundType.MainTheme, out var clip) || clip == null)
            return;

        musicSource.clip = clip;
        musicSource.volume = soundVolumes[clip];
        musicSource.loop = true;
        if (!musicSource.isPlaying)
            musicSource.Play();
    }

    private void PlaySFX(SoundType type)
    {
        if (!clipMap.TryGetValue(type, out var clip) || clip == null)
            return;

        var activeList = _activeSources[type];
        _maxSimultaneous.TryGetValue(type, out int maxAllowed);

        // Если лимит задан и мы уже достигли — пропускаем новый звук
        if (maxAllowed > 0 && activeList.Count >= maxAllowed)
            return;

        // Находим свободный AudioSource в общем пуле
        AudioSource src = pool.Find(s => !s.isPlaying);
        if (src == null)
        {
            src = gameObject.AddComponent<AudioSource>();
            pool.Add(src);
        }

        src.clip = clip;
        src.volume = soundVolumes[clip];
        src.loop = false;
        src.Play();

        // Регистрируем его как активный
        activeList.Add(src);
        StartCoroutine(ReleaseWhenDone(type, src));
    }

    private IEnumerator ReleaseWhenDone(SoundType type, AudioSource src)
    {
        yield return new WaitWhile(() => src.isPlaying);
        src.clip = null;

        // Убираем из списка активных
        _activeSources[type].Remove(src);
    }
}