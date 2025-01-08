using Assets.Scripts.CustomEventArgs;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Interfaces.Tower;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour, ICell
{
    public event EventHandler OnCellClick;

    private ITower tower;

    [SerializeField] private bool isTaken;

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
}
