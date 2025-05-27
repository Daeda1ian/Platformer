using UnityEngine;
using PixelCrew.Creatures;

namespace PixelCrew.Component {
    public class ArmHeroComponent : MonoBehaviour {

        public void ArmHero(GameObject go) {
            var hero = go.GetComponent<Hero>();
            if (hero != null) {
                hero.ArmHero();
            }
        }

    }
}
