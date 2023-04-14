using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class GameManager : NetworkBehaviour
{
    public static GameManager instance;

    public enum State{Menu, Game, Win, Lose}
    private State gameState;

    private int connectedPlayers;

    [Header("Events")]
    public static Action<State> onGameStateChanged;

    public override void OnNetworkSpawn(){
        base.OnNetworkSpawn();

        NetworkManager.OnServerStarted += NetworkManager_OnServerStarted;
        
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        NetworkManager.OnServerStarted -= NetworkManager_OnServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback -= Singleton_OnClientConnectedCallback;
    }

    private void NetworkManager_OnServerStarted(){
        
        if(!IsServer)
            return;

        connectedPlayers++;
        NetworkManager.Singleton.OnClientConnectedCallback += Singleton_OnClientConnectedCallback;

    }

    private void Singleton_OnClientConnectedCallback(ulong obj){
        connectedPlayers++;

        if(connectedPlayers >= 2){
            StartGame();
        }
    }

    private void Awake() {
        if(instance == null){
            instance = this;
        }else{
            Destroy(gameObject);
        }
    }


    // Start is called before the first frame update
    void Start(){
        gameState = State.Menu;
    }

    public void SetGameState(State gameState){
        this.gameState = gameState;
        onGameStateChanged?.Invoke(gameState);
    }

    private void StartGame(){
        StartGameClientRpc();
    }

    [ClientRpc]
    private void StartGameClientRpc(){
        gameState = State.Game;
        onGameStateChanged?.Invoke(gameState);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
