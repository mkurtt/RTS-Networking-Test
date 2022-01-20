using UnityEngine;
namespace Game.Scripts.Cameras
{
    public class FaceCamera : MonoBehaviour
    {
        private Transform _camTransform;

        private void Start()
        {
            _camTransform = Camera.main.transform;
        }

        private void LateUpdate()
        {
            transform.LookAt(transform.position + _camTransform.rotation * Vector3.forward, _camTransform.rotation * Vector3.up);
        }
    }
}
