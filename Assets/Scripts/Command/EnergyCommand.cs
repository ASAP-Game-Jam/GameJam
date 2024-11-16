// Класс интерфейса на уровне
public partial class LevelHUD {
    // Данная команда будет вызывать логику обновления количества очков в интерфейсе
    // Команда для управления энергией
    public class EnergyCommand : IUICommand {
        public uint Energy { get; }

        public EnergyCommand (uint energy) {
            Energy = energy;
        }
    }
}