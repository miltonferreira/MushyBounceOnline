using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] private AudioSource bumpSound;

    // Start is called before the first frame update
    void Start(){
        PlayerController.onBump += PlayBumpSound;
    }

    private void OnDestroy() {
        PlayerController.onBump -= PlayBumpSound;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayBumpSound(){
        bumpSound.Play();
    }
}
