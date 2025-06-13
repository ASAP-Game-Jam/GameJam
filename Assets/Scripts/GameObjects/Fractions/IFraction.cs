namespace Assets.Scripts.GameObjects.Fractions {
    
    public interface IFraction
    {
        FractionType Fraction { get; }
    }
    public interface IModifyFraction
    {
        FractionType Fraction { get; set; }
    }
}