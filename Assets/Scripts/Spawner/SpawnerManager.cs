using Assets.Scripts.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager: MonoBehaviour, ISpawnerManager {
    public event EventHandler OnSpawn;

    [SerializeField]
    private SpawnPoint[] spawnPoints;
    
    //
}
