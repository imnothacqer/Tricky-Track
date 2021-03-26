using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class CharacterShoot : MonoBehaviour
{
    [Header("Settings")] 
    public float shootRadius;
    [Header("References")] 
    public Shooter shooter;

    public CharacterBrain characterBrain;
    
    private float inputHorizontal;
    private float inputVertical;


    private void Start()
    {
        shooter = GetComponentInChildren<Shooter>();
        characterBrain = GetComponent<CharacterBrain>();
    }

    private void Update()
    {
        
        inputHorizontal = SimpleInput.GetAxis("Horizontal");
        inputVertical = SimpleInput.GetAxis("Vertical");

        if (Input.GetMouseButtonDown(0) && characterBrain.CanShoot)
        {
            
            Vector3 direction = CalculateDirection();
            shooter.LaunchProjectile(direction);



            if (Input.GetMouseButtonUp(0))
            {
                shooter.Shoot();
            }
        }
        
    }

    private Vector3 CalculateDirection()
    {
        Vector3 direction = Vector3.forward * 10f;
        direction.x += inputHorizontal * shootRadius;
        direction.y += inputVertical * shootRadius;
        
        return direction;
    }

    private bool IsInputZero()
    {
        return inputHorizontal != 0 || inputVertical != 0;
    }
}
