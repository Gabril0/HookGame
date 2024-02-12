using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempBlock : MonoBehaviour
{
    [SerializeField] private float destructionTime;
    [SerializeField] private float renableTime;
    private BoxCollider2D col;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        col = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player")) {
            Invoke("DisableBlock",destructionTime);
        }
    }
    private void DisableBlock() {
        col.enabled = false;
        spriteRenderer.enabled = false;
        Invoke("RenableBlock", renableTime);
    }
    private void RenableBlock() {
        col.enabled = true;
        spriteRenderer.enabled = true;
    }
}
