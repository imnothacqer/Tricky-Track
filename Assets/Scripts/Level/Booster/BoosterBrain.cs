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

    [Header("References")] 
    public ParticleSystem boostParticles;

    private CharacterMovement _characterMovement;

    private void Start()
    {
        boostParticles.Stop();
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _characterMovement = other.gameObject.GetComponent<CharacterMovement>();
            if (isBoosted)
            {
                _characterMovement.movementSpeed = boostedSpeed;
            }
            else
            {
                _characterMovement.movementSpeed = normalSpeed;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _characterMovement.movementSpeed = normalSpeed;
            boostParticles.Stop();
        }
        
    }

    public void ToggleBooster()
    {
        isBoosted = !isBoosted;

        // if (isBoosted)
        // {
        //     boostParticles.Play();
        // }
        // else
        // {
        //     boostParticles.Stop();
        // }
    }
}
