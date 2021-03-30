using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIShooter : MonoBehaviour
{
    [Header("References")] 
    public AIBrain aiBrain;
    public GameObject ballPrefab;
    
    private float shootTime = 1f;

    private Rigidbody selectedBall;
    private bool readyForShoot;
    private bool isAiming;

    private void Start()
    {
        aiBrain = GetComponentInParent<AIBrain>();
        GameManager.instance.OnStartGame += SpawnBall;
    }

    private void Update()
    {
        if (aiBrain.targetPosition != Vector3.zero && aiBrain.CanShoot)
        {
            Vector3 velocity =
                TrajectoryHelper.CalculateVelocity(aiBrain.targetPosition, transform.position, shootTime);
            
            Shoot(velocity);
            aiBrain.targetPosition = Vector3.zero;
        }
    }

    private void Shoot(Vector3 velocity)
    {
        if (selectedBall != null && readyForShoot)
        {
            readyForShoot = false;
            selectedBall.transform.parent = null;
            selectedBall.isKinematic = false;
            selectedBall.velocity = velocity;
            Destroy(selectedBall.gameObject, shootTime * 3f);
            StartCoroutine("Reload");
        }
    }
    
    private IEnumerator Reload()
    {
        yield return new WaitForSeconds(shootTime);
        selectedBall = null;
        SpawnBall();
    }

    private void SpawnBall()
    {
        if (selectedBall == null)
        {
            selectedBall = Instantiate(ballPrefab, transform.position, Quaternion.identity, transform).GetComponent<Rigidbody>();
            selectedBall.isKinematic = true;
            readyForShoot = true;
        }
    }
    
}
