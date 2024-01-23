using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.velocityX > 0.1f)
        {
            sprite.flipX = false;
        }
        else if (rb.velocityX < -0.1f)
        {
            sprite.flipX = true;
        }
    }
}
