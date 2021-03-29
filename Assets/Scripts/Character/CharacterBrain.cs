using System;
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
            IsRunning = !value;
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


    private bool isBucketHitted;
    private void Start()
    {
        characterAnimator = GetComponentInChildren<Animator>();

        GameManager.instance.OnStartGame += Begin;
        GameManager.instance.OnStopGame += Stop;
    }

    private void Begin()
    {
        IsRunning = true;
        CanShoot = true;
    }

    private void Stop()
    {
        IsRunning = false;
        CanShoot = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("BucketBall"))
        {
            if (isBucketHitted)
            {
                return;
            }

            isBucketHitted = true;
            IsHitted = true;
            StartCoroutine("GetUpAfterHitted");
        }
    }

    private IEnumerator GetUpAfterHitted()
    {
        yield return new WaitForSeconds(1.2f);
        IsHitted = false;
        yield return new WaitForSeconds(1.9f);
        isBucketHitted = false;
    }
    
    
}
