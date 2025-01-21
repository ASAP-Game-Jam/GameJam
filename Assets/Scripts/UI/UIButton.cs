using Assets.Scripts.CustomEventArgs;
using Assets.Scripts.Interfaces;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIButton : MonoBehaviour, IUICard
{
    [SerializeField] private TowerType towerType;
    [SerializeField] private uint cost;
    [SerializeField] private TMP_Text text;
    public uint Cost
    {
        get => cost;
        set
        {
            cost = value;
            if (text)
                text.text = value.ToString();
        }
    }

    public event EventHandler OnCardMarked;
    public event EventHandler OnCardCancel;

    private void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(OnClick);
        if(text==null)text = GetComponentInChildren<TMP_Text>();
        if (text != null) Cost = uint.Parse(text.text);
    }
    private void OnCancel()
    {
        OnCardCancel?.Invoke(this, EventArgs.Empty);
    }
    private void OnClick()
    {
        OnCardMarked?.Invoke(this, new EventMarkedArgs(towerType, cost));
    }
}
