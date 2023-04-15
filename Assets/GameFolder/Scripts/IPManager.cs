using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Net;
using System.Linq;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

public class IPManager : MonoBehaviour
{

    public static IPManager instance;

    [Header("Elements")]
    [SerializeField] private TextMeshProUGUI ipText;
    [SerializeField] private TMP_InputField ipInputField;


    private void Awake() {
        if(instance == null){
            instance = this;
        }else{
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start(){
        ipText.text = GetLocalIPv4();

        UnityTransport utp = NetworkManager.Singleton.GetComponent<UnityTransport>();
        utp.SetConnectionData(GetLocalIPv4(), 7777);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetInputIP(){
        return ipInputField.text;
    }

    public string GetLocalIPv4(){
        return Dns.GetHostEntry(Dns.GetHostName())
            .AddressList.First(
                f => f.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            .ToString();
    }
}
