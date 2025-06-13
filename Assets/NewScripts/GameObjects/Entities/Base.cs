using Assets.Scripts.Managers;

namespace Assets.Scripts.GameObjects.Entities
{
    public class Base : BasicEntity, IBase
    {
        private void Start()
        {
            LevelManager.BaseManager.AddBase(this);
        }
    }
}
