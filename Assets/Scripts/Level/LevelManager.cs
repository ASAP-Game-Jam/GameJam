using UnityEngine;

public class LevelManager: MonoBehaviour {
    [SerializeField] private LevelHUD levelHUD;

    private LevelProgress progress;

    private void Awake () {
        progress = new LevelProgress();

        // ������ Coins ����� ���� ����� ������ ���� ����������
        foreach (Battery coin in FindObjectsOfType<Battery>())
        {
            coin.SetLevelManager(this);
        }
    }

    public void UpdateScore (int score) {
        progress.LevelScore += score;
        //levelHUD.commandQueue.TryEnqueueCommand(new LevelHUD.UpdateScoreCommand(progress.LevelScore));
    }
}