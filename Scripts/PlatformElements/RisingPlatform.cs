using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingPlatform : MonoBehaviour
{
    private float sinSum = 0;
    [SerializeField][Range(10f, 100f)] float platformMovement;
    void Start()
    {
    }

    void Update()
    {
        sinSum += Time.deltaTime;
        transform.position += new Vector3(0,Mathf.Sin(sinSum)/platformMovement,0);
    }
}
