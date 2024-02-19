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
    void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        controller = GetComponentInParent<PlayerController>();
        spinningShadow.SetActive(false);
    }

    void LateUpdate()
    {
        animator.SetBool("isRolling",controller.isRolling);
        if (!controller.isRolling)
        {
            Invoke("ShadowDelay", 0.15f);
        }
        else {
            spinningShadow.SetActive(true);
        }
        if (!controller.isRolling) {
            transform.rotation = Quaternion.identity;
        }
    }
    private void ShadowDelay() {
        spinningShadow.SetActive(false);
    }
}
