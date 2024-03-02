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
                GameUpdateManager.AddUpdatable(this);
            }

            public void Stop()
            {
                GameUpdateManager.RemoveUpdatable(this);
            }

            public void Dispose()
            {
                OnUpdate = null;
            } 
#endregion
        }
    }
}