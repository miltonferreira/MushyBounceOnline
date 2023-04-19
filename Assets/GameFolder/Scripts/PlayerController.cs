using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerController : NetworkBehaviour
{
    [Header("Control Settings")]
    [SerializeField]private float moveSpeed;
    [SerializeField]private float maxX;     // limita movimentação do player, para não sair da tela
    private float clickedScreenX;
    private float clickedPlayerX;           // pega posicao atual do player na tela

    [Header("Animations")]
    [SerializeField] private Animator animator;
    [SerializeField] private Animator bodyAnimator;
    [SerializeField] private float animatorSpeedMultiplier;
    [SerializeField] private float animatorSpeedLerp;

    [Header("Events")]
    public static Action onBump;

    // Start is called before the first frame update
    void Start(){
        
        if(IsOwner)
            animator.Play("Idle");
    }

    // Update is called once per frame
    void Update()
    {
        if(IsOwner)
            ManageControl();
    }

    private void ManageControl(){
        if(Input.GetMouseButtonDown(0)){
            
            clickedScreenX = Input.mousePosition.x; // cria posicao 0 ao clicar na tela
            clickedPlayerX = transform.position.x;

            animator.speed = 1;

        }else if(Input.GetMouseButton(0)){
            float xDifference = Input.mousePosition.x - clickedScreenX;
            xDifference /= Screen.width;
            xDifference *= moveSpeed;

            float newXPosition = clickedPlayerX + xDifference;      // pega posicao antiga do player, mas novo valor do mouse/touch

            newXPosition = Mathf.Clamp(newXPosition, -maxX, maxX);  // compara menor e maior valor

            transform.position = new Vector2(newXPosition, transform.position.y); // movimenta ovo na tela

            //print("X difference = " + xDifference);
            UpdatePlayerAnimation();

        }else if(Input.GetMouseButtonUp(0)){
            animator.speed = 1;
            animator.Play("Idle");
        }
    }

    private float previousScreenX;
    private void UpdatePlayerAnimation(){

        float xDifference = (Input.mousePosition.x - previousScreenX) / Screen.width;
        xDifference *= animatorSpeedMultiplier;

        // valor positivo faz player mexer, caso negativo fica sem mexer
        xDifference = Mathf.Abs(xDifference);

        // cria interpolação de velocidade da animação
        float targetAnimatorSpeed = Mathf.Lerp(animator.speed, xDifference, Time.deltaTime * animatorSpeedLerp);

        animator.speed = targetAnimatorSpeed;
        animator.Play("Run");

        previousScreenX = Input.mousePosition.x;
    }

    
    public void Bump(){
        BumpClientRpc();
    }

    // indica ao client para chamar animação do host
    [ClientRpc]
    private void BumpClientRpc(){
        bodyAnimator.Play("Bump");
        onBump?.Invoke();
    }
}
