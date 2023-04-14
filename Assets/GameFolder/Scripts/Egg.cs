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
    private float gravityScale;

    [Header("Events")]
    public static Action onHit;
    public static Action onFellInWater;

    // Start is called before the first frame update
    void Start(){
        rig = GetComponent<Rigidbody2D>();
        isAlive = true;

        gravityScale = rig.gravityScale;
        rig.gravityScale = 0;

        // Wait 2 secods e cai
        StartCoroutine("WaitAndFall");
    }

    IEnumerator WaitAndFall(){
        yield return new WaitForSeconds(2f);
        rig.gravityScale = gravityScale;
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
            isAlive = false;
            onFellInWater?.Invoke();
        }
    }

    private void Bounce(Vector2 normal){
        rig.velocity = normal * bounceVelocity;
    }

    public void Reuse(){
        transform.position = Vector2.up * 5;
        rig.velocity = Vector2.zero;
        rig.angularVelocity = 0;
        rig.gravityScale = 0;

        isAlive = true;

        StartCoroutine("WaitAndFall");
    }
}
