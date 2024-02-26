using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour
{
    [SerializeField] private AudioClip boing;
    [SerializeField] private AudioSource source;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            source.clip = boing;
            source.Play();
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            PlayerController controller = collision.gameObject.GetComponent<PlayerController>();

            Vector2 direction = new Vector2(Mathf.Sign(playerRb.velocityX), Mathf.Sign(playerRb.velocityY));

            controller.tempVelocity = direction * 20;
        }
    }


}
