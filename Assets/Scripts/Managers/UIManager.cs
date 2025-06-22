using Assets.Scripts.GameObjects;
using Assets.Scripts.UI;
using System;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class UIManager : MonoBehaviour, IManager
    {
        public event Action<(AllyType Type, int Cost)> OnSelected;
        public EStatusManager Status { get; private set; }
        private UIEntityButton UIButton;
        public (AllyType Type, int Cost) Unit => (UIButton?.AlliedType ?? AllyType.None, UIButton?.Cost ?? 0);
        public bool IsSelect => UIButton != null;
        private void Update()
        {
            if(Input.GetMouseButtonDown(1) && Status == EStatusManager.Started)
            {
                Debug.Log("Unselecting unit on right click");
                UnSelect(UIButton?.AlliedType ?? AllyType.None, null);
            }
        }
        public void Shutdown()
        {
            LevelManager.AllyManager.OnSpawned -= UnSelect;
            Status = EStatusManager.Shutdown;
        }

        public void Startup()
        {
            LevelManager.AllyManager.OnSpawned += UnSelect;
            Status = EStatusManager.Started;
        }
        private void UnSelect(AllyType type, GameObject obj)
        {
//#if !UNITY_EDITOR
            UIButton?.ActivateCooldown();
            UIButton = null;
            OnSelected?.Invoke((AllyType.None, 0));
            //#endif
        }
        public void Select(UIEntityButton button)
        {
            UIButton?.UnSelect();
            UIButton = button;//UIButton == button ? null : button;
            OnSelected?.Invoke(Unit);
        }
    }
}
