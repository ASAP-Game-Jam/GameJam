namespace Assets.Scripts.Interfaces.Tower
{
    public interface ITower : IDestroyObject
    {
        uint Cost { get; set; }
    }
}
