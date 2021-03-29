using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBrain : MonoBehaviour
{
    [Header("Settings")] 
    public bool isDoorOpen;

    [Header("References")] 
    public Material openDoorMaterial;
    public Material closeDoorMaterial;
    public Animator doorAnimator;
    public List<GameObject> doors = new List<GameObject>();

    private bool isStopped;

    public bool IsDoorOpen
    {
        get
        {
            return isDoorOpen;
        }
        set
        {
            ToggleDoor(value);
            isDoorOpen = value;
        }
    }

    private void Start()
    {
        doorAnimator = GetComponent<Animator>();
    }
    
    public void BallHitTarget()
    {
        IsDoorOpen = !IsDoorOpen;
    }


    public void ToggleDoor(bool newState)
    {
        doorAnimator.SetBool("isDoorOpen", newState);
        foreach (GameObject door in doors)
        {
            door.GetComponent<Renderer>().material = newState ? openDoorMaterial : closeDoorMaterial;
        }
    }
}
