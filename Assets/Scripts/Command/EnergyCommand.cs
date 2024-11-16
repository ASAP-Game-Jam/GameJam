// ����� ���������� �� ������
public partial class LevelHUD {
    // ������ ������� ����� �������� ������ ���������� ���������� ����� � ����������
    // ������� ��� ���������� ��������
    public class EnergyCommand : IUICommand {
        public uint Energy { get; }

        public EnergyCommand (uint energy) {
            Energy = energy;
        }
    }
}