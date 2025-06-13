using Assets.Scripts.GameObjects.Entities;
using UnityEngine;

namespace Assets.Scripts.GameObjects.Attacks
{
    public class Target : MonoBehaviour, ITarget
    {
        public virtual bool CheckEntity(IBasicEntity entity)
        {
            return entity is IBasicEntity;
        }
    }
}
