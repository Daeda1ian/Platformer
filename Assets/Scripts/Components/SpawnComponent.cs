using UnityEngine;

namespace PixelCrew.Component {
    public class SpawnComponent : MonoBehaviour {

        [SerializeField] private Transform _target;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private bool _multipleSpawn;

        private GameObject instance;

        [ContextMenu("Spawn")]
        public void Spawn() {
            if (!_multipleSpawn && instance != null) {
                return;
            }
            instance = Instantiate(_prefab, _target.position, Quaternion.identity);
            instance.transform.localScale = transform.lossyScale;
        }

    }
}
