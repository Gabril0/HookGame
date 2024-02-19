using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowEyeHook : MonoBehaviour
{
    [SerializeField] public float speed;
    [SerializeField] public bool foundWall = false;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += transform.up * speed;
        transform.position = new Vector3(transform.position.x, transform.position.y, -0.5f);
    }
}
