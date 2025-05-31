using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using PixelCrew.Creatures;

namespace PixelCrew {
    public class HeroInputReader : MonoBehaviour {

        [SerializeField] private Hero _hero;

        private float _endTime;

        public void OnHorizontalMovement(InputAction.CallbackContext context) {
            var direction = context.ReadValue<float>();
            _hero.SetDirectionX(direction);
        }

        public void OnVerticalMovement(InputAction.CallbackContext context) {
            var direction = context.ReadValue<float>();
            _hero.SetDirectionY(direction);
        }

        public void OnSaySomething(InputAction.CallbackContext context) {
            if(context.canceled) {
                //_hero.SaySomething();
            }
        }

        public void OnInteract(InputAction.CallbackContext context) {
            if (context.canceled) {
                _hero.Interact();
            }
        }

        public void OnAttackSomething(InputAction.CallbackContext context) {
            if(context.canceled) {
                _hero.Attack();
            }
        }

        public void OnThrow(InputAction.CallbackContext context) {
            if(context.started) {
                _endTime = Time.time + 1f;
            }

            if(context.canceled) {
                if(_endTime <= Time.time) {
                    _hero.ThrowQueue();
                } else {
                    _hero.Throw();
                }
            }
        }

    }
}
