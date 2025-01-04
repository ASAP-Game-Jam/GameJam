using Assets.Scripts.Interfaces;
using System;
using UnityEngine;

public class LevelManager : MonoBehaviour, ILevelManager
{
    public LevelHUD levelHUD;
    [SerializeField] uint startEnergy = 5;
    private LevelProgress progress;

    public uint Score
    {
        get => progress.LevelScore;
        set
        {
            progress.LevelScore = Math.Abs(
                Math.Max(progress.LevelScore,value) - Math.Min(progress.LevelScore, value)) > 100 
                ? 0 : value;
            levelHUD?.AddProgressCommand(progress.LevelScore);
        }
    }

    private void Awake()
    {
        progress = new LevelProgress();
    }
    private void Start()
    {
        levelHUD = FindObjectOfType<LevelHUD>();
        Score = startEnergy;
    }
}