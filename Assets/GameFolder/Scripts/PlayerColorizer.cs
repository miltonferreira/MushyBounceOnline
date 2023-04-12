using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class PlayerColorizer : NetworkBehaviour
{

    [Header("Elements")]
    [SerializeField] private SpriteRenderer[] renderers;

    public override void OnNetworkSpawn(){
        base.OnNetworkSpawn();

        if(!IsServer && IsOwner){
            ColorizeServerRpc(Color.red);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    //???????
    [ServerRpc]
    private void ColorizeServerRpc(Color color){
        ColorizeClientRpc(color);
    }

    [ClientRpc]
    private void ColorizeClientRpc(Color color){
        foreach(SpriteRenderer renderer in renderers){
            renderer.color = color;
        }
    }
}
