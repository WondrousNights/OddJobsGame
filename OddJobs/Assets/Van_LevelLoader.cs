using UnityEngine;

public class Van_LevelLoader : MonoBehaviour
{
    public void LoadLevel()
    {
        Loader.LoadNetwork(Loader.Scene.Network_Valley);
    }
}
