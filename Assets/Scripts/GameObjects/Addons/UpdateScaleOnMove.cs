using Assets.Scripts.GameObjects.Moving;
using UnityEngine;

namespace Assets.Scripts.GameObjects.Addons
{
    [RequireComponent(typeof(IDirectionMove))]
    public class UpdateScaleOnMove : MonoBehaviour
    {
        private IDirectionMove mMove;
        public Direction2 Direction => mMove.Direction;
        private void Start()
        {
            mMove = GetComponent<IDirectionMove>();
        }
        private void Update()
        {
            transform.localScale = new Vector3(
                Direction switch { Direction2.Left => -1, _ => 1 } * Mathf.Abs(transform.localScale.x),
                transform.localScale.y, transform.localScale.z);
        }
    }
}
