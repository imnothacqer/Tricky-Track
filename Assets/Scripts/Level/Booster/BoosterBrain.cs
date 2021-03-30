using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class BoosterBrain : MonoBehaviour
{
    [Header("Settings")] 
    public bool isBoosted;

    [Header("Boost Setting")] 
    public float boostedSpeed = 10;
    public float normalSpeed = 7;
    

    private CharacterMovement _characterMovement;
    private AIBrain _aiBrain;
    
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _characterMovement = other.gameObject.GetComponent<CharacterMovement>();
            _aiBrain = other.gameObject.GetComponent<AIBrain>();

            if (_characterMovement)
            {
                _characterMovement.movementSpeed = isBoosted ? boostedSpeed : normalSpeed;
            }
            else if (_aiBrain)
            {
                _aiBrain.movementSpeed = isBoosted ? boostedSpeed : normalSpeed;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (_characterMovement)
            {
                _characterMovement.movementSpeed = normalSpeed;
            }else if (_aiBrain)
            {
                _aiBrain.movementSpeed = normalSpeed;
            }
        }
        
    }

    public void ToggleBooster()
    {
        isBoosted = !isBoosted;
    }
}
