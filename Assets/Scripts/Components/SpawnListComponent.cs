using System;
using UnityEngine;

namespace PixelCrew.Component {
    public class SpawnListComponent : MonoBehaviour {

        [SerializeField] private SpawnData[] _spawners;

        public void Spawn(string id) {
            foreach (var data in _spawners) {
                if (data.id == id) {
                    data.component.Spawn();
                }
            }
        }

        [Serializable]
        public class SpawnData {
            public string id;
            public SpawnComponent component;
        }

    }
}
