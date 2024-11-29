using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Lobbies.Models;
using TMPro;

public class LobbyPlayerSingleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerNameText;

    private Player player;

    public void UpdatePlayer(Player player)
    {
        this.player = player;
        playerNameText.text = player.Data["PlayerName"].Value;

    }
}
