using UnityEngine;
using UnityEngine.Events;
using System;

namespace PixelCrew.Component {
    public class HealthComponent : MonoBehaviour {

        [SerializeField] private int _health;
        [SerializeField] private UnityEvent _onDamage;
        [SerializeField] private UnityEvent _onDie;
        [SerializeField] private HealthChangeEvent _onChange;

        private bool isDestroy = false;

        public void ManageHealthPoints(int delta) {
            if(isDestroy) return;

            _health += delta;
            _onChange?.Invoke(_health);

            if(_health <= 0) {
                isDestroy = true;
                _onDie?.Invoke();
                return;
            } else if(delta < 0) {
                _onDamage?.Invoke();
            }
        }

        public void SetHealth(int health) {
            _health = health;
        }
    }

    [Serializable]
    public class HealthChangeEvent : UnityEvent<int> { }
}
