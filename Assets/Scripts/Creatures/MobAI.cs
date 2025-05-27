using System.Collections;
using UnityEngine;
using PixelCrew.Component;

namespace PixelCrew.Creatures {
    public class MobAI : MonoBehaviour {

        [SerializeField] private LayerCheck _vision;
        [SerializeField] private LayerCheck _canAttack;
        [SerializeField] private float _alarmDelay = 0.5f;
        [SerializeField] private float _attackCooldown = 1f;
        [SerializeField] private float _missHeroCooldown = 1f;

        private Coroutine _current;
        private GameObject _target;
        private Creature _creature;

        private SpawnListComponent _particles;
        private Animator _animator;
        private Patrol _patrol;

        private static readonly int isDeadKey = Animator.StringToHash("is-dead");
        private bool _isDead = false;

        private void Awake() {
            _particles = GetComponent<SpawnListComponent>();
            _creature = GetComponent<Creature>();
            _animator = GetComponent<Animator>();
            _patrol = GetComponent<Patrol>();
        }

        private void Start() {
            StartState(_patrol.DoPatrol());
        }

        public void OnHeroInVision(GameObject go) {
            if(_isDead) return;

            _target = go;
            StartState(AgroToHero());
        }

        private IEnumerator AgroToHero() {
            _particles.Spawn("Exclamation");
            yield return new WaitForSeconds(_alarmDelay);
            StartState(GoToHero());
        }

        private IEnumerator GoToHero() {
            while (_vision.IsTouchingLayer) {

                if (_canAttack.IsTouchingLayer) {
                    StartState(Attack());
                } else {
                    SetDirectionToTarget();
                }
                
                yield return null;
            }
            _particles.Spawn("Miss");
            yield return new WaitForSeconds(_missHeroCooldown);
            StartState(_patrol.DoPatrol());
        }

        private IEnumerator Attack() {
            while(_canAttack.IsTouchingLayer) {
                _creature.Attack();
                yield return new WaitForSeconds(_attackCooldown);
            }
            StartState(GoToHero());
        }

        private void SetDirectionToTarget() {
            var direction = _target.transform.position - transform.position;
            direction.y = 0;
            _creature.SetDirectionX(direction.normalized.x);
            _creature.SetDirectionY(direction.normalized.y);
        }

        private IEnumerator Patrolling() {
            yield return null;
        }

        private void StartState(IEnumerator coroutine) {
            _creature.SetDirectionX(0);
            _creature.SetDirectionY(0);

            if (_current != null) {
                StopCoroutine(_current);
            }
            _current = StartCoroutine(coroutine);
        }

        public void OnDie() {
            _isDead = true;
            _animator.SetBool(isDeadKey, true);

            if(_current != null) {
                StopCoroutine(_current);
            }
        }
    }
}
