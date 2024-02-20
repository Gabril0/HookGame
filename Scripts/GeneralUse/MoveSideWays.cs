using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSideWays : MonoBehaviour
{
    private RectTransform rect;
    void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    void Update()
    {
        rect.position = new Vector3(rect.position.x + (Mathf.Sin(Time.time))/5, rect.position.y, rect.position.z);
    }
}
