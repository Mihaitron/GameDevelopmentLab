using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class Disconnect : NetworkBehaviour
{
    public void DisconnectFromGame()
    {
        if (this.isServer && this.isClient)
        {
            NetworkManager.singleton.StopHost();
        }
        else if (this.isClientOnly)
        {
            NetworkManager.singleton.StopClient();
        }

        SceneManager.LoadScene(0);
    }
}
