using Assets.Scripts.Interfaces;
using Assets.Scripts.Interfaces.Enemy;
using Assets.Scripts.Interfaces.Tower;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour, ILevelManager
{
    [SerializeField] private LevelHUD levelHUD;

    private LevelProgress progress;
    private Queue<IEnemy>[] countEnemiesForLine;
    private ISpawnerManager spawnerManager;

    private void Awake()
    {
        progress = new LevelProgress();
        spawnerManager = null;// FindFirstObjectByType<SpawnerManager>();
        if (spawnerManager != null)
            spawnerManager.OnSpawn += SpawnNewEnemy;
        //foreach (ITowerAttack tower in FindAnyObjectsByType<ITowerAttack>())
        //    tower.SetLevelManager(this);
    }
    private void SpawnNewEnemy(object sender, EventArgs args)
    {
        int index = -1;
        // if(args is Event e) index = e.index;
        if (index >= 0 && index < countEnemiesForLine.Length)
            countEnemiesForLine[index].Enqueue(null); // null заменить объектом из args
    }
    public bool IsEnemyOnLine(uint index)
    {
        return countEnemiesForLine[index].Count > 0;
    }
}