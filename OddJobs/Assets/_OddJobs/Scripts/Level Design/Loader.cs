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
        Level_Shootin_Minigame,
        Network_DesertLevel
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
