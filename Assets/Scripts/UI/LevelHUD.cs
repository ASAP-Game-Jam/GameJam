using Assets.Scripts.Interfaces;
using Assets.Scripts.Other;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// ����� ����������� ����������� �� ������
public partial class LevelHUD : MonoBehaviour
{
    // ������ �� ��������� ������ � ������� ������������ ���� ������
    [SerializeField] private TMP_Text scoreText;

    [SerializeField] private Button tower1;
    [SerializeField] private Button tower2;
    [SerializeField] private Button tower3;
    [SerializeField] private Button tower4;
    [SerializeField] private Button tower5;

    [SerializeField] private TMP_Text timerText;

    [SerializeField] private Image healthImage;

    [SerializeField] private Button menu;

    [SerializeField] private GameObject[] healthDivs;

    [SerializeField] private GameObject endGamePanel;
    [SerializeField] private Button restartGameButton;
    [SerializeField] private TMP_Text endText;

    private ILevelManager levelManager;

    // �������� ������� ������
    public readonly UICommandQueue CommandQueue = new UICommandQueue();

    private void Start()
    {
        // ��� �������� ���������� �������� ������ �������
        // ��������� ����������� ������������ �������
        endGamePanel.SetActive(false);
        restartGameButton?.onClick.AddListener(AddRestartGameCommand);
        menu?.onClick.AddListener(AddMainMenuCommand);
        levelManager = FindObjectOfType<LevelManager>();
        StartCoroutine(AsyncUpdate());
    }

    // ������� ��� ���������� ������� ���������
    public void AddProgressCommand(uint progress)
    {
        var progressCommand = new ProgressCommand(progress);
        CommandQueue.TryEnqueueCommand(progressCommand);
    }

    public void AddEndOfTheGameCommand(BaseType winnerBase)
    {
        CommandQueue.TryEnqueueCommand(new EndOfTheGameCommand(winnerBase));
    }

    public void AddRestartGameCommand()
    {
        CommandQueue.TryEnqueueCommand(new RestartCommand());
    }

    public void AddMainMenuCommand()
    {
        CommandQueue.TryEnqueueCommand(new MainMenuCommand());
    }

    // ����� ��������� ������ �� �������
    private IEnumerator AsyncUpdate()
    {
        while (true)
        {
            // ������� ����� ������� �� �������
            if (CommandQueue.TryDequeueCommand(out var command))
            {
                switch (command)
                {
                    // � ����������� �� ���� ������� ������� ����� ����������
                    // ���� ����� ��������� ����� ����������� ������, � ��� ��� ����� ������������� � ����� �����.
                    case ProgressCommand progressCommand:
                        {
                            // ������� �����
                            scoreText.text = $"{progressCommand.Progress}";
                            break;
                        }
                    case EndOfTheGameCommand endGame:
                        {
                            if(endText != null)
                            {
                                endText.text = $"You are {endGame.BaseType switch { BaseType.EnemyBase => "Win", BaseType.TowerBase => "Defeat", _=>"ERROR" }}";
                            }
                            endGamePanel.SetActive(true);
                            break;
                        }
                    case MainMenuCommand:
                        levelManager.OpenMainMenu();
                        break;
                    case RestartCommand:
                        levelManager.ReloadGame();
                        break;
                }
            }

            // ����������� ����� �������, ��� �� ��������� ���������� 
            // �� ���� ����� ���������� ������
            yield return null;
        }
    }

    private void OnDestroy()
    {
        // ��� ����������� ���������� ��������� ������
        StopAllCoroutines();
    }
}
