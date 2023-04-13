using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Egg : MonoBehaviour
{
    [Header("Physics Settings")]
    [SerializeField] private float bounceVelocity;
    private Rigidbody2D rig;
    private bool isAlive = true;

    [Header("Events")]
    public static Action onHit;
    public static Action onFellInWater;

    // Start is called before the first frame update
    void Start(){
        rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other) {

        if(!isAlive)
            return;

        if(other.collider.TryGetComponent(out PlayerController playerController)){
            Bounce(other.GetContact(0).normal);
            onHit?.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {

        if(!isAlive)
            return;

        if(other.CompareTag("Water")){
            onFellInWater?.Invoke();
            isAlive = false;
        }
    }

    private void Bounce(Vector2 normal){
        rig.velocity = normal * bounceVelocity;
    }
}
