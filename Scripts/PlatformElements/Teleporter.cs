using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] Teleporter otherTeleporter;
    public bool canCollide = true;
    void Start()
    {
        
    }


    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && canCollide)
        {
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
