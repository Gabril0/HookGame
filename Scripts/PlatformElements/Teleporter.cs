using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Teleporter : MonoBehaviour
{
    [SerializeField] Teleporter otherTeleporter;
    public SpriteRenderer spriteRenderer;
    [SerializeField] private bool spriteIsBlue;
    [SerializeField] private Sprite bluePortal;
    [SerializeField] private Sprite orangePortal;
    [SerializeField] private Light2D lightComponent;
    [SerializeField] private AudioClip sound;
    [SerializeField] private AudioSource source;
    public bool canCollide = true;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = spriteIsBlue ? bluePortal : orangePortal;
        otherTeleporter.spriteRenderer.sprite = spriteIsBlue ? orangePortal : bluePortal;
        lightComponent.color = spriteIsBlue ? Color.blue: Color.yellow ;
    }


    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && canCollide)
        {
            source.clip = sound;
            source.Play();
            otherTeleporter.canCollide = false;
            canCollide = false;
            collision.GetComponent<PlayerController>().canControl = false;
            collision.GetComponent<PlayerController>().playerHooked = false;
            collision.transform.position = otherTeleporter.transform.position;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().canControl = true;
            Invoke("renableCollisions", 0.5f);
        }
    }

    private void renableCollisions() {
        canCollide = true;
        otherTeleporter.canCollide = true;
    }
}
