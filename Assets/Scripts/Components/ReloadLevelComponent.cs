using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Component {
    public class ReloadLevelComponent : MonoBehaviour {

        public void ReloadScene() {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }

    }
}
