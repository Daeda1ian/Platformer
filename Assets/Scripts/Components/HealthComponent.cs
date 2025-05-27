using UnityEngine;
using UnityEngine.Events;
using System;

namespace PixelCrew.Component {
    public class HealthComponent : MonoBehaviour {

        [SerializeField] private int _health;
        [SerializeField] private UnityEvent _onDamage;
        [SerializeField] private UnityEvent _onDie;
        [SerializeField] private UnityEvent _onHeal;
        [SerializeField] private HealthChangeEvent _onChange;

        private bool isDestroy = false;

        public void ManageHealthPoints(int delta) {
            if(isDestroy) return;

            _health += delta;
            _onChange?.Invoke(_health);

            if (delta < 0) {
                _onDamage?.Invoke();
            } else if (delta > 0) {
                _onHeal?.Invoke();
            }

            if (_health <= 0) {
                isDestroy = true;
                Debug.Log("OnDie: " + gameObject.name);
                _onDie?.Invoke();
            } 
        }

        public void SetHealth(int health) {
            _health = health;
        }
    }

    [Serializable]
    public class HealthChangeEvent : UnityEvent<int> { }
}
