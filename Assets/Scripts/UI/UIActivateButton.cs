using Assets.Scripts.GameObjects;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class UIActivateButton : MonoBehaviour
    {
        [SerializeField] public ActivateType ActivateType;
        [SerializeField] public int Cost = 10;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(Select);
        }
        public void Select()
        {

        }
        public void UnSelect()
        {

        }
    }
}
