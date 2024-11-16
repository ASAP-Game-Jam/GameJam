// Класс интерфейса на уровне
public partial class LevelHUD {
    // Данная команда будет вызывать логику обновления количества очков в интерфейсе
    // Команда для управления прогрессом
    public class ProgressCommand : IUICommand {
        public uint Progress { get; }

        public ProgressCommand (uint progress) {
            Progress = progress;
        }
    }
}