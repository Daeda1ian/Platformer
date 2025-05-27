using System;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Component {
    public class EnterCollisionComponent : MonoBehaviour {

        [SerializeField] private string[] _tags;
        [SerializeField] private EnterEvent _action;

        private void OnCollisionEnter2D(Collision2D collision) {
            for(int i = 0; i < _tags.Length; i++) {
                if(collision.gameObject.CompareTag(_tags[i])) {
                    _action?.Invoke(collision.gameObject);
                }
            }
        }
    }

    [Serializable]
    public class EnterEvent : UnityEvent<GameObject> {

    }
}
