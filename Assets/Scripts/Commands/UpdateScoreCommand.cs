// ����� ���������� �� ������
public partial class LevelHUD {
    // ������ ������� ����� �������� ������ ���������� ���������� ����� � ����������
    public class UpdateScoreCommand: IUICommand {
        // ����� ���� ������� ����� ����� ���������� � ����������
        public int NewScore { get; }

        public UpdateScoreCommand (int newScore) {
            NewScore = newScore;
        }
    }
}