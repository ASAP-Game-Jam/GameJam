// Assets/Scripts/Managers/SellManager.cs
using Assets.Scripts.GameObjects.Entities;
using Assets.Scripts.Level;
using Assets.Scripts.Managers; // ваш namespace
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(LevelManager))]
public class SellManager : MonoBehaviour, IManager
{
    public static SellManager Instance { get; private set; }

    [Range(0f, 1f), Tooltip("Процент возврата при продаже")]
    public float refundPercent = 0.3f;

    private bool _isSelling;
    private UIManager _ui;

    public EStatusManager Status { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void Startup()
    {
        Status = EStatusManager.Started;
        _ui = LevelManager.UIManager;
    }

    public void Shutdown()
    {
        Status = EStatusManager.Shutdown;
        _isSelling = false;
    }

    /// <summary>Включить режим продажи (вызывается кнопкой)</summary>
    public void RequestSell()
    {
        if (_isSelling) return;
        _isSelling = true;
    }

    private void Update()
    {
        if (!_isSelling || Status != EStatusManager.Started)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            // не кликаем по UI
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            Vector2 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var hit = Physics2D.Raycast(wp, Vector2.zero);
            if (hit.collider != null &&
                hit.collider.TryGetComponent<Cell>(out var cell))
            {
                // продаём
                int refund = Mathf.RoundToInt(cell.Target.Cost * refundPercent);
                LevelManager.StateManager.ChangeEnergy(refund);

                cell.Clear(); // очищаем ячейку
            }

            // всегда выходим из режима после первого клика
            _isSelling = false;
        }
    }
}