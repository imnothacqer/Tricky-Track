using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAreaCollider : MonoBehaviour
{
    public bool isStopped;
    public Transform pointTransform;
    public DoorBrain doorBrain;

    private void Start()
    {
        doorBrain = GetComponentInParent<DoorBrain>();
    }

    private void OnCollisionStay(Collision other)
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CharacterBrain characterBrain = other.gameObject.GetComponent<CharacterBrain>();
            if (!doorBrain.isDoorOpen)
            {
                
                characterBrain.IsRunning = false;
                characterBrain.transform.position = pointTransform.position;
                isStopped = true;
            }
            else
            {
                if (!characterBrain.IsRunning && isStopped)
                {
                    characterBrain.IsRunning = true;
                    isStopped = false;
                }
            }
        }
    }
}
