using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterShoot : MonoBehaviour
{
    [Header("Settings")] 
    public float shootRadius;
    public LayerMask shootLayer;
    public float shootRange;

    [Header("References")] 
    public CharacterBrain characterBrain;
    public GameObject ballPrefab;
    public GameObject cursor;

    [Header("Line Settings")] 
    public GameObject lineDotPrefab;
    public int dotCount;



    private List<GameObject> lineDotList = new List<GameObject>();
    private float shootTime = 1f;
    private float inputHorizontal;
    private float inputVertical;

    private Rigidbody selectedBall;
    private bool readyForShoot;

    private bool isAiming;

    public bool IsAiming
    {
        get
        {
            return isAiming;
        }
        set
        {
            isAiming = value;
            ToggleDotLine(value);
            cursor.SetActive(value);
        }
    }


    private void Start()
    {
        characterBrain = GetComponentInParent<CharacterBrain>();
        GameManager.instance.OnStartGame += SpawnBall;

        for (int i = 0; i < dotCount; i++)
        {
            GameObject dot = Instantiate(lineDotPrefab);
            dot.SetActive(false);
            lineDotList.Add(dot);
        }
    }

    private void Update()
    {
        
        inputHorizontal = SimpleInput.GetAxis("Horizontal");
        inputVertical = SimpleInput.GetAxis("Vertical");

        IsAiming = !IsInputZero();

        if (IsAiming  && characterBrain.CanShoot)
        {
            Vector3 targetVelocity = LaunchBall();

            if (IsAiming && Input.GetMouseButtonUp(0))
            {
                Shoot(targetVelocity);
                IsAiming = false;
            }
        }
        
    }

    private Vector3 LaunchBall()
    {
        Vector3 Velocity;
        RaycastHit hit;
        Vector3 direction = CalculateDirection();
        Ray rayToTarget = new Ray(transform.position, direction);
        bool isHit = Physics.Raycast(rayToTarget, out hit, shootRange, shootLayer);
        
        if (isHit)
        {
            cursor.transform.position = hit.point;
            Velocity = TrajectoryHelper.CalculateVelocity(hit.point, transform.position, shootTime);
        }
        else
        {
            Vector3 raycastLastPoint = transform.position + rayToTarget.direction * (shootRange / 3);
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
            selectedBall = Instantiate(ballPrefab, transform.position, Quaternion.identity, transform).GetComponent<Rigidbody>();
            selectedBall.isKinematic = true;
            readyForShoot = true;
        }
    }

    private Vector3 CalculateDirection()
    {
        Vector3 direction = Vector3.forward * 10f;
        direction.x += inputHorizontal * shootRadius;
        direction.y += inputVertical * shootRadius;
        
        return direction;
    }
    
    private void Visualize(Vector3 velocity)
    {
        for (int i = 0; i < dotCount; i++)
        {
            Vector3 pos = TrajectoryHelper.CalculatePosInTime(transform.position,velocity, i / (float) dotCount);
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
    
    private bool IsInputZero()
    {
        return inputHorizontal == 0 || inputVertical == 0;
    }
}
