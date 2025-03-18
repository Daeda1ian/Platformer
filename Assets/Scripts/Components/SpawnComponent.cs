using UnityEngine;

namespace PixelCrew.Component {
    public class SpawnComponent : MonoBehaviour {

        [SerializeField] private Transform _target;
        [SerializeField] private GameObject _prefab;

        [ContextMenu("Spawn")]
        public void Spawn() {
            GameObject instance = Instantiate(_prefab, _target.position, Quaternion.identity);
            instance.transform.localScale = transform.lossyScale;
        }

    }
}
