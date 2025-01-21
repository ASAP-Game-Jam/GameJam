using Assets.Scripts.CustomEventArgs;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Tower;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIButton : MonoBehaviour, IUICard
{
    [SerializeField] private TowerType towerType;
    [SerializeField] private uint cost;
    [SerializeField] private TMP_Text text;
    [SerializeField] private TMP_Text cooldownText;
    [SerializeField] private GameObject fog;

    [SerializeField] private float cooldown = 5f;
    private float time;
    private bool isActive = true;

    private ISpawnerManager spawnerManager;

    public float Cooldown
    {
        get => cooldown;
        set => cooldown = value > 0 ? value : 0;
    }

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
        if (text == null) text = GetComponentInChildren<TMP_Text>();
        if (text != null) Cost = uint.Parse(text.text);
        if (fog != null)
            fog.SetActive(false);
        spawnerManager = FindObjectOfType<TowerSpawnManager>();
        if (spawnerManager != null)
            spawnerManager.OnSpawn += OnSpawnHandler;
    }
    private void OnCancel()
    {
        OnCardCancel?.Invoke(this, EventArgs.Empty);
    }
    private void OnClick()
    {
        if (isActive)
            OnCardMarked?.Invoke(this, new EventMarkedArgs(towerType, cost));
    }
    private void OnSpawnHandler(object sender, EventArgs e)
    {
        if (e is EventTowerSpawnArgs args && args.TowerType == this.towerType)
            Block();
    }
    public void Block()
    {
        time = cooldown;
        isActive = false;
        if (fog != null)
            fog?.SetActive(!isActive);
    }

    private void Update()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
            if (cooldownText != null)
                cooldownText.text = $"{time:F2}";
        }
        else
        {
            isActive = true;
            if (fog != null)
                fog?.SetActive(!isActive);
        }
    }
}
