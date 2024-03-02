using UnityEngine;

namespace LKS.Data
{
    [CreateAssetMenu(fileName = "InputData", menuName = "Data/Input")]
    public class InputData : ScriptableObject
    {
        [SerializeField] private float _moveThreshold;
        [SerializeField] private float _holdTimeThreshold;
        [SerializeField] private float _lastMovePositionUpdateTimeout;

        public float MoveThreshold => _moveThreshold;
        public float HoldTimeThreshold => _holdTimeThreshold;
        public float LastMovePositionUpdateTimeout => _lastMovePositionUpdateTimeout;
    }
}