using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private CharacterBrain _characterBrain;
    
    [Header("Settings")] 
    [SerializeField] private float movementSpeed;

    private void Start()
    {
        _characterBrain = GetComponent<CharacterBrain>();
    }

    private void FixedUpdate()
    {
        if (_characterBrain.IsRunning)
        {
            transform.Translate(Vector3.forward * movementSpeed * Time.fixedDeltaTime);
        }
    }
}
