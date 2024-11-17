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

    private bool isTaken;

    private void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(OnClick);
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
        CellTaken();
        this.tower = tower;
        tower.OnDestroy += (object sender, EventArgs e) => CellUnTaken();
    }
}
