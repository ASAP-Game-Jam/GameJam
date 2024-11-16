// ����� ���������� �� ������
public partial class LevelHUD {
    // ������ ������� ����� �������� ������ ���������� ���������� ����� � ����������
    // ������� ��� ���������� ����������
    public class ProgressCommand : IUICommand {
        public uint Progress { get; }

        public ProgressCommand (uint progress) {
            Progress = progress;
        }
    }
}