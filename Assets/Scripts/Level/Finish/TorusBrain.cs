using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorusBrain : MonoBehaviour
{
    public int brokeHitcount = 4;

    public GameObject torusGlass;
    public GameObject torusGlassPartsParent;
    public GameObject targetObject;

    private int currentHitCount = 0;

    public void HitCounter()
    {
        currentHitCount += 1;
        if (currentHitCount == brokeHitcount)
        {
            Destroy(targetObject);
            BrokeGlass();
            
        }
    }


    private void BrokeGlass()
    {
        torusGlass.SetActive(false);
        torusGlassPartsParent.SetActive(true);
        
        foreach (Transform part in torusGlassPartsParent.transform)
        {
            Rigidbody partBody = part.GetComponent<Rigidbody>();
            partBody.isKinematic = false;
            Destroy(part.gameObject, 2f);
        }
        
        
    }
}
