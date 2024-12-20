using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerConfigurationManager : MonoBehaviour
{
    private List<PlayerConfiguration> playerConfigs;
    [SerializeField]
    private int MaxPlayers = 4;

    public static PlayerConfigurationManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.Log("[Singleton] Trying to instantiate a seccond instance of a singleton class.");
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
            playerConfigs = new List<PlayerConfiguration>();
        }
        
    }

    public void HandlePlayerJoin(PlayerInput pi)
    {
        Debug.Log("player joined " + pi.playerIndex);

        if(!playerConfigs.Any(p => p.PlayerIndex == pi.playerIndex))
        {
            playerConfigs.Add(new PlayerConfiguration(pi));
        }

        SetUpSplitScreen();
    }
    

    public List<PlayerConfiguration> GetPlayerConfigs()
    {
        return playerConfigs;
    }

    public void SetUpSplitScreen()
    {

        if(playerConfigs.Count == 1)
        {
            playerConfigs[0].Input.camera.rect = new Rect(0, 0f, 1, 1f);
        }
        if(playerConfigs.Count == 2)
        {
            playerConfigs[0].Input.camera.rect = new Rect(0, 0.5f, 1f, 0.5f);
            playerConfigs[1].Input.camera.rect = new Rect(0, 0, 1, 0.5f);
        }
    }


}

public class PlayerConfiguration
{
    public PlayerConfiguration(PlayerInput pi)
    {
        PlayerIndex = pi.playerIndex;
        Input = pi;
    }

    public PlayerInput Input { get; private set; }
    public int PlayerIndex { get; private set; }

}