using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.XR;

public class PlayerController : MonoBehaviour
{
    private float horizontalMovement;
    private float verticalMovement;
    private Vector2 tempVelocity;


    private Rigidbody2D rb;

    private Vector2 hitPoint;
    private LineRenderer lineRenderer;
    private DistanceJoint2D distanceJoint;
    private Vector2 directionHooked;
    private bool playerHooked = false;
    private bool hittedHook = false;

    private float originalGravity;
    private float ropeGravity;

    private bool isOnGround = false;
    private CapsuleCollider2D capsuleCollider;

    private float originalAcceleration;
    private float speedOnHook;

    [SerializeField] LineRenderer projection;

    [SerializeField] private float maxRopeSize;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float acceleration;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float maxSpeedOnRope;

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
        projection.enabled = true;


        originalAcceleration = acceleration;
    }

    void Update()
    {
        InputRegister();
        CollisionCheck();
        Project();
    }

    private void FixedUpdate() // make the player shoot some rectangles an his eye at the end to simulate the rope when he doesnt hit anything
    {
        Move();
        Debug.Log(tempVelocity);

        rb.velocity = hittedHook ? new Vector2(tempVelocity.x, tempVelocity.y) : new Vector2(tempVelocity.x, rb.velocity.y);
    }

    private void Project() {

        Vector2 direction = new Vector2(horizontalMovement, verticalMovement).normalized;

        Vector3 secondPosition = transform.position + (Vector3)direction * maxRopeSize;

        projection.SetPosition(0, transform.position);
        projection.SetPosition(1, secondPosition);
    }

    private void Move() {
        acceleration = isOnGround ? originalAcceleration / 2 : originalAcceleration;

        if (isOnGround)
        {
            tempVelocity.x += Mathf.Abs(tempVelocity.x) < maxSpeed ? horizontalMovement * acceleration : 0;
            tempVelocity.x += Mathf.Abs(tempVelocity.x) > 0 ? -tempVelocity.x / 10 : 0;

            
        }
        else if (hittedHook) {
            speedOnHook += Mathf.Abs(speedOnHook) > 0? -Mathf.Sign(speedOnHook) * acceleration/100: speedOnHook;
            speedOnHook += Mathf.Abs(speedOnHook) < maxSpeedOnRope? horizontalMovement * acceleration / 50: 0;
            if (rb.velocity.magnitude < 0.5f)
            {
                speedOnHook = -speedOnHook;
            }
            speedOnHook += Mathf.Abs(speedOnHook) >= 0 && Mathf.Abs(speedOnHook) <= 1 ? horizontalMovement * acceleration : 0;
            tempVelocity.x = speedOnHook;
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
                speedOnHook = tempVelocity.x;
                hittedHook = hit;
                rb.freezeRotation = false;
                if (hittedHook) hitPoint = hit ? hit.point : Vector2.zero;
            }
            else
            {
                if (!isOnGround)rb.gravityScale = ropeGravity * 2;
                //tempVelocity.y = verticalMovement < 0 ? verticalMovement * acceleration : tempVelocity.y;


                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, hitPoint);
                lineRenderer.enabled = true;
                distanceJoint.connectedAnchor = hitPoint;
                distanceJoint.enabled = true;

                
                distanceJoint.distance =  distanceJoint.distance < maxRopeSize/2 ? distanceJoint.distance + 1 : distanceJoint.distance - 1;
                distanceJoint.distance = distanceJoint.distance < maxRopeSize/2 + 1 || distanceJoint.distance > maxRopeSize / 2 - 1  ? maxRopeSize/2: distanceJoint.distance;
                distanceJoint.distance = distanceJoint.distance < 3 ? 3 : distanceJoint.distance;


            }

        }
        else
        {
            rb.gravityScale = originalGravity;
            lineRenderer.enabled = false;
            distanceJoint.enabled = false;
            hittedHook = false;
            rb.freezeRotation = true;
            transform.rotation = Quaternion.Euler(0,0,0);
        }
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
