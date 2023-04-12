using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class UIManager : MonoBehaviour
{

    [Header("Panels")]
    [SerializeField]private GameObject connectPanel;
    [SerializeField]private GameObject waitingPanel;
    [SerializeField]private GameObject gamePanel;

    // Start is called before the first frame update
    void Start(){

        ShowConnectionPanel();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ShowConnectionPanel(){
        connectPanel.SetActive(true);
        waitingPanel.SetActive(false);
        gamePanel.SetActive(false);
    }

    private void ShowWaitingPanel(){
        connectPanel.SetActive(false);
        waitingPanel.SetActive(true);
        gamePanel.SetActive(false);
    }

    private void ShowGamePanel(){
        connectPanel.SetActive(false);
        waitingPanel.SetActive(false);
        gamePanel.SetActive(true);
    }

    public void HostButtonCallBack(){
        NetworkManager.Singleton.StartHost();
        ShowWaitingPanel();
    }

    public void ClientButtonCallBack(){
        NetworkManager.Singleton.StartClient();
        ShowWaitingPanel();
    }
}
