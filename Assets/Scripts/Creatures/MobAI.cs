﻿using System.Collections;
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
            LookAtHero();
            MoveMob(Vector2.zero);
            _particles.Spawn("Exclamation");
            yield return new WaitForSeconds(_alarmDelay);
            StartState(GoToHero());
        }

        private void LookAtHero() {
            var direction = GetDirectionToTarget();
            _creature.UpgradeDirection(direction);
        }

        private Vector2 GetDirectionToTarget() {
            var direction = _target.transform.position - transform.position;
            direction.y = 0;
            return direction.normalized;
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
            MoveMob(Vector2.zero);
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
            var direction = GetDirectionToTarget();
            MoveMob(direction);
            //_creature.SetDirectionX(direction.x);
            //_creature.SetDirectionY(direction.y);
        }

        private IEnumerator Patrolling() {
            yield return null;
        }

        private void StartState(IEnumerator coroutine) {
            MoveMob(Vector2.zero);

            if (_current != null) {
                StopCoroutine(_current);
            }
            _current = StartCoroutine(coroutine);
        }

        private void MoveMob(Vector2 move) {
            _creature.SetDirectionX(move.x);
            _creature.SetDirectionY(move.y);
        }

        public void OnDie() {
            _isDead = true;
            _animator.SetBool(isDeadKey, true);
            MoveMob(Vector2.zero);

            if(_current != null) {
                StopCoroutine(_current);
            }
        }
    }
}
