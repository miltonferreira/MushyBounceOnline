using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Control Settings")]
    [SerializeField]private float moveSpeed;
    [SerializeField]private float maxX;     // limita movimentação do player, para não sair da tela
    private float clickedScreenX;
    private float clickedPlayerX;           // pega posicao atual do player na tela

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ManageControl();
    }

    private void ManageControl(){
        if(Input.GetMouseButtonDown(0)){
            
            clickedScreenX = Input.mousePosition.x; // cria posicao 0 ao clicar na tela
            clickedPlayerX = transform.position.x;

        }else if(Input.GetMouseButton(0)){
            float xDifference = Input.mousePosition.x - clickedScreenX;
            xDifference /= Screen.width;
            xDifference *= moveSpeed;

            float newXPosition = clickedPlayerX + xDifference;      // pega posicao antiga do player, mas novo valor do mouse/touch

            newXPosition = Mathf.Clamp(newXPosition, -maxX, maxX);  // compara menor e maior valor

            transform.position = new Vector2(newXPosition, transform.position.y); // movimenta ovo na tela

            //print("X difference = " + xDifference);
        }
    }
}
