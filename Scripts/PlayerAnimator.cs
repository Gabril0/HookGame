using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private PlayerController controller;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject spinningShadow;
    private float rotationAmmount = 0;
    void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        controller = GetComponentInParent<PlayerController>();
        spinningShadow.SetActive(false);
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
        animator.SetBool("IsHooking",controller.playerHooked);

        spinningShadow.SetActive(controller.playerHooked);

        if (controller.hittedHook)
        {
            rotationAmmount = -rb.velocity.x;
            transform.Rotate(Vector3.forward, rotationAmmount);
        }
        if(!controller.playerHooked) {
            transform.rotation = Quaternion.identity;
        }
    }
}
