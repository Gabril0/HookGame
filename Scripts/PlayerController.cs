using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float horizontalMovement;
    private float verticalMovement;
    private Vector2 tempVelocity;
    private Rigidbody2D rb;
    private bool playerHooked = false;
    private Vector2 hitPoint;
    private LineRenderer lineRenderer;
    private DistanceJoint2D distanceJoint;
    private Vector2 directionHooked;
    private bool hittedHook = false;


    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float speed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        tempVelocity = rb.velocity;
        lineRenderer = GetComponent<LineRenderer>();
        distanceJoint = GetComponent<DistanceJoint2D>();
        distanceJoint.enabled = false;
    }

    void Update()
    {
        InputRegister();
    }

    private void FixedUpdate()
    {
        Move();
        rb.velocity = new Vector2(tempVelocity.x, rb.velocity.y);
    }

    private void Move() {

        //steps
        //make raycast circle throught the player
        //keep the distance joint anchor and not disable it, dont update the distance after that, just need the ray to know if it hitted
        tempVelocity.x = horizontalMovement * speed;

        
        if (playerHooked)
        {
            RaycastHit2D hit;
            if (!hittedHook) {
                directionHooked = new Vector2(horizontalMovement, verticalMovement);
                hit = Physics2D.Raycast(transform.position, directionHooked, 10, ~playerLayer);
                hittedHook = hit;
                if(hittedHook) hitPoint = hit ? hit.point : Vector2.zero;
            }
            else {
                

                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, hitPoint);
                lineRenderer.enabled = true;
                distanceJoint.enabled = true;
                distanceJoint.connectedAnchor = hitPoint;
            }

        }
        else {
            lineRenderer.enabled = false;
            distanceJoint.enabled = false;
            hittedHook = false;
        }
        Debug.DrawLine(transform.position, transform.position + (Vector3)new Vector2(horizontalMovement, verticalMovement) * 10, Color.red, ~playerLayer);
        Debug.Log(hitPoint);

    }
    private void InputRegister() {
        horizontalMovement = Input.GetAxis("Horizontal");
        verticalMovement = Input.GetAxis("Vertical");
        playerHooked = Input.GetKey(KeyCode.Space);

    }
}
