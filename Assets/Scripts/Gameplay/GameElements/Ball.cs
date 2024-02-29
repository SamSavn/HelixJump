using UnityEngine;

namespace LKS.GameElements
{
    public class Ball : GameElement
    {
#region Serialized Fields
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _jumpForce;
#endregion

#region Fields
        private Vector3 _impactNormal;
#endregion

#region Collision Detection
        private void OnCollisionEnter(Collision collision)
        {
            _impactNormal = collision.GetContact(0).normal;

            if (Vector3.Dot(_impactNormal, Vector3.up) > 0.9f)
            {
                _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z);
                _rigidbody.AddForce(new Vector3(0, _jumpForce / _rigidbody.mass, 0), ForceMode.VelocityChange);
            }
        } 
#endregion
    }
}