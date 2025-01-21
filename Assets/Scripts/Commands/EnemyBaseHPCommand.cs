// Класс интерфейса на уровне
public partial class LevelHUD
{
    public class EnemyBaseHPCommand : IUICommand
    {
        public uint HP { get; }
        public uint MaxHP { get; }

        public EnemyBaseHPCommand(uint maxHp, uint hp)
        {
            HP = hp;
            MaxHP = maxHp;
        }
    }
}