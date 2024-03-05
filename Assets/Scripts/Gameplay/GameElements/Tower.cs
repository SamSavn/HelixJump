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

        private List<Platform> _platforms;

        private Quaternion _currentRotation;

        private float _globalPlatformsDistance;
        private float _currentSlideDuration;

        private int _maxPlatforms;
#endregion

#region Serialized Fields
        [SerializeField] private float _slidingDefaultDuration;
        [SerializeField] private float _slidingMinDuration;
        [SerializeField] private float _slidingDurationModifier;
#endregion

#region Properties
        private float TopPosThreshold => Position.y + Scale.y;
        private float BottomPosThreshold => Position.y - Scale.y; 
        public float SlidingDuration => _currentSlideDuration;
#endregion

#region Unity Methods
        private void Awake()
        {
            Position = Vector3.zero;
            _generationData = AddressablesLoader.LoadSingle<LevelGenerationData>(GENERATION_DATA_ADDRESS);

            if (_generationData == null)
            {
                Debug.LogError("Unable to load level generation data");
                return;
            }

            _globalPlatformsDistance = _generationData.PlatformsDistance.LocalToGlobal(Axis.Y, transform);
            _maxPlatforms = Mathf.RoundToInt((TopPosThreshold - BottomPosThreshold) / _globalPlatformsDistance);

            GameManager.SetTower(this);
        }

        protected override void OnEnable()
        {
            _levelHandler ??= new LevelHandler(_generationData);
            _platforms ??= _levelHandler?.Generate(this, _maxPlatforms);
            _platformsIterator ??= new Iterator(_platforms);

            Idle();

            GameUpdateManager.AddUpdatable(this);
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
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
            return platform.Position.y >= BottomPosThreshold && platform.Position.y < TopPosThreshold;
        }

        public void ResetSlideDuration()
        {
            _currentSlideDuration = _slidingDefaultDuration;
        }

        public void UpdateSlideDuration()
        {
            if(_currentSlideDuration > _slidingMinDuration)
            {
                _currentSlideDuration -= _slidingDurationModifier;
                _currentSlideDuration = Mathf.Max(_currentSlideDuration, _slidingMinDuration);
            }
        }

        [ContextMenu("Slide")]
        public void Slide()
        {            
            _stateMachine.ChangeState(new SlidingState(this, _platformsIterator));
        }

        public void Idle()
        {
            if (_stateMachine == null)
            {
                _stateMachine = new StateMachine<TowerState>(new IdleState(this));
            }
            else
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

        public override void Dispose()
        {
            RemoveListeners();
            GameUpdateManager.RemoveUpdatable(this);

            for (int i = 0; i < _platforms.Count; i++)
            {
                PoolingManager.AddToPool(_platforms[i]);
            }

            _platforms.Clear();
            _platforms = null;

            _platformsIterator = null;

            _levelHandler = null;
            _stateMachine = null;

            base.Dispose();
        }
#endregion

#region Private Methods
        protected override void AddListeners()
        {
            base.AddListeners();

            GameManager.BallStateChanged += OnBallStateChanged;

            if (_platformsIterator != null)
            {
                _platformsIterator.OnIterationCompleted += OnPlatformsIterationCompleted; 
            }
        }

        protected override void RemoveListeners()
        {
            base.RemoveListeners();

            GameManager.BallStateChanged -= OnBallStateChanged;

            if (_platformsIterator != null)
            {
                _platformsIterator.OnIterationCompleted -= OnPlatformsIterationCompleted; 
            }
        }
#endregion

#region Event Handlers
        private void OnBallStateChanged(States.BallStates.BallState state)
        {
            if (state.GetType() == typeof(States.BallStates.FallingState))
            {
                Slide();
                UpdateSlideDuration();
            }
            else if (state.GetType() == typeof(States.BallStates.BouncingState))
            {
                ResetSlideDuration();
            }
        }

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
#endregion
    }
}