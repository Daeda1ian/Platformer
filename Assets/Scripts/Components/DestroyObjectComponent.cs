using UnityEngine;

namespace PixelCrew.Component {
    public class DestroyObjectComponent : MonoBehaviour {

        [SerializeField] private GameObject _objectToDestroy;

        public void DestroyObject() {
            Destroy(_objectToDestroy);
        }

    }
}
