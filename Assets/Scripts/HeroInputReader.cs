using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using PixelCrew.Creatures;

namespace PixelCrew {
    public class HeroInputReader : MonoBehaviour {

        [SerializeField] private Hero _hero;

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

    }
}
