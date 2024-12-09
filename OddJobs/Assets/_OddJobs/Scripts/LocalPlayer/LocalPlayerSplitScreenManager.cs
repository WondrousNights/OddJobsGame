using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.InputSystem;

public class LocalPlayerSplitScreenManager : MonoBehaviour
{


    [SerializeField] List<PlayerInput> playerInputs;

    public void OnPlayerJoined(PlayerInput playerInput)
    {

        playerInputs.Add(playerInput);

        if(playerInputs.Count == 1)
        {
            playerInputs[0].camera.rect = new Rect(0, 0f, 1, 1f);
        }
        if(playerInputs.Count == 2)
        {
            playerInputs[0].camera.rect = new Rect(0, 0.5f, 1f, 0.5f);
            playerInputs[1].camera.rect = new Rect(0, 0, 1, 0.5f);
        }

        
    }
}
