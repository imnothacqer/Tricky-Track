using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoosterPlatform : MonoBehaviour
{
    private bool isBoosted;
    public GameObject boosterArrow;

    public Material boostedMaterial;
    public Material slowedMaterial;

    private Renderer arrowRenderer;

    private void Start()
    {
        arrowRenderer = boosterArrow.GetComponent<Renderer>();
    }


    public bool IsBoosted
    {
        get
        {
            return isBoosted;
        }
        set
        {
            isBoosted = value;
            arrowRenderer.material = value ? boostedMaterial : slowedMaterial;
        }
    }
    
}
