using LKS.GameElements;
using LKS.Inputs;
using LKS.Utils;
using UnityEngine;

namespace LKS.States.TowerStates
{
    public class RotatingState : TowerState
    {
#region Constants & Fields
        private float _normalizedSwipeDistance;
        private float _initialRotation;
        private float _targetRotation;
        private float _rotationAngle;
#endregion

#region Constructors
        public RotatingState(Tower tower, SwipeInfo info) : base(tower)
        {
            Update(info);
        }
#endregion

#region Public Methods
        public override void OnEnter()
        {
            base.OnEnter();
            _initialRotation = _tower.EulerAngles.y;
        }

        public override void UpdateState()
        {
            base.UpdateState();
            _tower.Rotate(_rotationAngle);
        }

        public void Update(SwipeInfo info)
        {
            _normalizedSwipeDistance = info.Distance.x / Screen.width;
            _targetRotation = Mathf.Lerp(_initialRotation, _initialRotation + 360f, _normalizedSwipeDistance);

            if (info.Direction.x == Direction.Left)
            {
                _rotationAngle = /*_initialRotation + */_targetRotation;
            }
            else if (info.Direction.x == Direction.Right)
            {
                _rotationAngle = /*_initialRotation*/ - _targetRotation;
            }
        } 
#endregion
    }
}