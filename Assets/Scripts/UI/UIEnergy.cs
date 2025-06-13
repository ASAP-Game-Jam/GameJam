using Assets.Scripts.Managers;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class UIEnergy : MonoBehaviour
    {
        [SerializeField] private TMP_Text energyText;
        private void Start()
        {
            LevelManager.StateManager.OnEnergyChanged += (i) => energyText.text = i.ToString();
        }
    }
}
