using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    OnMainMenu,
    OnEnterGameplay,
    OnGamePlay,
    OnExitGameplay,
    OnFinishTrigger
}
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState currentGameState;

    public Action OnMainMenu;
    public Action OnEnterGameplay;
    public Action OnGamePlay;
    public Action OnExitGameplay;
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

    private void Update()
    {
        switch (currentGameState)
        {
            case GameState.OnMainMenu:
                OnMainMenu?.Invoke();
                break;
            case GameState.OnEnterGameplay:
                OnEnterGameplay?.Invoke();
                break;
            case GameState.OnGamePlay:
                OnGamePlay?.Invoke();
                break;
            case GameState.OnExitGameplay:
                OnEnterGameplay?.Invoke();
                break;
            case GameState.OnFinishTrigger:
                OnEnterGameplay?.Invoke();
                break;
            
        }
    }
}
