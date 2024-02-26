using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingPlatform : MonoBehaviour
{
    private float sinSum = 0;
    [SerializeField][Range(1f, 100f)] float platformMovement;
    [SerializeField] bool isReversed = false;
    void Start()
    {
    }

    void FixedUpdate()
    {
        if (!isReversed)
        {
            sinSum += Time.deltaTime;
            transform.position += new Vector3(0, Mathf.Sin(sinSum) / platformMovement, 0);
        }
        else {
            sinSum -= Time.deltaTime;
            transform.position += new Vector3(0, Mathf.Sin(sinSum) / platformMovement, 0);
        }
        
    }
}
