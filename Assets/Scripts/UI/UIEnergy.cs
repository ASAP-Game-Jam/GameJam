using Assets.Scripts.Managers;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class UIEnergy : MonoBehaviour
    {
        [SerializeField] private TMP_Text energyText;
        private void Awake()
        {
            StartCoroutine(Connect());
        }
        private IEnumerator Connect()
        {
            float time = 3f;
            while (LevelManager.StateManager == null && time >= 0)
            {
                time -= Time.deltaTime;
                yield return null;
            }
            if (LevelManager.StateManager == null)
            {
                Debug.LogError("StateManager is not found!");
                yield break;
            }
            LevelManager.StateManager.OnEnergyChanged += (i) => energyText.text = i.ToString();
        }
    }
}
