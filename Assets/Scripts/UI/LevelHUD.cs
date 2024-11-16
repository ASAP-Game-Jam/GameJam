using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Класс управляющий интерфейсом на уровне
public partial class LevelHUD: MonoBehaviour {
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

    // Создадим очередь команд
    public readonly UICommandQueue CommandQueue = new UICommandQueue();
    
    private void Start () {
        // При создании интерфейса запустим задачу которая
        // позволяет ассинхронно обрабатывать команды
        StartCoroutine(AsyncUpdate());
    }

    // Функция для добавления команды энергии
    public void AddEnergyCommand (uint energy) {
        var energyCommand = new EnergyCommand(energy);
        CommandQueue.TryEnqueueCommand(energyCommand);
    }

    // Функция для добавления команды прогресса
    public void AddProgressCommand (uint progress) {
        var progressCommand = new ProgressCommand(progress);
        CommandQueue.TryEnqueueCommand(progressCommand);
    }

    // Метод обработки команд из очереди
    private IEnumerator AsyncUpdate () {
        while (true)
        {
            // Получим новую команду из очереди
            if (CommandQueue.TryDequeueCommand(out var command))
            {
                switch (command)
                {
                    // В зависимости от типа команды выберем нужны обработчик
                    // Сюда можно добавлять любые обработчики команд, и они все будут сгруппированы в одном месте.
                    case UpdateScoreCommand updateScoreCommand:
                        {
                            // Обновим текст
                            scoreText.text = $"Score: {updateScoreCommand.NewScore}";
                            break;
                        }
                }
            }

            // Обязательно нужно сказать, что мы закончили обновление 
            // на этом цикле обновления движка
            yield return null;
        }
    }

    private void OnDestroy () {
        // При уничтожении интерфейса остановим задачу
        StopAllCoroutines();
    }
}
