namespace Assets.Scripts.Interfaces
{
    public interface ILevelManager
    {
        uint Score { get; set; }
        void ReloadGame();
        void SaveGame();
        void LoadGame();
        void ExitGame();
        void OpenMainMenu();
    }
}
