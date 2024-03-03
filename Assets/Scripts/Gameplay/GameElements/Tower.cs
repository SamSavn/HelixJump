using LKS.Data;
using LKS.Extentions;
using LKS.GameUpdate;
using LKS.Helpers;
using LKS.Inputs;
using LKS.Iterations;
using LKS.Managers;
using LKS.States;
using LKS.States.TowerStates;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

namespace LKS.GameElements
{
    public class Tower : GameElement, IUpdatable
    {
#region Fields
        private const string GENERATION_DATA_ADDRESS = "LevelGenerationData";

        private StateMachine<TowerState> _stateMachine;

        private LevelGenerationData _generationData;
        private LevelGenerator _levelGenerator;
        private Iterator _platformsIterator; 

        private List<Platform> _platforms = new List<Platform>();

        private Quaternion _currentRotation;

        private float _globalPlatformsDistance;
        private int _additionalPlatformsOffset;
        private int _maxPlatforms;
#endregion

#region Properties
        private float Offset => _additionalPlatformsOffset * _globalPlatformsDistance;
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
            _additionalPlatformsOffset = _generationData.AdditionalPlatformsOffset;
            _globalPlatformsDistance = _generationData.PlatformsDistance.LocalToGlobal(Axis.Y, transform);
            _maxPlatforms = Mathf.RoundToInt((TopPosThreshold - BottomPosThreshold) / _globalPlatformsDistance);

            GameManager.SetTower(this);
        }

        private void OnEnable()
        {
            InputManager.OnSwipe += OnSwipe;
            InputManager.OnInputUp += OnInputUp;

            GameUpdateManager.AddUpdatable(this);
        }

        private void Start()
        {
            _platforms = _levelGenerator?.Generate(this, _maxPlatforms);
            _platformsIterator = new Iterator(_platforms);
        }

        private void OnDisable()
        {
            InputManager.OnSwipe -= OnSwipe;
            InputManager.OnInputUp -= OnInputUp;

            GameUpdateManager.RemoveUpdatable(this);
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

        [ContextMenu("Slide")]
        public void Slide()
        {
            _stateMachine.ChangeState(new SlidingState(this, _platformsIterator));
        }

        public void Idle()
        {
            _stateMachine.ChangeState(new IdleState(this));
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

        public void CustomUpdate()
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                GameManager.StartGame();
            }
        }
        #endregion
    }
}