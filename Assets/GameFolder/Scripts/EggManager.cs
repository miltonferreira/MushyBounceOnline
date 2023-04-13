using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class EggManager : NetworkBehaviour
{
    [Header("Elements")]
    [SerializeField] private Egg eggPrefab;

    // Start is called before the first frame update
    void Start(){
        GameManager.onGameStateChanged += GameStateChangedCallback;
    }

    public override void OnDestroy(){
        base.OnDestroy();

        GameManager.onGameStateChanged -= GameStateChangedCallback;
    }

    private void GameStateChangedCallback(GameManager.State gameState){
        
        switch(gameState){
            case GameManager.State.Game:
                SpawnEgg();
                break;
        }

    }

    private void SpawnEgg(){
        if(!IsServer)
            return;

        Egg eggInstance = Instantiate(eggPrefab, Vector3.up * 5, Quaternion.identity);
        eggInstance.GetComponent<NetworkObject>().Spawn();  // faz com que egg aparece também para o cliente
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
