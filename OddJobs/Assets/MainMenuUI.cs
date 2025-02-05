using System;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] GameObject continueButton;


    void Start()
    {
        if(GetPlayerSave() == false)
        {
            continueButton.SetActive(false);
        }

        Authenticate();
    }

    private bool GetPlayerSave()
    {
        int saveGame = PlayerPrefs.GetInt("GameStarted");

        if(saveGame == 0)
        {
            return false;
        }
        else
        {
            return true;
        }

    }

    async void Authenticate()
    {
         await UnityServices.InitializeAsync();

         AuthenticationService.Instance.SignedIn += () => {
            // do nothing
            Debug.Log("Signed in! " + AuthenticationService.Instance.PlayerId);

            //RefreshLobbyList();
        };


        if(!AuthenticationService.Instance.IsSignedIn) await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public void StartNewGame()
    {
        int gameSave = PlayerPrefs.GetInt("GameStarted");

        if(gameSave > 0)
        {
            //Restart Game
            PlayerPrefs.SetInt("GuildPrestige", 0);
            PlayerPrefs.SetInt("WaterFromExtraction", 0);
        }
        
        //Setup New Game
        PlayerPrefs.SetInt("GameStarted", 1);

        LobbyManager.Instance.CreateLobby();
    }

    public void ContinueGame()
    {
        LobbyManager.Instance.CreateLobby();
    }
}
