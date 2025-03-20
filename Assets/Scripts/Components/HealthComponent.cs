using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Component {
    public class HealthComponent : MonoBehaviour {

        [SerializeField] private int _health;
        [SerializeField] private UnityEvent _onDamage;
        [SerializeField] private UnityEvent _onDie;

        public void ManageHealthPoints(int delta) {
            _health += delta;
            if(_health <= 0) {
                _onDie?.Invoke();
                return;
            }
            if(delta < 0) {
                _onDamage?.Invoke();
            }
            //Debug.Log("health: " + _health);
            
        }
    }
}
