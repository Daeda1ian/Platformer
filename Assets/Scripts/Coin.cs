using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew {
    public class Coin : MonoBehaviour {

        [SerializeField] private int value;
        [SerializeField] private Hero _hero;

        private string coin_type;

        private void Awake() {
            if(value == 1) {
                coin_type = "silver";
            } else if(value == 10) {
                coin_type = "gold";
            }
        }

        public void AddCoin() {
            _hero.SetCoin(value, coin_type);
        }

    }

}
