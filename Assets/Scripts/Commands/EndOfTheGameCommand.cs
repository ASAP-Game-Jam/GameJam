// Класс интерфейса на уровне
using Assets.Scripts.Interfaces;
using Assets.Scripts.Other;

public partial class LevelHUD
{
    // Данная команда будет вызывать логику обновления количества очков в интерфейсе
    // Команда для управления энергией
    public class EndOfTheGameCommand : IUICommand
    {
        public BaseType BaseType { get; }

        public EndOfTheGameCommand(BaseType baseType)
        {
            BaseType = baseType;
        }
    }
}