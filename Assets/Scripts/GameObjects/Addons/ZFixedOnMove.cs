using UnityEngine;

namespace Assets.Scripts.GameObjects.Addons
{
    public class ZFixedOnMove : MonoBehaviour
    {
        public float minZ = 0.0f;
        public float coefficient = 100.0f;
        void Update()
        {
            Vector3 vector3 = transform.position;
            vector3.z = minZ + vector3.y / coefficient;
            transform.position = vector3;
        }
    }
}
