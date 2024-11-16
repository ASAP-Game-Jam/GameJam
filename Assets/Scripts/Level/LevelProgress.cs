public class LevelProgress
{
    private uint _levelScore;
    public uint LevelScore
    {
        get => _levelScore;
        set => _levelScore = (value < 0 ? 0 : value);
    }
    
}