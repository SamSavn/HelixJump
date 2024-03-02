using LKS.GameUpdate;
using LKS.Helpers;
using System;
using UnityEngine;

namespace LKS.Managers
{
    public static class GameUpdateManager
    {
        #region Constants & Fields
        private const string CONTROLLER_ADDRESS = "GameUpdateController";
        private static GameUpdateController _controller;
        #endregion

        #region Properties
        private static GameUpdateController Controller
        {
            get
            {
                if(_controller == null)
                {
                    _controller = CreateController();
                }

                return _controller;
            }
            set => _controller = value;
        }
        #endregion

        #region Constructors
        static GameUpdateManager()
        {
            Controller = CreateController();
        }
        #endregion

        #region Public Methods
        public static void AddUpdatable(IUpdatable updatable)
        {
            Controller.AddUpdatable(updatable);
        }

        public static void RemoveUpdatable(IUpdatable updatable)
        {
            Controller.RemoveUpdatable(updatable);
        }
        #endregion

        #region Private Methods
        private static GameUpdateController CreateController()
        {
            if (_controller != null)
                return _controller;

            GameObject prefab = AddressablesLoader.LoadSingle<GameObject>(CONTROLLER_ADDRESS);

            if (prefab == null)
            {
                prefab = new GameObject(CONTROLLER_ADDRESS);
                prefab.AddComponent<GameUpdateController>();
            }
            else
            {
                prefab.name = CONTROLLER_ADDRESS;
            }

            return GameObject.Instantiate(prefab).GetComponent<GameUpdateController>();
        }
        #endregion
    }
}