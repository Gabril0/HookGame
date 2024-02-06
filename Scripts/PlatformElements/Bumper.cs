using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour
{
    [SerializeField] float BumpForce;
    private PlayerController controller;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player")) {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            controller = playerRb.GetComponent<PlayerController>();

            controller.canControl = false;
            controller.horizontalMovement = -Mathf.Sign(playerRb.velocityX);
            playerRb.velocity = new Vector2(-playerRb.velocity.x * BumpForce, -playerRb.velocity.y * BumpForce);
            Invoke("EnablePlayerControl", 5f);
        }
    }

    private void EnablePlayerControl() {
        controller.canControl = true;
    }
}
