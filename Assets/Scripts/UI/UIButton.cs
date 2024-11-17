using Assets.Scripts.CustomEventArgs;
using Assets.Scripts.Interfaces;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIButton : MonoBehaviour, IUICard
{
    [SerializeField] private TowerType towerType;

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
        OnCardMarked?.Invoke(this, new EventMarkedArgs(towerType));
    }
}
