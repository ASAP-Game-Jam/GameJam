using Assets.Scripts.CustomEventArgs;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Interfaces.Tower;
using System;
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
        if (tower != null)
            tower.Activate();
        this.isTaken = true;
    }

    public void CellUnTaken()
    {
        this.isTaken = false;
        if (tower?.HP > 0)
            tower.Deactivate();
        tower = null;
    }

    public void AddTower(ITower tower)
    {
        if (tower != null && IsEmpty())
        {
            this.tower = tower;
            CellTaken();
            tower.OnDestroy += (object sender, EventArgs e) => CellUnTaken();
        }
    }

    public bool IsEmpty()
    {
        return !this.isTaken;
    }

    public ITower GetTower()
    {
        return tower;
    }
}
