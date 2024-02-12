using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Geiser : MonoBehaviour
{
    [SerializeField] private float geiserDuration;
    [SerializeField] private float geiserCD;
    private float time;

    private BoxCollider2D col;
    private SpriteRenderer render;
    private void Start()
    {
        col = GetComponent<BoxCollider2D>();
        render = GetComponent<SpriteRenderer>();
        col.enabled = false;
        time = 0;
    }
    private void Update()
    {
        time += Time.deltaTime;
        if (time > geiserCD)
        {
            col.enabled = true;
            render.color = Color.red;
            Invoke("resetTimer", geiserDuration);
        }
    }
    private void resetTimer() {
        time = 0;
        col.enabled = false;
        render.color = Color.gray;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().isAlive = false;
        }
    }
}
