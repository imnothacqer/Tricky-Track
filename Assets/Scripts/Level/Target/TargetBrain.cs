using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TargetBrain : MonoBehaviour
{
    [Header("Settings")]
    public bool isOpen;

    [Header("References")]
    public GameObject targetCircleObject;
    
    [Header("Events")]
    public UnityEvent OnOpen;
    public UnityEvent OnClose;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            isOpen = !isOpen;
            ToggleMaterial();
            if (isOpen)
            {
                OnOpen?.Invoke();    
            }
            else
            {
                OnClose?.Invoke();
            }
        }
    }

    private void ToggleMaterial()
    {
        Renderer targetRenderer = targetCircleObject.GetComponent<Renderer>();
        targetRenderer.materials[0].color = isOpen ? Color.green : Color.red;
        targetRenderer.materials[1].color = isOpen ? Color.green : Color.red;
    }
}
