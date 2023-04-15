using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
using UnityEngine.SceneManagement;
using Unity.Netcode.Transports.UTP;

public class UIManager : MonoBehaviour
{

    [Header("Panels")]
    [SerializeField]private GameObject connectPanel;
    [SerializeField]private GameObject waitingPanel;
    [SerializeField]private GameObject gamePanel;

    [Header("Win Lose Panels")]
    [SerializeField]private GameObject winPanel;
    [SerializeField]private GameObject losePanel;

    // Start is called before the first frame update
    void Start(){

        ShowConnectionPanel();

        GameManager.onGameStateChanged += GameStateChangedCallback; // subscribe ao event
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy() {
        GameManager.onGameStateChanged -= GameStateChangedCallback; // unsubscribe ao event
    }

    private void GameStateChangedCallback(GameManager.State gameState){
        
        switch(gameState){
            case GameManager.State.Game:
                ShowGamePanel();
                break;
            case GameManager.State.Win:
                ShowWinPanel();
                break;
            case GameManager.State.Lose:
                ShowLosePanel();
                break;
        }

    }

    private void ShowConnectionPanel(){
        connectPanel.SetActive(true);
        waitingPanel.SetActive(false);
        gamePanel.SetActive(false);

        winPanel.SetActive(false);
        losePanel.SetActive(false);
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

    private void ShowWinPanel(){
        winPanel.SetActive(true);
    }

    private void ShowLosePanel(){
        losePanel.SetActive(true);
    }

    public void HostButtonCallBack(){
        //NetworkManager.Singleton.StartHost();
        ShowWaitingPanel();
        RelayManager.instance.StartCoroutine(
            RelayManager.instance.ConfigureTransportAndStartNgoAsHost()
        );
    }

    public void ClientButtonCallBack(){

        // Let´s grab the IP address that player has entered
        // Vamos pegar o IP Address que o player teve entrado

        //string ipAddress = IPManager.instance.GetInputIP();

        // Configure the Network Manager
        //UnityTransport utp = NetworkManager.Singleton.GetComponent<UnityTransport>();
        //utp.SetConnectionData(ipAddress, 7777);

        //NetworkManager.Singleton.StartClient();

        RelayManager.instance.StartCoroutine(
            RelayManager.instance.ConfigureTransportAndStartNgoAsConnectingPlayer()
        );

        ShowWaitingPanel();
    }

    public void NextButtonCallback(){   // reinicia a cena do game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        NetworkManager.Singleton.Shutdown();

    }
}
