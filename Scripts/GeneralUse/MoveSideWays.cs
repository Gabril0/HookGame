using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class MoveSideWays : MonoBehaviour
{
    private RectTransform rect;
    [SerializeField] private float movement = 5;
    void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    void Update()
    {
        rect.position = new Vector3(rect.position.x + (Mathf.Sin(Time.time))/movement, rect.position.y, rect.position.z);
    }
}
