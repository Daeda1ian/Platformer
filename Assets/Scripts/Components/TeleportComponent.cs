using UnityEngine;

namespace PixelCrew.Component {
    public class TeleportComponent : MonoBehaviour {

        [SerializeField] private Transform _destination;

        public void Teleport(GameObject target) {
            target.transform.position = _destination.position;
        }

    }
}
