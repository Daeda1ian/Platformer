using UnityEngine;

namespace PixelCrew.Component {
    public class HealthPointsManageComponent : MonoBehaviour {

        [SerializeField] private int deltaHP;

        public void ManageHealthPoints(GameObject target) {
            HealthComponent healthComponent = target.GetComponent<HealthComponent>();
            if (healthComponent != null) {
                healthComponent.ManageHealthPoints(deltaHP);
            }
        }

    }
}
