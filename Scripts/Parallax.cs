using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Parallax : MonoBehaviour
{
    [SerializeField] private GameObject bg;
    private SpriteRenderer sprite;

    private float length;


    [SerializeField] Transform cameraPosition;
    private Vector3 initialPosition;

    [SerializeField] private float parallaxSpeed = 0.5f;

    void Start()
    {
        sprite = bg.GetComponent<SpriteRenderer>();
        length = sprite.bounds.size.x;
        initialPosition = cameraPosition.position;
    }

    void Update()
    {
        Effect(bg, parallaxSpeed);
    }

    private void Effect(GameObject spriteToMove, float speed) {
        float temp = (cameraPosition.position.x * (1 - speed));
        float distance = (cameraPosition.position.x * speed);

        spriteToMove.transform.position = new Vector3(initialPosition.x + distance, cameraPosition.transform.position.y, spriteToMove.transform.position.z);

        if(temp > initialPosition.x + length) initialPosition.x += length;
        else if(temp < initialPosition.x - length) initialPosition.x -= length;
    }

}
