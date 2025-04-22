using Assets.Scripts.CustomEventArgs;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Interfaces.Tower;
using Assets.Scripts.Tower;
using System;
using System.Drawing;
using UnityEngine;

public class Cell : MonoBehaviour, ICell
{
    public event EventHandler OnCellClick;

    private ITower tower;

    [SerializeField] private bool isTaken;

    public Vector3 Position => transform.position;
    
    private void Start()
    {
    }
    private void OnClick()
    {
        OnCellClick?.Invoke(this, new EventCellArgs(this, isTaken));
    }

    public void CellTaken()
    {
        this.isTaken = true;
    }

    public void CellUnTaken()
    {
        this.isTaken = false;
        tower = null;
    }

    public void AddTower(ITower tower)
    {
        if (tower != null && IsEmpty())
        {
            CellTaken();
            this.tower = tower;
            tower.OnDestroy += (object sender, EventArgs e) => CellUnTaken();
        }
    }

    public bool IsEmpty()
    {
        return !this.isTaken;
    }

    public void RemoveTower()
    {
        if (tower != null && tower is Tower t)
        {
            t.HP = 0;
        }
    }

    public ITower GetTower()
    {
        return tower;
    }
}
