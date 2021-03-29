using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("UI References")] 
    public GameObject FinishGamePanel;
    public GameObject GameOverPanel;
    
    public static GameManager instance;

    public Action OnStartGame;
    public Action OnStopGame;
    public Action OnGameOver;
    public Action OnFinishTrigger;
    public Action OnFinish;
    

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

    private void Start()
    {
        OnGameOver += GameOver;
        OnFinish += FinishGame;
    }

    public void StartGame()
    {
        OnStartGame?.Invoke();
    }

    public void StopGame()
    {
        OnStopGame?.Invoke();
    }

    public void GameOver()
    {
        GameOverPanel.SetActive(true);
    }

    public void FinishGame()
    {
        FinishGamePanel.SetActive(true);
    }
}
