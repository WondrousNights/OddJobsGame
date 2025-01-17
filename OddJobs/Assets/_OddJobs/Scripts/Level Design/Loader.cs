using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader 
{
    //Add new levels here
    public enum Scene
    { 
        Lobby,
        Parkour,
        Zone,
        Network_DesertLevel,
        Network_Valley
    }
    public static void Load(Scene targetScene)
    {
        SceneManager.LoadScene(targetScene.ToString(), LoadSceneMode.Single);
    }
    public static void LoadNetwork(Scene targetScene)
    {
        NetworkManager.Singleton.SceneManager.LoadScene(targetScene.ToString(), LoadSceneMode.Single);
    }
}
