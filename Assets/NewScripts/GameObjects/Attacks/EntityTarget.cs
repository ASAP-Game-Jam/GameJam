using Assets.Scripts.GameObjects.Entities;

namespace Assets.Scripts.GameObjects.Attacks
{
    public class EntityTarget : Target
    {
        public override bool CheckEntity(IBasicEntity entity)
        {
            return entity is IEntity;
        }
    }
}
