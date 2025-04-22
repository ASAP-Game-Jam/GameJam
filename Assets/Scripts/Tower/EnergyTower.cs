using Assets.Scripts.CustomEventArgs;
using Assets.Scripts.Interfaces.Tower;
using System;
using UnityEngine;

namespace Assets.Scripts.Tower
{
    public class EnergyTower : MonoBehaviour, IEnergyTower
    {
        [SerializeField] private float cooldown = 3f;
        private float timeActivate;
        [SerializeField]private uint energyCount = 2;
        public float Cooldown => cooldown;
        public uint EnergyCount => energyCount;
        public event EventHandler OnActivated;

        private void Start()
        {
            timeActivate = cooldown;
        }
        private void FixedUpdate()
        {
            if (timeActivate <= 0)
            {
                OnActivated?.Invoke(this, new EventEnergyArgs(energyCount));
                timeActivate = cooldown;
            }
            else
                timeActivate -= Time.fixedDeltaTime;
        }

    }
}
