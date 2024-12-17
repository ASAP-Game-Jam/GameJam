namespace Assets.Scripts.Interfaces
{
    public interface ILevelManager
    {
        uint Score { get; }
        bool IsEnemyOnLine(uint index);
    }
}
