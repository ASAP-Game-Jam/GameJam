using Assets.Scripts.Other;

namespace Assets.Scripts.Interfaces.Base
{
    public interface IBase : IDestroyObject
    {
        BaseType BaseType { get; }
    }
}
