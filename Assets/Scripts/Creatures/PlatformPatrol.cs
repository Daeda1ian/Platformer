using System.Collections;
using UnityEngine;

namespace PixelCrew.Creatures {
    public class PlatformPatrol : Patrol {

        [SerializeField] private GameObject _platformEgdeChecker;

        private LayerCheck _groundCheck;
        private Creature _creature;
        private bool _waiting = false;

        private void Awake() {
            _creature = GetComponent<Creature>();
            _groundCheck = _platformEgdeChecker.GetComponent<LayerCheck>();
        }

        public override IEnumerator DoPatrol() {
            while (enabled) {
                var direction = _platformEgdeChecker.transform.position - transform.position;
                if (!IsOnThePatform()) {
                    direction = ChangeDirection(direction);
                    _waiting = true;
                }
                direction.y = 0;
                _creature.SetDirectionX(direction.normalized.x);
                _creature.SetDirectionY(direction.normalized.y);

                if (_waiting) {
                    yield return new WaitForSeconds(0.3f);
                    _waiting = false;
                }

                yield return null;
            }
        }

        private bool IsOnThePatform() {
            bool isEdge = _groundCheck.IsTouchingLayer;
            return isEdge;
        }

        private Vector3 ChangeDirection(Vector3 direction) {
            var pos = new Vector3(-direction.x, direction.y, 0);
            return pos;
        }

    }
}
