using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Action OnStartGame;
    public Action OnStopGame;
    public Action OnFinishTrigger;

    private void Awake()
    {
        #region Singleton

        if (instance != null && instance != this)
        {
           Destroy(gameObject);
           Debug.LogError("[GameManager] Instance already created. Destroyed this object");
        }
        else
        {
            instance = this;
        }

        #endregion
        
    }

    public void StartGame()
    {
        OnStartGame?.Invoke();
    }

    public void StopGame()
    {
        OnStopGame?.Invoke();
    }
}
