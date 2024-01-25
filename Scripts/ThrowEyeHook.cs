using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowEyeHook : MonoBehaviour
{
    [SerializeField] float speed;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.up * speed;
    }
}
