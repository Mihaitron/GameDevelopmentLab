using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Leave : MonoBehaviour
{
    public void LeaveScene()
    {
        SceneManager.LoadScene(0);
    }
}
