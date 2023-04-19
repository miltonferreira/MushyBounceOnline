using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class MatchmakingManager : MonoBehaviour
{

    public static MatchmakingManager instance;

    Lobby lobby;
    [Header("Settings")]
    [SerializeField] private string _joinCode;

    private void Awake() {
        if(instance == null){
            instance = this;
        }else{
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async void PlayButtonCallback(){
        await Authenticate();

        lobby = await QuickJoinLobby() ?? await CreateLobby();
    }

    async Task Authenticate(){
        try
        {
            await UnityServices.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            var playerID = AuthenticationService.Instance.PlayerId;
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    private async Task<Lobby> QuickJoinLobby(){
        try{

            Lobby lobby = await Lobbies.Instance.QuickJoinLobbyAsync();

            JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(
                lobby.Data[_joinCode].Value);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(
                allocation.RelayServer.IpV4,
                (ushort)allocation.RelayServer.Port,
                allocation.AllocationIdBytes,
                allocation.Key,
                allocation.ConnectionData,
                allocation.HostConnectionData
            );

            NetworkManager.Singleton.StartClient();
            return lobby;

        }catch(Exception e){
            Debug.Log(e);
            return null;
        }
    }

    private async Task<Lobby> CreateLobby(){
        try{

            int maxPlayers = 2;
            string lobbyName = "MyCoolLobby";

            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxPlayers);
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            CreateLobbyOptions options = new CreateLobbyOptions();
            options.Data = new Dictionary<string, DataObject>{{ 
                _joinCode, new DataObject(DataObject.VisibilityOptions.Public, joinCode)}};

            Lobby lobby = await Lobbies.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);

            // Ping the lobby to prevent it from turning to inactive mode
            StartCoroutine(HeartbeatLobbyCoroutine(lobby.Id, 15));

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(
                allocation.RelayServer.IpV4,
                (ushort) allocation.RelayServer.Port,
                allocation.AllocationIdBytes,
                allocation.Key,
                allocation.ConnectionData
            );

            NetworkManager.Singleton.StartHost();

            return lobby;

        }catch(Exception e){
            Debug.Log(e);
            return null;
        }
    }

    IEnumerator HeartbeatLobbyCoroutine(string lobbyId, float waitTimeSeconds){
        var delay = new WaitForSecondsRealtime(waitTimeSeconds);

        while(true){
            LobbyService.Instance.SendHeartbeatPingAsync(lobbyId);
            yield return delay;
        }
    }

}
