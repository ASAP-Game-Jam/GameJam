using Assets.Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class MenuButton : MonoBehaviour
    {
        [SerializeField] private string menuSceneName = "MainMenu";

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(GoToMenu);
        }

        private void GoToMenu()
        {
            LevelManager.GameManager.LoadMenuScene(menuSceneName);
        }
    }
}
