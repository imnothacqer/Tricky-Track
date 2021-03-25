using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour
{
    [Header("References")]
    public GameObject objectPrefab;

    [Header("Settings")] 
    public int startWith = 10;
    [Tooltip("If type '0' is unlimited")] 
    public int maxObject = 0;

    private List<GameObject> pool;

    private void Start()
    {
        if (startWith != 0)
        {
            for (int i = 0; i < startWith; i++)
            {
                GameObject createdObject = CreateObject();
                pool.Add(createdObject);
            }
        }
    }

    public GameObject GetObject()
    {
        foreach (GameObject selected in pool)
        {
            if (!selected.active)
            {
                return selected;
            }
        }

        GameObject createdObject = CreateObject();
        pool.Add(createdObject);
        return createdObject;
    }

    public void TakeInPool(GameObject outObject)
    {
        outObject.transform.position = Vector3.zero;
        outObject.transform.rotation = Quaternion.identity;

        outObject.SetActive(false);
        outObject.name = "PoolObject";
    }

    public void SetAllDisable()
    {
        foreach (GameObject selected in pool)
        {
            selected.SetActive(false);
        }
    }

    public void DestrolAll()
    {
        foreach (GameObject selected in pool)
        {
            pool.Remove(selected);
            Destroy(selected);
        }
    }

    public GameObject CreateObject()
    {
        GameObject newObject = Instantiate(objectPrefab, Vector3.zero, Quaternion.identity, null);
        newObject.SetActive(false);
        newObject.name = "PoolObject";
        return newObject;
    }
}
