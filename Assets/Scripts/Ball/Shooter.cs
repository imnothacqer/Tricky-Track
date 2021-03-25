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
    public float afterShoot;

    private CharacterBrain characterBrain;
    private Camera cam;
    private Rigidbody selectedBall;

    private void Start()
    {
        cam = Camera.main;
        characterBrain = GetComponentInParent<CharacterBrain>();
        lineVisual.positionCount = lineSegment;

        GameManager.instance.OnStartGame += SpawnBall;
    }

    private void Update()
    {
        if (characterBrain.CanShoot)
        {
            LaunchProjectile();
        }
    }

    private void LaunchProjectile()
    {
        Ray camRay = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(camRay, out hit, 100f, layer))
        {
            cursor.SetActive(true);
            lineVisual.enabled = true;
            cursor.transform.position = hit.point + Vector3.up * 0.1f;

            Vector3 Vo = CalculateVelocity(hit.point, transform.position, 1f);

            if (useLine)
            {
                Visualize(Vo);
            }

            transform.rotation = Quaternion.LookRotation(Vo);

            if (Input.GetMouseButtonDown(0) && readyForShoot)
            {
                Shoot(Vo);
            }
        }
        else
        {
            cursor.SetActive(false);
            lineVisual.enabled = false;
        }
    }

    private void Shoot(Vector3 velocity)
    {
        if (selectedBall != null)
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
        yield return new WaitForSeconds(afterShoot);
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


    private Vector3 CalculateVelocity(Vector3 target, Vector3 origin, float time)
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