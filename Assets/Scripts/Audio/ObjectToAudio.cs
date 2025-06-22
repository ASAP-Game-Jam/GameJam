using Assets.Scripts.GameObjects;
using Assets.Scripts.GameObjects.Attacks;
using Assets.Scripts.Managers;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct EnemyAudioMapping
{
    public EnemyType enemyType;
    public SoundType attackSound;
}

[Serializable]
public struct AllyAudioMapping
{
    public AllyType allyType;
    public SoundType attackSound;
}

public class ObjectToAudio : MonoBehaviour
{
    [Header("Mappings")]
    [SerializeField] private EnemyAudioMapping[] enemyMappings;
    [SerializeField] private AllyAudioMapping[] allyMappings;

    [Header("UI Buttons")]
    [SerializeField] private Button[] buttons;

    private void Start()
    {
        // подписка на появление юнитов
        LevelManager.EnemyManager.OnEnemySpawned += RegisterEnemySound;
        LevelManager.AllyManager.OnSpawned += RegisterAllySound;

        // подписка на кнопки
        foreach (var b in buttons)
            b.onClick.AddListener(OnButtonClick);
    }

    private void OnDisable()
    {
        LevelManager.EnemyManager.OnEnemySpawned -= RegisterEnemySound;
        LevelManager.AllyManager.OnSpawned -= RegisterAllySound;

        foreach (var b in buttons)
            b.onClick.RemoveListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        AudioEvents.Play(SoundType.ButtonClick);
    }

    private void RegisterEnemySound(EnemyType type, GameObject go)
    {
        var map = enemyMappings.FirstOrDefault(m => m.enemyType == type);
        if (map.attackSound != default)
            SubscribeAttack(go, map.attackSound);
    }

    private void RegisterAllySound(AllyType type, GameObject go)
    {
        // звук строительства прокидываем на всех
        AudioEvents.Play(SoundType.ConstructionSound);

        var map = allyMappings.FirstOrDefault(m => m.allyType == type);
        if (map.attackSound != default)
            SubscribeAttack(go, map.attackSound);
    }

    private void SubscribeAttack(GameObject go, SoundType sound)
    {
        var atk = go.GetComponent<BasicAttack>();
        if (atk != null)
            atk.OnAttacking += () => AudioEvents.Play(sound);
    }
}