using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
using TMPro;

public class ScoreManager : NetworkBehaviour
{

    [Header("Elements")]
    [SerializeField] TextMeshProUGUI scoreText;
    private int hostScore;
    private int clientScore;

    public override void OnNetworkSpawn(){
        base.OnNetworkSpawn();

        NetworkManager.OnServerStarted += NetworkManager_OnServerStarted;
        
    }

    private void NetworkManager_OnServerStarted(){
        
        if(!IsServer)
            return;

        Egg.onFellInWater += EggFellInWaterCallback;
        GameManager.onGameStateChanged += GameStateChangedCallback;

    }

    public override void OnDestroy(){
        base.OnDestroy();

        NetworkManager.OnServerStarted -= NetworkManager_OnServerStarted;
        Egg.onFellInWater -= EggFellInWaterCallback;
        GameManager.onGameStateChanged -= GameStateChangedCallback;
    }

    // Start is called before the first frame update
    void Start(){
        updateScoreText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GameStateChangedCallback(GameManager.State gameState){
        switch(gameState){
            case GameManager.State.Game:
                ResetScores();
                break;
        }
    }

    private void EggFellInWaterCallback(){
        if(PlayerSelector.instance.IsHostTurn()){
            clientScore++;
        }else{
            hostScore++;
        }

        // Update do display score
        UpdateScoreClientRpc(hostScore, clientScore);
        updateScoreText();

        CheckForEndGame();

    }

    [ClientRpc]
    private void UpdateScoreClientRpc(int hostScore, int clientScore){
        this.hostScore = hostScore;
        this.clientScore = clientScore;
    }

    private void updateScoreText(){
        UpdateScoreTextClientRpc();        
    }

    [ClientRpc]
    private void UpdateScoreTextClientRpc(){
        scoreText.text = "<color=#0055ffff>"+ hostScore +" </color>-<color=#ff5500ff> "+ clientScore +"</color>";
    }

    private void ResetScores(){
        hostScore = 0;
        clientScore = 0;

        UpdateScoreClientRpc(hostScore, clientScore);
        updateScoreText();
    }

    private void CheckForEndGame(){
        if(hostScore >= 3){
            // host wins
            HostWin();

        }else if(clientScore >= 3){
            // client wins
            ClientWin();
        }else{
            // respawn the egg
            ReuseEgg();
        }
    }

    private void HostWin(){
        HostWinClientRpc();
    }

    [ClientRpc]
    private void HostWinClientRpc(){
        if(IsServer)
            GameManager.instance.SetGameState(GameManager.State.Win);
        else
            GameManager.instance.SetGameState(GameManager.State.Lose);
    }

    private void ClientWin(){
        ClientWinClientRpc();
    }

    [ClientRpc]
    private void ClientWinClientRpc(){
        if(IsServer)
            GameManager.instance.SetGameState(GameManager.State.Lose);
        else
            GameManager.instance.SetGameState(GameManager.State.Win);
    }

    private void ReuseEgg(){
        EggManager.instance.ReuseEgg();
    }
}
