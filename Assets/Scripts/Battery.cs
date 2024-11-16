using UnityEngine;

internal class Battery: MonoBehaviour {
    //����������� �� ������, �� ������� ����� ����������� ���� ����� ����� �������� �� ����
    [SerializeField] private int score = 5;

    // �������� �� ����� ����
    private LevelManager levelManager;

    // ������� ��� ��������� ���������
    public void SetLevelManager (LevelManager levelManager) {
        this.levelManager = levelManager;
    }

    
}

/*��� ���� ����� Battery ������� �����:

1.������� ������
2. �������� �� ���� ���������� Coins � Collider
3. � ��������� ��������� ���� IsTrigger*/