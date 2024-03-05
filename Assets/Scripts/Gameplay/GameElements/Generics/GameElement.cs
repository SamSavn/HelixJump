using System;
using UnityEngine;

namespace LKS.GameElements
{
    public abstract class GameElement : MonoBehaviour, IDisposable
    {
#region Properties
        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        public Vector3 LocalPosition
        {
            get => transform.localPosition;
            set => transform.localPosition = value;
        }

        public Vector3 EulerAngles
        {
            get => transform.eulerAngles;
            set => transform.eulerAngles = value;
        }

        public Quaternion Rotation
        {
            get => transform.rotation;
            set => transform.rotation = value;
        }

        public Vector3 LocalScale
        {
            get => transform.localScale;
            set => transform.localScale = value;
        }

        public Vector3 Scale => transform.lossyScale;
        
        public int Id => gameObject.GetInstanceID();

        public bool IsActive => gameObject.activeSelf;
#endregion

#region Unity Methods
        protected virtual void OnEnable()
        {
            AddListeners();             
        }

        protected virtual void OnDisable()
        {
            RemoveListeners();
        }
        #endregion

        #region Public Methods
        public virtual void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        protected virtual void AddListeners() { }
        protected virtual void RemoveListeners() { }

        public virtual void Dispose()
        {
            
        }
#endregion

#region Overrides
        public override bool Equals(object other)
        {
            if (other is GameElement ge)
            {
                return ge.Id == Id;
            }

            return base.Equals(other);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
#endregion
    }
}