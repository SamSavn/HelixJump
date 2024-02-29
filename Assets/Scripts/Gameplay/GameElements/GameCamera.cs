using LKS.Managers;
using UnityEngine;

namespace LKS.GameElements
{
    public class GameCamera : GameElement
    {
        [SerializeField] private Camera _camera;

        private void Awake()
        {
            GameManager.SetGameCamera(this);
        }
    }
}