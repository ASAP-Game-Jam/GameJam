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
    [SerializeField] private string textWin = "You win";
    [SerializeField] private string textDefead = "You defead";
    // ������ �� ��������� ������ � ������� ������������ ���� ������
    [SerializeField] private TMP_Text scoreText;

    [SerializeField] private Button tower1;
    [SerializeField] private Button tower2;
    [SerializeField] private Button tower3;
    [SerializeField] private Button tower4;
    [SerializeField] private Button tower5;

    [SerializeField] private TMP_Text timerText;

    [SerializeField] private Image healthImage;
    private float leftHealth, rightHealth, lengthHealth;

    [SerializeField] private Button menu;

    [SerializeField] private GameObject endGamePanel;
    [SerializeField] private Button restartGameButton;
    [SerializeField] private TMP_Text endText;

    private ILevelManager levelManager;
    RectTransform healthRectTransform;

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
        healthRectTransform = healthImage.gameObject.GetComponent<RectTransform>();
        RectTransform parentRectTransform = healthRectTransform.parent.GetComponent<RectTransform>();

        leftHealth = healthRectTransform.offsetMin.x;
        rightHealth = healthRectTransform.offsetMax.x;
        lengthHealth = parentRectTransform.rect.width - leftHealth - rightHealth;
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

    public void AddBaseHPCommand(uint maxHp,uint hp)
    {
        CommandQueue.TryEnqueueCommand(new EnemyBaseHPCommand(maxHp,hp));
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
                                endText.text = $" {endGame.BaseType switch { BaseType.EnemyBase => textWin, BaseType.TowerBase => textDefead, _=>"ERROR" }}";
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
                    case EnemyBaseHPCommand baseHP:
                        float newLength = (baseHP.HP / (float)baseHP.MaxHP - 1) * lengthHealth + rightHealth;
                        healthRectTransform.offsetMax = new Vector2(newLength, healthRectTransform.offsetMax.y);
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
