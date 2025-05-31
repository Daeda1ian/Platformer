using PixelCrew.Creatures;
using UnityEngine;

namespace PixelCrew {
    public class Sword : MonoBehaviour {

        private Hero _hero;

        private void Start() {
            _hero = FindObjectOfType<Hero>();
        }

        public void SetSword() {
            _hero.AddSword();
        }

    }
}
