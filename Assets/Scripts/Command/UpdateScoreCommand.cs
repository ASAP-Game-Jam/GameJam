// Класс интерфейса на уровне
public partial class LevelHUD {
    // Данная команда будет вызывать логику обновления количества очков в интерфейсе
    public class UpdateScoreCommand: IUICommand {
        // Новый счет который будет нужно отобразить в интерфейсе
        public int NewScore { get; }

        public UpdateScoreCommand (int newScore) {
            NewScore = newScore;
        }
    }
}