using Assets.Scripts.GameObjects.Entities;
using System;
using UnityEngine;

namespace Assets.Scripts.Level
{
    public class Cell : MonoBehaviour
    {
        public bool IsAnchor = true;
        public bool IsEmpty { get; private set; } = true;
        private IBasicEntity target;
        public IBasicEntity Target
        {
            get => target;
        }
        public void AddObject(IBasicEntity obj)
        {
            if (!IsEmpty)
            {
                Debug.LogWarning($"Cell is fill");
                return;
            }
            if (obj == null)
                throw new ArgumentNullException("obj is null");
            if (IsAnchor)
                Select(obj);
            target.OnDestroyed += (_) => Unselect();
        }
        private void Select(IBasicEntity unitControl)
        {
            IsEmpty = false;
            target = unitControl;
        }
        private void Unselect()
        {
            IsEmpty = true;
            target = null;
        }
        public void Clear()
        {
            if (target != null)
                target.TakeDamage(target.HP);
            Unselect();
        }
    }
}
