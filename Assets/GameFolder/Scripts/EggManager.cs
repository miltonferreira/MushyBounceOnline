using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class EggManager : NetworkBehaviour
{

    public static EggManager instance;

    [Header("Elements")]
    [SerializeField] private Egg eggPrefab;

    private void Awake() {
        if(instance == null){
            instance = this;
        }else{
            Destroy(gameObject);
        }
    }

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

        // cria egg na cena
        Egg eggInstance = Instantiate(eggPrefab, Vector3.up * 5, Quaternion.identity);
        eggInstance.GetComponent<NetworkObject>().Spawn();  // faz com que egg aparece também para o cliente
        eggInstance.transform.SetParent(transform);         // torna egg filho desse gameobject
    }

    public void ReuseEgg(){
        if(!IsServer)
            return;

        if(transform.childCount < 0)
            return;

        transform.GetChild(0).GetComponent<Egg>().Reuse();
        
    }
}
