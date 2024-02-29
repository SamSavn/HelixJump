using UnityEngine;

namespace LKS.GameElements
{
    public abstract class GameElement : MonoBehaviour
    {
#region Properties
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