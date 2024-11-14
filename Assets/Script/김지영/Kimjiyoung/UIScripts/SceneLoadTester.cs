using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoadTester : MonoBehaviour
{
    public bool gameStarted = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {            
            LoadingSceneController.Instance.LoadScene("MainUI");
        }        
    }

    public void StartGame()
    {
        if (!gameStarted)
        {
            LoadingSceneController.Instance.LoadScene("Lost Temple Scene");
        }
    }
}
