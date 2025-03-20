using System.Collections;
using UnityEngine;

namespace PixelCrew.Component {
    public class TranslateComponent : MonoBehaviour {

        [SerializeField] private Transform[] _points;
        [SerializeField] private float _moveSpeed;

        private int _number = 0;
        private bool _inMove;

        public void TranslateObject() {
            if(_inMove) return;
            Transform point = _points[_number];
            
            StartCoroutine(MoveObject(point));
            _number++;
            if (_number >= _points.Length) {
                _number = 0;
            }
        }

        public void TranslateObjectToDefaultPosition() {
            if(_inMove) return;
            Transform point = _points[0];
            StartCoroutine(MoveObject(point));
            _number = 1;
        }

        private IEnumerator MoveObject(Transform point) {
            _inMove = true;
            while(Vector3.Distance(transform.position, point.position) > 0.01f) {
                transform.position = Vector3.MoveTowards(transform.position, point.position, _moveSpeed * Time.deltaTime);

                yield return null;
            }
            _inMove = false;
        }


    }
}
