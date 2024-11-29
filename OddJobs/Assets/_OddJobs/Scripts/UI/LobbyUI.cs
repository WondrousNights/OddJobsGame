using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using System;

public class LobbyUI : MonoBehaviour
{
    public static LobbyUI Instance { get; private set; }

    [SerializeField] private Transform playerSingleTemplate;
    [SerializeField] private Transform container;
    [SerializeField] private TextMeshProUGUI lobbyNameText;
    [SerializeField] private Button leaveLobbyButton;
    [SerializeField] private Button startGameButton;

    float updateLobbyCounter = 2f;

    private void Awake()
    {
        Instance = this;


        startGameButton.onClick.AddListener(() => {
            LobbyManager.Instance.StartGame();
        });
        


    }
    private void Start()
    {
        LobbyManager.Instance.OnJoinedLobby += UpdateLobby_Event;
        this.gameObject.SetActive(false);
    }

    private void UpdateLobby_Event(object sender, LobbyManager.LobbyEventArgs e)
    {
        UpdateLobby();
        this.gameObject.SetActive(true);
    }

    private void Update()
    {
        UpdateLobby();
    }
    private void UpdateLobby()
    {
        updateLobbyCounter -= Time.deltaTime;
        if (updateLobbyCounter <= 0)
        {
            updateLobbyCounter = 1.5f;
            UpdateLobby(LobbyManager.Instance.GetJoinedLobby());
        }

    }
    private void UpdateLobby(Lobby lobby)
    {

        if (lobby == null)
            Debug.Log("Unable to fetch lobby");
            
        ClearLobby();

        foreach (Player player in lobby.Players)
        {
            Debug.Log("Amount of players" + lobby.Players.Count);
            Transform playerSingleTransform = Instantiate(playerSingleTemplate, container);
            playerSingleTransform.gameObject.SetActive(true);
            LobbyPlayerSingleUI lobbyPlayerSingleUI = playerSingleTransform.GetComponent<LobbyPlayerSingleUI>();

            lobbyPlayerSingleUI.UpdatePlayer(player);
        }

        lobbyNameText.text = lobby.Name;
        
    }
    private void ClearLobby()
    {
        foreach (Transform child in container)
        {
            if (child == playerSingleTemplate) continue;
            Destroy(child.gameObject);
        }
    }
}
