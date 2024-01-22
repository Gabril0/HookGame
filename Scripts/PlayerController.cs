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

    private float originalGravity;
    private float ropeGravity;

    private bool isOnGround = false;
    private CapsuleCollider2D capsuleCollider;

    private float originalAcceleration;
    private float speedBeforeHook;

    [SerializeField] private float maxRopeSize;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float acceleration;
    [SerializeField] private float maxSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        tempVelocity = rb.velocity;
        lineRenderer = GetComponent<LineRenderer>();
        distanceJoint = GetComponent<DistanceJoint2D>();
        distanceJoint.enabled = false;
        originalGravity = rb.gravityScale;
        ropeGravity = 10 * originalGravity;
        capsuleCollider = GetComponent<CapsuleCollider2D>();

        originalAcceleration = acceleration;
    }

    void Update()
    {
        InputRegister();
        CollisionCheck();
    }

    private void FixedUpdate()
    {
        Move();
        
        rb.velocity = hittedHook ? new Vector2(tempVelocity.x, tempVelocity.y) : new Vector2(tempVelocity.x, rb.velocity.y);
    }

    private void Move() {
        acceleration = isOnGround ? originalAcceleration / 2 : originalAcceleration;
        if (!hittedHook)
        {
            tempVelocity.x += Mathf.Abs(tempVelocity.x) < maxSpeed ? horizontalMovement * acceleration : 0;
            tempVelocity.x += Mathf.Abs(tempVelocity.x) > 0 ? -tempVelocity.x / 10 : 0;
            speedBeforeHook = tempVelocity.x;
        }
        else {
            tempVelocity.x += Mathf.Abs(tempVelocity.x) > 0 ? (speedBeforeHook * Mathf.Sign(tempVelocity.x))/ 100 : 0;
        }
        HookCheck();

    }

    private void HookCheck() {
        if (playerHooked)
        {
            RaycastHit2D hit;
            if (!hittedHook)
            {
                directionHooked = new Vector2(horizontalMovement, verticalMovement);
                hit = Physics2D.Raycast(transform.position, directionHooked, maxRopeSize, ~playerLayer);
                hittedHook = hit;
                if (hittedHook) hitPoint = hit ? hit.point : Vector2.zero;
            }
            else
            {
                rb.gravityScale = ropeGravity;
                tempVelocity.y = verticalMovement < 0 ? verticalMovement * acceleration : tempVelocity.y;


                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, hitPoint);
                lineRenderer.enabled = true;
                distanceJoint.connectedAnchor = hitPoint;
                distanceJoint.enabled = true;

                distanceJoint.distance = ((Mathf.Abs(verticalMovement) > 0 || Mathf.Abs(horizontalMovement) > 0) && distanceJoint.distance <= maxRopeSize) ? distanceJoint.distance + 1 : distanceJoint.distance;
                distanceJoint.distance = distanceJoint.distance < 3 ? 3 : distanceJoint.distance;


            }

        }
        else
        {
            rb.gravityScale = originalGravity;
            lineRenderer.enabled = false;
            distanceJoint.enabled = false;
            hittedHook = false;
        }
        Debug.DrawLine(transform.position, transform.position + (Vector3)new Vector2(horizontalMovement, verticalMovement) * maxRopeSize, Color.red, ~playerLayer);
        Debug.Log(tempVelocity);
    }

    private void CollisionCheck() {
        isOnGround = Physics2D.CapsuleCast(capsuleCollider.bounds.center, capsuleCollider.bounds.size - new Vector3(0.2f, 0f, 0f)
            , capsuleCollider.direction, 0,Vector2.down, 0.05f, ~playerLayer);
    }
    private void InputRegister() {
        horizontalMovement = Input.GetAxis("Horizontal");
        verticalMovement = Input.GetAxis("Vertical");
        playerHooked = Input.GetKey(KeyCode.Space);

    }
}
