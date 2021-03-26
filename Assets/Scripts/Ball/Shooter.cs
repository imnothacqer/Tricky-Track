using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [Header("Line Settings")] public LineRenderer lineVisual;
    public int lineSegment;
    public bool useLine;

    [Header("References")] public Rigidbody bulletPrefab;
    public GameObject cursor;
    public Transform shootPoint;

    [Header("Settings")] public LayerMask layer;
    public bool readyForShoot;
    public float shootTime;

    private Rigidbody selectedBall;
    private Vector3 velocity;

    private void Start()
    {
        lineVisual.positionCount = lineSegment;

        GameManager.instance.OnStartGame += SpawnBall;
    }

    private void Update()
    {
    }

    public void LaunchProjectile(Vector3 direction)
    {
        Vector3 Vo;
        Debug.DrawRay(transform.position, direction, Color.green);
        RaycastHit hit;
        lineVisual.enabled = true;
        
        Ray rayToPoint = new Ray(transform.position, direction);
        if (Physics.Raycast(rayToPoint, out hit,100f))
        {
            cursor.SetActive(true);
            cursor.transform.position = hit.point;
            Vo = CalculateVelocity(hit.point, transform.position, 1f);
            
        }
        else
        {
            cursor.SetActive(false);
            Vector3 lastPosition = transform.position + rayToPoint.direction * 20f;
            Vo = CalculateVelocity(lastPosition, transform.position, 1f);
        }
        
        transform.rotation = Quaternion.LookRotation(Vo);
        velocity = Vo;
        
        Visualize(Vo);
    }

    public void Shoot()
    {
        if (selectedBall != null && readyForShoot)
        {
            readyForShoot = false;
            selectedBall.transform.parent = null;
            selectedBall.isKinematic = false;
            selectedBall.velocity = velocity;
            Destroy(selectedBall.gameObject, 3f);
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
            selectedBall = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity, transform);
            selectedBall.isKinematic = true;
            readyForShoot = true;
        }
    }


    public Vector3 CalculateVelocity(Vector3 target, Vector3 origin, float time)
    {
        Vector3 distance = target - origin;
        Vector3 distanceXZ = distance;
        distanceXZ.y = 0;

        float Sy = distance.y;
        float Sxz = distanceXZ.magnitude;

        float Vxz = Sxz / time;
        float Vy = Sy / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;

        Vector3 result = distanceXZ.normalized;
        result *= Vxz;
        result.y = Vy;

        return result;
    }

    private Vector3 CalculatePosInTime(Vector3 vo, float time)
    {
        Vector3 Vxz = vo;
        Vxz.y = 0f;

        Vector3 result = shootPoint.position + vo * time;
        float sY = (-0.5f * Math.Abs(Physics.gravity.y) * (time * time)) + (vo.y * time) + shootPoint.position.y;

        result.y = sY;

        return result;
    }

    private void Visualize(Vector3 vo)
    {
        for (int i = 0; i < lineSegment; i++)
        {
            Vector3 pos = CalculatePosInTime(vo, i / (float) lineSegment);
            lineVisual.SetPosition(i, pos);
        }
    }
}