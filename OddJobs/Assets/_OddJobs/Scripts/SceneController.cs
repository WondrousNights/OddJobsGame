using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void LoadSceneWithInt(int sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
