﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBrain : MonoBehaviour
{
    [Header("Character References")] 
    [SerializeField] private Animator characterAnimator;

    [Header("Animator Settings")] 
    [SerializeField] private bool isRunning;
    [SerializeField] private bool isHitted;
    [SerializeField] private bool isWin;
    [SerializeField] private bool isDefeat;
    [SerializeField] private bool canShoot;

    public PoolObject ballPool = new PoolObject();

    public bool IsRunning
    {
        get
        {
            return isRunning;
        }
        set
        {
            isRunning = value;
            characterAnimator.SetBool("isRunning", value);
        }
    }
    
    public bool IsHitted
    {
        get
        {
            return isHitted;
        }
        set
        {
            isHitted = value;
            characterAnimator.SetBool("isHitted", value);
        }
    }
    
    public bool IsWın
    {
        get
        {
            return isWin;
        }
        set
        {
            isWin = value;
            characterAnimator.SetBool("isWin", value);
        }
    }
    
    public bool IsDefeat
    {
        get
        {
            return isDefeat;
        }
        set
        {
            isDefeat = value;
            characterAnimator.SetBool("isDefeat", value);
        }
    }
    
    public bool CanShoot
    {
        get
        {
            return canShoot;
        }
        set
        {
            canShoot = value;
        }
    }
    
   

    private void Start()
    {
        characterAnimator = GetComponentInChildren<Animator>();

        GameManager.instance.OnEnterGameplay += Begin;
        GameManager.instance.OnExitGameplay += Stop;
    }

    private void Begin()
    {
        IsRunning = true;
        GameManager.instance.currentGameState = GameState.OnGamePlay;
    }

    private void Stop()
    {
        IsRunning = false;
    }
}