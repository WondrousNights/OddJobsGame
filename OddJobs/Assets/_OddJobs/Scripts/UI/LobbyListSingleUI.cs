using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyListSingleUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI lobbyNameText;
    [SerializeField] private TextMeshProUGUI playersText;
    [SerializeField] private TextMeshProUGUI gameModeText;


    private Lobby lobby;


    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => {
            LobbyManager.Instance.JoinLobby(lobby);
            this.gameObject.SetActive(false);
        });
    }

    public void UpdateLobby(Lobby lobby)
    {
        this.lobby = lobby;

        lobbyNameText.text = lobby.Name;
        //playersText.text = lobby.Players.Count + "/" + lobby.MaxPlayers;
    }

}
