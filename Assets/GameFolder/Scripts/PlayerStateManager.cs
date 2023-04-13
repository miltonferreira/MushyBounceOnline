using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerStateManager : NetworkBehaviour
{
    [Header("Elements")]
    [SerializeField] private Collider2D collider;
    [SerializeField] private SpriteRenderer[] renderers;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Enable() {
        EnableClientRpc();
    }

    [ClientRpc]
    private void EnableClientRpc(){

        collider.enabled = true;

        foreach(SpriteRenderer renderer in renderers){
            Color color = renderer.color;
            color.a = 1f;
            renderer.color = color;
        }
    }

    public void Disable(){
        DisableClientRpc();
    }

    [ClientRpc]
    public void DisableClientRpc(){

        collider.enabled = false;

        foreach(SpriteRenderer renderer in renderers){
            Color color = renderer.color;
            color.a = .2f;
            renderer.color = color;
        }
    }
}
