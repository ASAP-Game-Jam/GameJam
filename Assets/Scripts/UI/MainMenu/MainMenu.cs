using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.UI.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField]
        private Button startGameButton;

        [SerializeField]
        private Button exitGameButton;

        private void Awake()
        {
            startGameButton?.onClick.AddListener(StartGame);
            exitGameButton?.onClick.AddListener(ExitGame);
        }

        private void StartGame()
        {
            SceneManager.LoadScene("RScene");
        }

        private void ExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quiot();
#endif
        }
    }
}
