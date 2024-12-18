using Assets.Scripts.CustomEventArgs;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Interfaces.Enemy;
using Assets.Scripts.Interfaces.Tower;
using Assets.Scripts.Other;
using Assets.Scripts.Tower;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour, ILevelManager
{
    [SerializeField] private LevelHUD levelHUD;

    private LevelProgress progress;
    private Queue<IEnemy>[] countEnemiesForLine;
    private ISpawnerManager spawnerManager;
    [SerializeField] private GameObject[] towerPrefabs;
    [SerializeField] const uint startEnergy = 100;

    private ICellandITower cellTower;

    public uint Score => progress.LevelScore;

    private void Awake()
    {
        cellTower = new CellTowerConnect();
        progress = new LevelProgress();
        spawnerManager = null;// FindFirstObjectByType<SpawnerManager>();
        if (spawnerManager != null)
            spawnerManager.OnSpawn += SpawnNewEnemy;
        foreach (Tower tower in FindObjectsOfType<Tower>())
            tower.OnDestroy += TowerIsDestroy;
        foreach (var i in FindObjectsOfType<UIButton>())
        {
            i.SetLevelManager(this);
            i.OnCardMarked += TowerSelected;
            i.OnCardCancel += TowerUnSelected;
        }
        foreach (var i in FindObjectsOfType<Cell>())
        {
            i.OnCellClick += CellSelected;
        }
        levelHUD = FindObjectOfType<LevelHUD>();
        progress.LevelScore = startEnergy;
    }
    private void Start()
    {
        levelHUD.AddProgressCommand(Score);
    }
    private void SpawnNewEnemy(object sender, EventArgs args)
    {
        int index = -1;
        // if(args is Event e) index = e.index;
        if (index >= 0 && index < countEnemiesForLine.Length)
            countEnemiesForLine[index].Enqueue(null); // null �������� �������� �� args
    }
    public bool IsEnemyOnLine(uint index)
    {
        return countEnemiesForLine[index].Count > 0;
    }
    private void TowerSelected(object sender, EventArgs args)
    {
        if (args is EventMarkedArgs mark && sender is IUICard card)
        {
            cellTower.Card = card;
            cellTower.Type = mark.TowerType;
        }
    }
    private void TowerUnSelected(object sender, EventArgs args)
    {
        cellTower.Card = null;
    }
    private void CellSelected(object sender, EventArgs args)
    {
        if (sender is Cell cell && args is EventCellArgs cellArgs && !cellArgs.IsTaken)
        {
            cellTower.Cell = cell;
            if (cellTower.Card != null && cellTower != null)
            {
                SpawnTower();
            }
        }
    }
    private void SpawnTower()
    {
        if (cellTower.Cell != null && cellTower.Card != null && towerPrefabs.Length > 0)
        {
            if (cellTower.Cell is Cell cell)
            {
                var obj = Instantiate(towerPrefabs[(int)cellTower.Type], cell.gameObject.transform);
                cell.AddTower(obj.GetComponent<ITower>());
                Debug.Log("SpawnTower");
            }
        }
    }
    private void TowerIsDestroy(object sender, EventArgs args)
    {
    }
}