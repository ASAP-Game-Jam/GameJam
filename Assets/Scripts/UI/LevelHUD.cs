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

    // �������� ������� ������
    public readonly UICommandQueue CommandQueue = new UICommandQueue();

    private void Start()
    {
        // ��� �������� ���������� �������� ������ �������
        // ��������� ����������� ������������ �������
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
                            
                            break;
                        }
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
