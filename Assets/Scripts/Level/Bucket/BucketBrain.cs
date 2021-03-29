using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketBrain : MonoBehaviour
{
    [Header("Settings")] 
    public bool isBroken;
    public float explosionForce;
    public float explosionRadius;

    [Header("References")] 
    public GameObject bucketBody;
    public GameObject bucketPartsParent;
    public GameObject bucketBallsParent;
    

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            isBroken = true;
            BrokeBucket();
        }
    }

    private void BrokeBucket()
    {
        GetComponent<BoxCollider>().enabled = false;
        bucketBody.SetActive(false);
        bucketPartsParent.SetActive(true);
        
    
        foreach (Transform part in bucketPartsParent.transform)
        {
            Rigidbody partBody = part.GetComponent<Rigidbody>();
            partBody.isKinematic = false;
            Destroy(part.gameObject, 1f);
        }
        
        foreach (Transform ball in bucketBallsParent.transform)
        {
            Rigidbody ballBody = ball.GetComponent<Rigidbody>();
            ballBody.isKinematic = false;
            Destroy(ball.gameObject, 3f);
        }

        Collider[] explotionArea = Physics.OverlapSphere(bucketBallsParent.transform.position, explosionRadius);

        foreach (Collider inArea in explotionArea)
        {
            Rigidbody selectedObject = inArea.gameObject.GetComponent<Rigidbody>();

            if (selectedObject != null)
            {
                selectedObject.AddExplosionForce(explosionForce, bucketBallsParent.transform.position, explosionRadius);
            }
        }
    }
}
