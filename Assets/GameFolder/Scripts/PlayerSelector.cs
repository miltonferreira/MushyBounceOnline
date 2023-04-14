using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class PlayerSelector : NetworkBehaviour
{
    //*** Checa se é host ou client ------------------
    // fazendo o alpha do outro jogador fica mais transparente -----

    public static PlayerSelector instance;

    private bool isHostTurn;

    
    private void Awake() {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        NetworkManager.OnServerStarted += NetworkManager_OnServerStarted;
    }

    private void NetworkManager_OnServerStarted(){
        if(!IsServer)
            return;

        GameManager.onGameStateChanged += GameStateChangedCallback;
        Egg.onHit += SwitchPlayers;
    }

    public override void OnDestroy(){
        base.OnDestroy();

        NetworkManager.OnServerStarted -= NetworkManager_OnServerStarted;
        GameManager.onGameStateChanged -= GameStateChangedCallback;
        Egg.onHit -= SwitchPlayers;
    }

    private void GameStateChangedCallback(GameManager.State gameState){
        
        switch(gameState){
            case GameManager.State.Game:
                Initilize();
                break;
        }

    }

    private void Initilize(){
        //checa se é host ou client desativando a outra opção
        PlayerStateManager[] playerStateManagers = FindObjectsOfType<PlayerStateManager>();

        for(int i =0; i < playerStateManagers.Length; i++){
            if(playerStateManagers[i].GetComponent<NetworkObject>().IsOwnedByServer){
                // É o host e desativa o client
                if(isHostTurn){
                    playerStateManagers[i].Enable();
                }else{
                    playerStateManagers[i].Disable();
                }
            }else{
                if(isHostTurn){
                    playerStateManagers[i].Disable();
                }else{
                    playerStateManagers[i].Enable();
                }
            }
        }
    }

    private void SwitchPlayers(){
        isHostTurn = !isHostTurn;

        Initilize();
    }

    public bool IsHostTurn(){
        return isHostTurn;
    }

}
