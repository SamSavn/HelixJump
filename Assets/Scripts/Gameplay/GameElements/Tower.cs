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

using BallState = LKS.States.BallStates.BallState;
using BallFallState = LKS.States.BallStates.FallingState;
using BallBounceState = LKS.States.BallStates.BouncingState;
using System.Runtime.InteropServices;

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
        private Platform _topPlatform;

        private Quaternion _currentRotation;

        private float _globalPlatformsDistance;
        private float _currentSlideDuration;

        private int _maxPlatforms;
#endregion

#region Serialized Fields
        [SerializeField] private Transform _mesh;
        [SerializeField] private float _slidingDefaultDuration;
        [SerializeField] private float _slidingMinDuration;
        [SerializeField] private float _slidingDurationModifier;
#endregion

#region Properties
        public override Vector3 Position 
        { 
            get => _mesh.position; 
            set => _mesh.position = value; 
        }

        private Platform TopPlatform
        {
            get
            {
                if (_topPlatform != null)
                {
                    return _topPlatform;
                }
                else if (_platforms != null && _platforms.Count > 0)
                {
                    return _platforms[0];
                }

                return null;
            }
            set
            {
                if (value != null)
                {
                    _topPlatform = value;
                }
                else if (_platforms != null &&  _platforms.Count > 0)
                {
                    _topPlatform = _platforms[0];
                }

                _topPlatform = null;
            }
        }

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
            _platforms ??= _levelHandler?.Generate(this, _maxPlatforms, out _topPlatform);
            _platformsIterator ??= new Iterator(_platforms);

            TopPlatform = _topPlatform;

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
            if (!_stateMachine.HasState<SlidingState>())
            {
                _stateMachine.ChangeState(new SlidingState(this)); 
            }
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

        public void CheckForLevelUpdate()
        {
            if (TopPlatform != null && !CanActivatePlatform(TopPlatform))
            {
                _platformsIterator.Remove(TopPlatform);
                _levelHandler.UpdateLevel(out Platform newPlatform, out _topPlatform);
                TopPlatform = _topPlatform;

                if (newPlatform != null)
                {
                    _platformsIterator.Add(newPlatform);
                }
            }

            Platform platform;
            for (int i = 0; i < _platforms.Count; i++)
            {
                platform = _platforms[i];

                if(platform == null)
                    continue;

                platform.SetActive(CanActivatePlatform(platform));
                platform.CheckIfEnable();
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
        private void OnBallStateChanged(BallState state)
        {
            if (state.GetType() == typeof(BallFallState))
            {
                CheckForLevelUpdate();
                Slide();
            }
            else if (state.GetType() == typeof(BallBounceState))
            {
                Idle();                
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
            _levelHandler.UpdateLevel(out Platform newPlatform, out _topPlatform);
            TopPlatform = _topPlatform;

            if (newPlatform != null)
            {
                _platformsIterator.Add(newPlatform);
            }
        }
#endregion
    }
}