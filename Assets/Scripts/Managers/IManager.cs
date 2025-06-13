namespace Assets.Scripts.Managers{
    public interface IManager
    {
        EStatusManager Status { get; }
        void Startup();
        void Shutdown();
    }
}