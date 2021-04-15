using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mirror;

public class MainMenu : MonoBehaviour
{
    public int nextScene;
    public InputField ipInput;

    public void HostGame()
    {
        StartCoroutine(HostGameAsync());
        PlayerPrefs.SetInt("client", 0);
    }

    private IEnumerator HostGameAsync()
    {
        this.SetIpAddress();

        yield return null;

        AsyncOperation async_load = SceneManager.LoadSceneAsync(this.nextScene);
        while (!async_load.isDone)
        {
            yield return null;
        }
    }

    public void ConnectToGame()
    {
        StartCoroutine(ConnectToGameAsync());
        PlayerPrefs.SetInt("client", 1);
    }

    private IEnumerator ConnectToGameAsync()
    {
        this.SetIpAddress();

        yield return null;

        AsyncOperation async_load = SceneManager.LoadSceneAsync(this.nextScene);
        while (!async_load.isDone)
        {
            yield return null;
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void SetIpAddress()
    {
        string ip = this.ipInput.text;

        PlayerPrefs.SetString("ip", ip == "" ? "localhost" : ip);
    }
}
