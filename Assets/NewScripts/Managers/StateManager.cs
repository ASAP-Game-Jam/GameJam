using System;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class StateManager : MonoBehaviour, IManager
    {
        public event Action<int> OnEnergyChanged;
        [SerializeField] private int _energy = 15;
        public EStatusManager Status { get; private set; }
        public int Energy => _energy;

        public void Startup()
        {
            Debug.Log("StateManager manager starting...");
            Debug.Log($"Progress: {_energy}");
            OnEnergyChanged?.Invoke(_energy);
            Status = EStatusManager.Started;
        }

        public void Shutdown()
        {
            Debug.Log("StateManager manager shutdown...");
            Status = EStatusManager.Shutdown;
        }
        public bool IsEnergy(int energy) => _energy - energy >= 0;
        public void ChangeEnergy(int energy)
        {
            if (energy >= 0)
                _energy += energy;
            else if (IsEnergy(-energy))
                _energy += energy;
            else
                Debug.Log("ChangeEnergy");

            OnEnergyChanged?.Invoke(_energy);
        }   
    }
}
