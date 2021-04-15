using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class InGameHandler : MonoBehaviour
{
    public NetworkManager networkManager;

    private void Start()
    {
        string ip = PlayerPrefs.GetString("ip");
        int client = PlayerPrefs.GetInt("client");

        PlayerPrefs.DeleteAll();

        this.networkManager.networkAddress = ip;

        switch (client)
        {
            case 0:
                this.networkManager.StartHost();
                break;

            case 1:
                this.networkManager.StartClient();
                break;

            default:
                this.networkManager.StartServer();
                break;
        }
    }
}
