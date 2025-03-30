using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneAdditive : MonoBehaviour
{

    [SerializeField]
    private string sceneName;
    

    private void Awake() {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }
}
