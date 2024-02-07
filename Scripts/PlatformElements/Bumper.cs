using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            PlayerController controller = collision.gameObject.GetComponent<PlayerController>();

            Vector2 direction = new Vector2(Mathf.Sign(playerRb.velocityX), Mathf.Sign(playerRb.velocityY));

            controller.tempVelocity = direction * 20;
        }
    }


}
