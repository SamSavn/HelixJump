using LKS.Utils;
using UnityEngine;

namespace LKS.Inputs
{
    public class RotateCommand : ICommand
    {
        private Direction _direction;
        private float _rotationAngle;

        public RotateCommand(Direction direction, float rotationAngle)
        {
            _direction = direction;
            _rotationAngle = _direction == Direction.Right 
                                ? rotationAngle
                                : -rotationAngle;
        }

        public void Execute(Transform transform)
        {
            transform.Rotate(Vector3.up, _rotationAngle);
        }
    }
}