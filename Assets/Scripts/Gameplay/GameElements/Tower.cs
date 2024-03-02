using LKS.Data;
using LKS.Extentions;
using LKS.Helpers;
using LKS.Inputs;
using LKS.Managers;
using LKS.States;
using LKS.States.TowerStates;
using System.Collections.Generic;
using UnityEngine;

namespace LKS.GameElements
{
    public class Tower : GameElement
    {
#region Fields
        private const string GENERATION_DATA_ADDRESS = "LevelGenerationData";
        private const int ADDITIONAL_PLATFORMS_OFFSET = 3;

        private StateMachine<TowerState> _stateMachine;

        private LevelGenerationData _generationData;
        private LevelGenerator _levelGenerator;
        private List<Platform> _platforms = new List<Platform>();

        private Quaternion _currentRotation;

        private float _globalPlatformsDistance;
#endregion

#region Properties
        private float Offset => ADDITIONAL_PLATFORMS_OFFSET * _globalPlatformsDistance;
        private float TopPosThreshold => Position.y + Scale.y + Offset;
        private float BottomPosThreshold => Position.y - Scale.y - Offset; 
#endregion

#region Unity Methods
        private void Awake()
        {
            _stateMachine = new StateMachine<TowerState>(new IdleState(this));
            _generationData = AddressablesLoader.LoadSingle<LevelGenerationData>(GENERATION_DATA_ADDRESS);

            if(_generationData == null)
            {
                Debug.LogError("Unable to load level generation data");
                return;
            }

            _levelGenerator = new LevelGenerator(_generationData);
            _globalPlatformsDistance = transform.TransformPoint(new Vector3(0, _generationData.PlatformsDistance, 0)).y;            

            GameManager.SetTower(this);
        }

        private void OnEnable()
        {
            InputManager.OnSwipe += OnSwipe;
            InputManager.OnInputUp += OnInputUp;
        }

        private void Start()
        {
            _platforms = _levelGenerator?.Generate(this, 10);
        }

        private void OnDisable()
        {
            InputManager.OnSwipe -= OnSwipe;
            InputManager.OnInputUp -= OnInputUp;
        }
#endregion

#region Public Methods
        public bool CanActivatePlatform(Platform platform)
        {
            return platform.Position.y.IsInRange(BottomPosThreshold, TopPosThreshold);
        }

        public void Rotate(float angle)
        {
            _currentRotation = Quaternion.AngleAxis(angle * Time.deltaTime, Vector3.up);
            Rotation = _currentRotation * Rotation;
        }
#endregion

#region Event Handlers
        private void OnSwipe(SwipeInfo info)
        {
            if (!_stateMachine.IsInState<RotatingState>())
            {
                _stateMachine.ChangeState(new RotatingState(this, info));
            }
            else
            {
                _stateMachine.GetCurrentState<RotatingState>().Update(info);
                _stateMachine.UpdateState();
            }
        }

        private void OnInputUp(InputInfo _)
        {
            if (!_stateMachine.IsInState<IdleState>())
            {
                _stateMachine.ChangeState(new IdleState(this)); 
            }
        }
#endregion
    }
}