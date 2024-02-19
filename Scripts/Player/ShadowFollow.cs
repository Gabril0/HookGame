using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowFollow : MonoBehaviour
{
    private SpriteRenderer render;
    private PlayerController playerController;
    void Start()
    {
        render = GetComponent<SpriteRenderer>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.identity;


    }
}
