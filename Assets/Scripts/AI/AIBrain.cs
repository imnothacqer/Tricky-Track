using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIBrain : MonoBehaviour
{
    [Header("Settings")] public float movementSpeed;

    public Vector3 targetPosition;

    [Header("Target Detector Settings")] public float detectRadius;
    public LayerMask targetMask;

    [Header("Character References")] [SerializeField]
    private Animator enemyAnimator;

    [Header("Animator Settings")] [SerializeField]
    private bool isRunning;

    [SerializeField] private bool isHitted;
    [SerializeField] private bool canShoot;


    private Transform playerTransform;

    public bool IsRunning
    {
        get { return isRunning; }
        set
        {
            isRunning = value;
            enemyAnimator.SetBool("isRunning", value);
        }
    }

    public bool IsHitted
    {
        get { return isHitted; }
        set
        {
            isHitted = value;
            IsRunning = !value;
            enemyAnimator.SetBool("isHitted", value);
        }
    }

    public bool CanShoot
    {
        get { return canShoot; }
        set { canShoot = value; }
    }

    protected bool isBucketHitted;

    private void Start()
    {
        enemyAnimator = GetComponentInChildren<Animator>();
        GameManager.instance.OnStartGame += Begin;
        GameManager.instance.OnFinishTrigger += Stop;
        GameManager.instance.OnGameOver += Stop;

        playerTransform = FindObjectOfType<CharacterBrain>().transform;
    }

    private void Update()
    {
        Movement();

        DetectTarget();
    }

    private void Begin()
    {
        IsRunning = true;
        CanShoot = true;
    }

    private void Stop()
    {
        IsRunning = false;
        CanShoot = false;
    }

    private void Movement()
    {
        if (IsRunning)
        {
            transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
        }
    }

    private void DetectTarget()
    {
        Collider[] objects = Physics.OverlapSphere(transform.position, detectRadius, targetMask);
        foreach (Collider selected in objects)
        {
            if (targetPosition != Vector3.zero)
            {
                return;
            }

            if (selected.gameObject.CompareTag("Target"))
            {
                if (transform.position.z > selected.transform.position.z)
                {
                    return;
                }

                if (selected.transform.position.x < 1)
                {
                    TargetBrain targetBrain = selected.GetComponent<TargetBrain>();
                    if (targetBrain != null && !targetBrain.isOpen)
                    {
                        targetPosition = targetBrain.transform.position;
                        return;
                    }
                }

                if (selected.transform.position.x > 2)
                {
                    TargetBrain targetBrain = selected.GetComponent<TargetBrain>();
                    if (targetBrain != null && targetBrain.isOpen)
                    {
                        targetPosition = targetBrain.transform.position;
                        return;
                    }
                }

                if (selected.transform.position.x > 1 && selected.transform.position.y > 3)
                {
                    BucketBrain bucketBrain = selected.GetComponentInParent<BucketBrain>();

                    if (bucketBrain)
                    {
                        if (selected.transform.position.z - 5 >= playerTransform.position.z)
                        {
                            targetPosition = selected.transform.position;
                        }
                    }
                }
            }
        }
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("BucketBall"))
        {
            if (isBucketHitted)
            {
                return;
            }

            isBucketHitted = true;
            IsHitted = true;
            StartCoroutine("GetUpAfterHitted");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("FinishTrigger"))
        {
            GameManager.instance.OnGameOver?.Invoke();
        }
    }

    private IEnumerator GetUpAfterHitted()
    {
        yield return new WaitForSeconds(1.2f);
        IsHitted = false;
        yield return new WaitForSeconds(1.9f);
        isBucketHitted = false;
    }
}