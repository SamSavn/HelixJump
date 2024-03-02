using LKS.Extentions;
using LKS.GameElements;
using LKS.Inputs;
using System;
using System.Collections;
using UnityEngine;

namespace LKS.Managers
{
    public static partial class InputManager
    {
#region Constants & Fields
        private const float MOVE_THRESHOLD = .1f;

        public static event Action<InputInfo> OnInputDown;
        public static event Action<InputInfo> OnInputUp;
        public static event Action<InputInfo> OnInputHeld;
        public static event Action<SwipeInfo> OnSwipe;

        private static Vector2 _currentPosition;
        private static Vector2? _startPosition;
        private static Vector2 _moveDirection;
        private static Vector2 _lastMovePosition;

        private static InputInfo _inputHoldInfo;
        private static SwipeInfo _swipeInfo;

        private static Coroutine _updateMoveCoroutine;
        private static InputController _controller;

        private static float _pressTime;
#endregion

#region Constructors
        static InputManager()
        {
            _swipeInfo = new SwipeInfo();
            _inputHoldInfo = new InputInfo();

            _controller = new InputController(UpdateInputs);
            _pressTime = 0;
        }
#endregion

#region Public Methods
        public static void Start()
        {
            Debug.Log("Start");
            _controller.Start();
        }

        public static void Stop()
        {
            _controller.Stop();

            _swipeInfo.Reset();
            _inputHoldInfo = new InputInfo();
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
            if (!GetInputHold())
                return false;

            Vector2 direction = GetInputPosition() - _lastMovePosition;
            UpdateLastMovePosition();

            return (direction.x > MOVE_THRESHOLD || direction.x < -MOVE_THRESHOLD) ||
                   (direction.y > MOVE_THRESHOLD || direction.y < -MOVE_THRESHOLD);
        }

        private static void UpdateLastMovePosition()
        {
            if (_updateMoveCoroutine == null)
            {
                GameUpdateManager.StartCoroutine(UpdateMovePositionCo());
            }

            IEnumerator UpdateMovePositionCo()
            {
                yield return null;
                _lastMovePosition = _currentPosition;
                _updateMoveCoroutine = null;
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
                _startPosition = null;
                _swipeInfo.Reset();

                OnInputUp?.Invoke(new InputInfo(GetInputPosition()));
            }

            if (GetInputHold())
            {
                _currentPosition = GetInputPosition();
                _pressTime += Time.deltaTime;
                _inputHoldInfo.Update(_currentPosition, _pressTime);

                if (IsMoving())
                {
                    Debug.Log("SWIPE");

                    _swipeInfo.Update(_startPosition.Value, GetInputPosition(), GetMoveDirection());
                    OnSwipe?.Invoke(_swipeInfo);
                }
                else
                {
                    Debug.Log("HOOOOOOLD");
                    OnInputHeld?.Invoke(_inputHoldInfo);
                }
            }
        } 
#endregion
    }
}