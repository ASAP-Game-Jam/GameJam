namespace Assets.Scripts.Interfaces
{
    public interface IBullet
    {
        Direction Direction { get; set; }
        uint Damage { get; set; }
        float Speed { get; set; }
    }
    public enum Direction { Left, Right };
}
