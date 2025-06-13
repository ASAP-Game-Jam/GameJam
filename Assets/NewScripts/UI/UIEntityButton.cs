using Assets.Scripts.Managers;
using Assets.Scripts.GameObjects;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class UIEntityButton : MonoBehaviour
    {
        [SerializeField] public AllyType AlliedType;
        [SerializeField] public int Cost = 10;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(Select);
        }
        public void Select() => LevelManager.UIManager.Select(this);
        public void UnSelect()
        {

        }
    }
}
