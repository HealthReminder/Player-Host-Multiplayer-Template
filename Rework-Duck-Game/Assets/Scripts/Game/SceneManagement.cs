using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public void LoadScene( int sceneIndex) {
        SceneManager.LoadScene(sceneIndex);
    }
}
