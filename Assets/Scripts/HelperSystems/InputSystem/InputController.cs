using LKS.GameUpdate;
using System;

namespace LKS.Managers
{
    public static partial class InputManager
    {
        private class InputController : IUpdatable, IDisposable
        {
#region Constants & Fields
            public Action OnUpdate;
            private bool _running;
#endregion

#region Constructors
            public InputController(Action onUpdate)
            {
                OnUpdate = onUpdate;
            }
#endregion

#region Public Methods
            public void Update()
            {
                OnUpdate?.Invoke();
            }

            public void Start()
            {
                if (_running)
                    return;

                _running = true;
                GameUpdateManager.AddUpdatable(this);
            }

            public void Stop()
            {
                if (!_running)
                    return;

                _running = false;
                GameUpdateManager.RemoveUpdatable(this);
            }

            public void Dispose()
            {
                Stop();
                OnUpdate = null;
            } 
#endregion
        }
    }
}