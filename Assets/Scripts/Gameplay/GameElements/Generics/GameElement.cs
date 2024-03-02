using UnityEngine;

namespace LKS.GameElements
{
    public abstract class GameElement : MonoBehaviour
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

        public Vector3 Scale => transform.lossyScale;
        
        public int Id => gameObject.GetInstanceID();
#endregion

#region Public Methods
        public virtual void SetActive(bool active)
        {
            gameObject.SetActive(active);
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
            return base.GetHashCode();
        }  
#endregion
    }
}