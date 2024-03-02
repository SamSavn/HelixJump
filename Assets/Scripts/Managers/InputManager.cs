using LKS.Data;
using LKS.Extentions;
using LKS.GameElements;
using LKS.Helpers;
using LKS.Inputs;
using System;
using System.Collections;
using UnityEngine;

namespace LKS.Managers
{
    public static partial class InputManager
    {
        #region Constants & Fields
        private const string DATA_ADDRESS = "InputData";

        public static event Action<InputInfo> OnInputDown;
        public static event Action<InputInfo> OnInputUp;
        public static event Action<InputInfo> OnInputHeld;
        public static event Action<SwipeInfo> OnSwipe;

        private static Vector2? _lastMovePosition;
        private static Vector2? _startPosition;
        private static Vector2 _currentPosition;
        private static Vector2 _moveDirection;

        private static InputInfo _inputHoldInfo;
        private static SwipeInfo _swipeInfo;

        private static InputData _data;
        private static InputController _controller;
        private static Coroutine _updateMovePosCoroutine;
        private static WaitForSeconds _updateMovePosWait;

        private static float _pressTime;
#endregion

#region Constructors
        static InputManager()
        {
            Reset();

            _data = AddressablesLoader.LoadSingle<InputData>(DATA_ADDRESS);
            _updateMovePosWait = new WaitForSeconds(_data.LastMovePositionUpdateTimeout);
            _controller = new InputController(UpdateInputs);
        }
#endregion

#region Public Methods
        public static void Start()
        {
            _controller.Start();
        }

        public static void Stop()
        {
            _controller.Stop();
            Reset();
        }
#endregion

#region Private Methods
        private static bool GetInputDown()
        {
#if UNITY_EDITOR
            return Input.GetMouseButtonDown(0);
#else
            return Input.GetTouch(0).phase == TouchPhase.Began;
#endif
        }

        private static bool GetInputUp()
        {
#if UNITY_EDITOR
            return Input.GetMouseButtonUp(0);
#else
            return Input.GetTouch(0).phase == TouchPhase.Ended;
#endif
        }

        private static bool GetInputHold()
        {
#if UNITY_EDITOR
            return Input.GetMouseButton(0);
#else
            return Input.GetTouch(0).phase is TouchPhase.Stationary or TouchPhase.Moved;
#endif
        }

        private static bool IsMoving()
        {
            if (!_lastMovePosition.HasValue || !GetInputHold())
                return false;

            Vector2 direction = GetInputPosition() - _lastMovePosition.Value;
            UpdateLastMovePosition();

            return (direction.x > _data.MoveThreshold || direction.x < -_data.MoveThreshold) ||
                   (direction.y > _data.MoveThreshold || direction.y < -_data.MoveThreshold);
        }

        private static void UpdateLastMovePosition()
        {
            if (_updateMovePosCoroutine == null)
            {
                GameUpdateManager.StartCoroutine(UpdateMovePositionCo());
            }

            IEnumerator UpdateMovePositionCo()
            {
                yield return _updateMovePosWait;
                _lastMovePosition = _currentPosition;
                _updateMovePosCoroutine = null;
            }
        }

        private static Vector2 GetInputPosition()
        {
#if UNITY_EDITOR
            return Input.mousePosition;
#else
            return Input.GetTouch(0).position;
#endif
        }

        private static Vector2 GetMoveDirection()
        {
            if (!GetInputHold() || !_startPosition.HasValue)
            {
                return Vector2.zero;
            }
            
            _moveDirection = _currentPosition - _startPosition.Value;
            return _moveDirection.normalized;
        }

        private static void UpdateInputs()
        {
            if (GetInputDown())
            {
                _startPosition = GetInputPosition();
                _pressTime = 0;

                OnInputDown?.Invoke(new InputInfo(_startPosition.Value));
            }

            if (GetInputUp())
            {                
                Reset();
                OnInputUp?.Invoke(new InputInfo(GetInputPosition()));
            }

            if (GetInputHold())
            {
                _currentPosition = GetInputPosition();
                _pressTime += Time.deltaTime;

                if (!_lastMovePosition.HasValue)
                {
                    _lastMovePosition = _currentPosition; 
                }

                if (IsMoving())
                {
                    _pressTime = 0;
                    _swipeInfo.Update(_startPosition.Value, GetInputPosition(), GetMoveDirection());

                    OnSwipe?.Invoke(_swipeInfo);
                }
                else if (_pressTime >= _data.HoldTimeThreshold)
                {
                    _inputHoldInfo.Update(_currentPosition, _pressTime);
                    OnInputHeld?.Invoke(_inputHoldInfo);
                }
            }
            else
            {
                Reset();
            }
        } 

        private static void Reset()
        {
            _swipeInfo.Reset();
            _inputHoldInfo = new InputInfo();

            _pressTime = 0;
            _lastMovePosition = null;
            _startPosition = null;
        }
#endregion
    }
}