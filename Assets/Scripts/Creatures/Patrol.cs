using UnityEngine;
using System.Collections;

namespace PixelCrew.Creatures {
    public abstract class Patrol : MonoBehaviour {

        public abstract IEnumerator DoPatrol();

    }
}
