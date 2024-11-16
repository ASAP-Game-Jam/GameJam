using Assets.Scripts.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell: MonoBehaviour, ICell {
    public event EventHandler OnCellClick;
    private bool isTaken;

    private void Start () 
    {
        this.GetComponent<Button>().onClick.AddListener(OnClick);
    }
    private void OnClick() {
        OnCellClick?.Invoke(this, EventArgs.Empty);
        Debug.Log("Clicked");
    }

}
