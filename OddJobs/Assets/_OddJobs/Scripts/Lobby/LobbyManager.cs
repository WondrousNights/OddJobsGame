using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using System;
using Unity.Netcode;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager Instance { get; private set; }
    public const string KEY_START_GAME = "0";

    [SerializeField] GameObject lobbyUI;

    private Lobby hostLobby;
    private Lobby joinedLobby;

    private string playerName;

    private float lobbyUpdateTimer = 2f;
    private float lobbyHeartbeatTimer;


    public event EventHandler<LobbyEventArgs> OnJoinedLobby;
    // public event EventHandler<LobbyEventArgs> OnJoinedLobbyUpdate;
    //public event EventHandler<LobbyEventArgs> OnKickedFromLobby;
    // public event EventHandler<LobbyEventArgs> OnLobbyGameModeChanged;

    ILobbyEvents m_LobbyEvents;

    public Lobby GetJoinedLobby()
    {
        return joinedLobby;
    }

    public void DisableLobbyUI()
    {
        lobbyUI.SetActive(false);
    }
    public class LobbyEventArgs : EventArgs
    {
        public Lobby lobby;
    }

    public event EventHandler<OnLobbyListChangedEventArgs> OnLobbyListChanged;
    public class OnLobbyListChangedEventArgs : EventArgs
    {
        public List<Lobby> lobbyList;
    }

    // Start is called before the first frame update
    private void Start()
    {
        Instance = this;

        NetworkManager.Singleton.OnClientStopped += OnServerStopped;
        NetworkManager.Singleton.OnServerStopped += OnServerStopped;
        
    }

    private void OnServerStopped(bool obj)
    {
        LeaveLobby();
        RelayManager.LeaveRelay();
        Loader.Load(Loader.Scene.Menu);
    }

    public async void Authenticate()
    {

        if(AuthenticationService.Instance.IsSignedIn) return;

        AuthenticationService.Instance.SignedIn += () => {
            // do nothing
            Debug.Log("Signed in! " + AuthenticationService.Instance.PlayerId);

            //RefreshLobbyList();
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    private void Update()
    {
       
        
        LobbyHeartbeat();
        HandleLobbyPollForUpdates();
    }
    public async void CreateLobby()
    {
        Debug.Log("Attempting to create lobby");
        try
        {
            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions
            {
                IsPrivate = false,
                Player = GetPlayer(),
                Data = new Dictionary<string, DataObject>
                {
                    { KEY_START_GAME, new DataObject(DataObject.VisibilityOptions.Member, "0")}
                }
            };
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync("Lobby", 4, createLobbyOptions);
            hostLobby = lobby;
            joinedLobby = lobby;
            
            Debug.Log("Created lobby with 4 players with code : " + lobby.LobbyCode);

            StartGame();
        }
        catch {
            Debug.Log("Lobby was no created");
        }
      
    }

    private void OnLobbyDeleted()
    {
        RelayManager.LeaveRelay();
        Loader.Load(Loader.Scene.Menu);
    }

    public async void ListLobbies()
    {
        try
        {
            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();

            foreach (Lobby lobby in queryResponse.Results)
            {
                //List code goes here
            }
        }
        catch {
            Debug.Log("Query Response Failed");
        }
    }

    private async void LobbyHeartbeat()
    {
        if (hostLobby != null)
        {
            lobbyHeartbeatTimer -= Time.deltaTime;
            if (lobbyHeartbeatTimer < 0f)
            {
                lobbyHeartbeatTimer = 15f;
                await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
            }
            
        }
       
    }

    public async void JoinLobby(Lobby lobby)
    {
        Player player = GetPlayer();

        joinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobby.Id, new JoinLobbyByIdOptions
        {
            Player = player
        });
        var callbacks = new LobbyEventCallbacks();
        callbacks.KickedFromLobby += OnKickedFromLobby;
        callbacks.LobbyDeleted += OnKickedFromLobby;
        m_LobbyEvents = await Lobbies.Instance.SubscribeToLobbyEventsAsync(joinedLobby.Id, callbacks);

        OnJoinedLobby?.Invoke(this, new LobbyEventArgs { lobby = lobby });
    }

    private void OnKickedFromLobby()
    {
    // These events will never trigger again, so let’s remove it.
    this.m_LobbyEvents = null;
    // Refresh the UI in some way
    

    if(joinedLobby != null)
    {
        LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);
    }
    RelayManager.LeaveRelay();
    Loader.Load(Loader.Scene.Menu);
    
    }


    public async void QuickJoinLobby()
    {
        try {
             Lobby lobby = await LobbyService.Instance.QuickJoinLobbyAsync();
            joinedLobby = lobby;
        }
        catch
        {
            Debug.Log("Can't quick join");
        }
      
    }


   public void PrintPlayers()
    {
        PrintPlayers(joinedLobby);
    }
    void PrintPlayers(Lobby lobby)
    {
        foreach (Player player in lobby.Players)
        {
            Debug.Log(player.Data["PlayerName"].Value);
        }
    }

    private Player GetPlayer()
    {
        return new Player
        {
            Data = new Dictionary<string, PlayerDataObject>
            {
                { "PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName) }
            }
        };
    }

    private async void HandleLobbyPollForUpdates()
    {
        if (joinedLobby != null)
        {
            lobbyUpdateTimer -= Time.deltaTime;
            if (lobbyUpdateTimer < 0f)
            {
                lobbyUpdateTimer = 1.1f;
                Lobby lobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);
                joinedLobby = lobby;


                if (joinedLobby.Data[KEY_START_GAME].Value != "0")
                {
                    if (!IsLobbyHost())
                    {
                        RelayManager.JoinRelay(joinedLobby.Data[KEY_START_GAME].Value);
                    }

                    if(lobbyUI != null)
                    {
                        lobbyUI.SetActive(false);
                    }
                   
                }
            }


            
        }
    }

    public async void RefreshLobbyList()
    {
        try
        {
            QueryLobbiesOptions options = new QueryLobbiesOptions();
            options.Count = 25;

            // Filter for open lobbies only
            options.Filters = new List<QueryFilter> {
                new QueryFilter(
                    field: QueryFilter.FieldOptions.AvailableSlots,
                    op: QueryFilter.OpOptions.GT,
                    value: "0")
            };

            // Order by newest lobbies first
            options.Order = new List<QueryOrder> {
                new QueryOrder(
                    asc: false,
                    field: QueryOrder.FieldOptions.Created)
            };

            QueryResponse lobbyListQueryResponse = await Lobbies.Instance.QueryLobbiesAsync();

            OnLobbyListChanged?.Invoke(this, new OnLobbyListChangedEventArgs { lobbyList = lobbyListQueryResponse.Results });
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void LeaveLobby()
    {
        try
        {
            if (hostLobby != null)
            {
                await LobbyService.Instance.DeleteLobbyAsync(joinedLobby.Id);
                Loader.Load(Loader.Scene.Menu);
                RelayManager.LeaveRelay();
            }
            else
            {
                await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);
                Loader.Load(Loader.Scene.Menu);
                RelayManager.LeaveRelay();
            }
           
        }
        catch
        {
            Debug.Log("You can't leave");
        }
        
    }

    private async void MigrateLobbyHost()
    {
        try
        {
            hostLobby = await Lobbies.Instance.UpdateLobbyAsync(hostLobby.Id, new UpdateLobbyOptions
            {
                HostId = joinedLobby.Players[1].Id
            });
            joinedLobby = hostLobby;
            PrintPlayers(hostLobby);
        }
        catch { }
    }

    public async void StartGame()
    {
        if (IsLobbyHost())
        {
            try {
                string relayCode = await RelayManager.CreateRelay();

                Lobby lobby = await Lobbies.Instance.UpdateLobbyAsync(joinedLobby.Id, new UpdateLobbyOptions
                {
                    Data = new Dictionary<string, DataObject>
                    {
                        { KEY_START_GAME, new DataObject(DataObject.VisibilityOptions.Member, relayCode) }
                    }
                });

                joinedLobby = lobby;
            }
            catch { }
        }
       
    }

    public bool IsLobbyHost()
    {
        return joinedLobby != null && joinedLobby.HostId == AuthenticationService.Instance.PlayerId;
    }

    void OnApplicationQuit()
    {
        if(hostLobby != null)
        {
             LobbyService.Instance.DeleteLobbyAsync(joinedLobby.Id);
             RelayManager.LeaveRelay();
             NetworkManager.Singleton.Shutdown();
        }
    }
}
