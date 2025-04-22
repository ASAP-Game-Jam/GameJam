using Assets.Scripts.Interfaces;
using Assets.Scripts.Interfaces.Tower;
using Assets.Scripts.UI;
using System;
using UnityEngine;

namespace Assets.Scripts.Tower
{
    class TowerModifyManager : MonoBehaviour
    {
        public LayerMask tileMask;
        public float percentGetToDestroy;
        private UIModifyButton _selectButton;
        private LevelManager _levelManager;
        private void Start()
        {
            if (percentGetToDestroy < 0 || percentGetToDestroy > 1)
                percentGetToDestroy = 0.2f;
            _levelManager = FindFirstObjectByType<LevelManager>();
            // Подключить кнопку
            UIModifyButton button = FindFirstObjectByType<UIModifyButton>();
            if (button != null)
            {
                button.OnSelect += (object sender, EventArgs args) =>
                {
                    if (_selectButton != null) _selectButton.Cancel();
                    _selectButton = (UIModifyButton)sender;
                };
                button.OnCancel += (object sender, EventArgs args) =>
                {
                    _selectButton = null;
                };
            }
            foreach (var i in FindObjectsOfType<UIButton>())
                i.OnCardMarked += (object s, EventArgs e) => { Clear(); };
        }

        private void Update()
        {
            // Проверить клик на ячейке
            RaycastHit2D hit = Physics2D.Raycast(
                Camera.main.ScreenToWorldPoint(Input.mousePosition),
                Vector2.zero,
                Mathf.Infinity,
                tileMask
                );
            // "Проверить выбранную кнопку"
            if (_selectButton != null)
            {
                // "Проверить клик"
                if (hit.collider)
                {
                    ICell cell = hit.collider.GetComponent<ICell>();
                    if (!cell.IsEmpty() && Input.GetAxis("Fire1") == 1)
                    {
                        ITower tower = cell.GetTower();
                        _levelManager.Score += (uint)((float)tower.Cost * percentGetToDestroy);
                        cell.RemoveTower();
                        _selectButton.Cancel();
                    }
                }
            }
        }
        public void Clear()
        {
            _selectButton?.Cancel();
        }
    }
}
