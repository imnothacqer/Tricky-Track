using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterShoot : MonoBehaviour
{
    [Header("Settings")] public float shootRadius;
    public LayerMask shootLayer;
    public float shootRange;

    [Header("References")] public CharacterBrain characterBrain;
    public GameObject ballPrefab;
    public GameObject cursor;

    [Header("Line Settings")] public GameObject lineDotPrefab;
    public int dotCount;


    private List<GameObject> lineDotList = new List<GameObject>();
    private float shootTime = 1f;
    private float inputHorizontal;
    private float inputVertical;
    private Camera cam;

    private Rigidbody selectedBall;
    private bool readyForShoot;
    private bool isAiming;

    private bool isFinishTrigged;

    public bool IsAiming
    {
        get { return isAiming; }
        set
        {
            isAiming = value;
            ToggleDotLine(value);
            cursor.SetActive(value);
        }
    }


    private void Start()
    {
        cam = Camera.main;

        characterBrain = GetComponentInParent<CharacterBrain>();
        GameManager.instance.OnStartGame += SpawnBall;
        GameManager.instance.OnFinishTrigger += FinishTrigged;
        GameManager.instance.OnFinish += Finish;

        for (int i = 0; i < dotCount; i++)
        {
            GameObject dot = Instantiate(lineDotPrefab);
            dot.SetActive(false);
            lineDotList.Add(dot);
        }
    }

    private void Update()
    {
        if (isFinishTrigged)
        {
            IsAiming = true;
            Vector3 velocity = LaunchBall();
            
            Shoot(velocity);
            return;
        }
        
        if (characterBrain.CanShoot && (Input.GetMouseButtonDown(0) || IsAiming))
        {
            IsAiming = true;
            Vector3 velocity = LaunchBall();

            if (Input.GetMouseButtonUp(0))
            {
                Shoot(velocity);
                IsAiming = false;
            }
        }
    }

    private Vector3 LaunchBall()
    {
        Vector3 Velocity;
        RaycastHit hit;
        Ray rayToTarget = cam.ScreenPointToRay(Input.mousePosition);
        bool isHit = Physics.Raycast(rayToTarget, out hit, shootRange, shootLayer);

        if (isHit)
        {
            cursor.transform.position = hit.point;
            Velocity = TrajectoryHelper.CalculateVelocity(hit.point, transform.position, shootTime);
        }
        else
        {
            Vector3 raycastLastPoint = transform.position + rayToTarget.direction * shootRange;
            Velocity = TrajectoryHelper.CalculateVelocity(raycastLastPoint, transform.position, shootTime);
        }

        cursor.SetActive(isHit);

        Visualize(Velocity);

        return Velocity;
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
            selectedBall = Instantiate(ballPrefab, transform.position, Quaternion.identity, transform)
                .GetComponent<Rigidbody>();
            selectedBall.isKinematic = true;
            readyForShoot = true;
        }
    }

    private void Visualize(Vector3 velocity)
    {
        for (int i = 0; i < dotCount; i++)
        {
            Vector3 pos = TrajectoryHelper.CalculatePosInTime(transform.position, velocity, i / (float) dotCount);
            lineDotList[i].transform.position = pos;
        }
    }

    private void ToggleDotLine(bool newStatus)
    {
        for (int i = 0; i < dotCount; i++)
        {
            lineDotList[i].SetActive(newStatus);
        }
    }

    private void FinishTrigged()
    {
        isFinishTrigged = true;
        shootTime = 0.3f;
    }

    private void Finish()
    {
        ToggleDotLine(false);
        Destroy(selectedBall.gameObject);
        isFinishTrigged = false;
    }
}