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
        private LevelHandler _levelHandler;
        private Iterator _platformsIterator; 

        private List<Platform> _platforms = new List<Platform>();

        private Quaternion _currentRotation;

        private float _globalPlatformsDistance;
        private int _maxPlatforms;
#endregion

#region Properties
        private float TopPosThreshold => Position.y + Scale.y;
        private float BottomPosThreshold => Position.y - Scale.y; 
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

            _levelHandler = new LevelHandler(_generationData);
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
            _platforms = _levelHandler?.Generate(this, _maxPlatforms);
            _platformsIterator = new Iterator(_platforms);
            _platformsIterator.OnIterationCompleted += OnPlatformsIterationCompleted;
        }

        private void OnDisable()
        {
            InputManager.OnSwipe -= OnSwipe;
            InputManager.OnInputUp -= OnInputUp;

            GameUpdateManager.RemoveUpdatable(this);
        }

        private void OnDestroy()
        {
            _levelHandler.Dispose();
        }
#endregion

#region Public Methods
        public bool CanActivatePlatform(Platform platform)
        {
            return platform.Position.y.IsInRange(BottomPosThreshold, TopPosThreshold, false);
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

        public void CustomUpdate()
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                GameManager.StartGame();
            }
        }
#endregion

#region Event Handlers
        private void OnPlatformsIterationCompleted()
        {
            if (_platformsIterator == null ||
                _platformsIterator.First is not Platform topPlatform ||
                CanActivatePlatform(topPlatform))
            {
                return;
            }

            _platformsIterator.Remove(topPlatform);
            _levelHandler.UpdateLevel(out Platform newPlatform);

            if (newPlatform != null)
            {
                _platformsIterator.Add(newPlatform);
            }
        }

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