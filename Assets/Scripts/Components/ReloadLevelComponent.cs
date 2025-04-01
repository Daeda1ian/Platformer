using UnityEngine;
using UnityEngine.SceneManagement;
using PixelCrew.Model;

namespace PixelCrew.Component {
    public class ReloadLevelComponent : MonoBehaviour {

        public void ReloadScene() {
            var session = FindObjectOfType<GameSession>();
            
            Destroy(session.gameObject);


            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }
}
