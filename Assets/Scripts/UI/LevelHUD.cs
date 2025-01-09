using Assets.Scripts.Interfaces;
using Assets.Scripts.Other;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Класс управляющий интерфейсом на уровне
public partial class LevelHUD : MonoBehaviour
{
    // Ссылка на текстовый виджет в котором отображается счет игрока
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

    // Создадим очередь команд
    public readonly UICommandQueue CommandQueue = new UICommandQueue();

    private void Start()
    {
        // При создании интерфейса запустим задачу которая
        // позволяет ассинхронно обрабатывать команды
        endGamePanel.SetActive(false);
        restartGameButton?.onClick.AddListener(AddRestartGameCommand);
        menu?.onClick.AddListener(AddMainMenuCommand);
        levelManager = FindObjectOfType<LevelManager>();
        StartCoroutine(AsyncUpdate());
    }

    // Функция для добавления команды прогресса
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

    // Метод обработки команд из очереди
    private IEnumerator AsyncUpdate()
    {
        while (true)
        {
            // Получим новую команду из очереди
            if (CommandQueue.TryDequeueCommand(out var command))
            {
                switch (command)
                {
                    // В зависимости от типа команды выберем нужны обработчик
                    // Сюда можно добавлять любые обработчики команд, и они все будут сгруппированы в одном месте.
                    case ProgressCommand progressCommand:
                        {
                            // Обновим текст
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

            // Обязательно нужно сказать, что мы закончили обновление 
            // на этом цикле обновления движка
            yield return null;
        }
    }

    private void OnDestroy()
    {
        // При уничтожении интерфейса остановим задачу
        StopAllCoroutines();
    }
}
