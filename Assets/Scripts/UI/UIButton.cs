using Assets.Scripts.CustomEventArgs;
using Assets.Scripts.Interfaces;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIButton : MonoBehaviour, IUICard
{
    [SerializeField] private TowerType towerType;
    [SerializeField] private uint cost;
    [SerializeField] private ILevelManager levelManager;
    public uint Cost => cost;

    public event EventHandler OnCardMarked;
    public event EventHandler OnCardCancel;

    private void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(OnClick);
    }
    private void OnCancel()
    {
        OnCardCancel?.Invoke(this, EventArgs.Empty);
    }
    private void OnClick()
    {
        if (levelManager.Score >= cost)
        {
            OnCardMarked?.Invoke(this, new EventMarkedArgs(towerType,cost));
            //throw new InvalidImplementationException();
        }
    }
    public void SetLevelManager(ILevelManager levelManager) => this.levelManager = levelManager;
}
