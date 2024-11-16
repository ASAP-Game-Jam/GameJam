using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// ����� ����������� ����������� �� ������
public partial class LevelHUD: MonoBehaviour {
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
    // �������� ������� ������
    public readonly UICommandQueue CommandQueue = new UICommandQueue();

    private void Start () {
        // ��� �������� ���������� �������� ������ �������
        // ��������� ����������� ������������ �������
        StartCoroutine(AsyncUpdate());
    }

    //// ������� ��� �������� ������� EnergyCommand � ���������� � � �������
    //public void CreateEnergyCommand (uint energy) {
    //    // ������ ����� ������� EnergyCommand
    //    EnergyCommand command = new EnergyCommand(energy);

    //    // ��������� ������� � �������
    //    CommandQueue.TryEnqueueCommand(command);
    //}

    // ����� ��������� ������ �� �������
    private IEnumerator AsyncUpdate () {
        while (true)
        {
            // ������� ����� ������� �� �������
            if (CommandQueue.TryDequeueCommand(out var command))
            {
                switch (command)
                {
                    // � ����������� �� ���� ������� ������� ����� ����������
                    // ���� ����� ��������� ����� ����������� ������, � ��� ��� ����� ������������� � ����� �����.
                    case UpdateScoreCommand updateScoreCommand:
                        {
                            // ������� �����
                            scoreText.text = $"Score: {updateScoreCommand.NewScore}";
                            break;
                        }
                }
            }

            // ����������� ����� �������, ��� �� ��������� ���������� 
            // �� ���� ����� ���������� ������
            yield return null;
        }
    }

    private void OnDestroy () {
        // ��� ����������� ���������� ��������� ������
        StopAllCoroutines();
    }
}
