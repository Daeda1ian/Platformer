using UnityEngine;

namespace PixelCrew {
    public class FollowTarget : MonoBehaviour {

        [SerializeField] private Transform _target;
        [SerializeField] private float _damping;

        private void LateUpdate() {
            Vector3 destination = new Vector3(_target.position.x, _target.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, destination, _damping * Time.deltaTime);
            Debug.Log("LateUpdate");
        }

    }
}
