using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.XR;

public class PlayerController : MonoBehaviour
{
    private float horizontalMovement;
    private float verticalMovement;
    public Vector2 tempVelocity;

    public bool canControl;

    public bool isRolling;

    private Rigidbody2D rb;

    private Vector2 hitPoint;
    private LineRenderer lineRenderer;
    private DistanceJoint2D distanceJoint;
    private Vector2 directionHooked;
    public bool playerHooked { get; set; }
    public bool hittedHook = false;

    private float originalGravity;
    private float ropeGravity;

    private bool isOnGround = false;
    private CapsuleCollider2D capsuleCollider;

    private float originalAcceleration;
    private float speedOnHook;

    [SerializeField] SpriteRenderer projection;
    private UnityEngine.Color projectionYellow = new UnityEngine.Color(255f / 255f, 255f / 255f, 13f / 255f, 1f);
    private UnityEngine.Color projectionGray = new UnityEngine.Color(200f / 255f, 200f / 255f, 200f / 255f, 0.5f);


    [SerializeField] private float maxRopeSize;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float acceleration;
    [SerializeField] private float accelerationOnRope;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float maxSpeedOnRope;

    private RaycastHit2D hit;

    [SerializeField] private Animator eyeThrowAnimation;
    private bool canPlayEyeThrowAnimation = true;


    [SerializeField] GameObject eyeHookSegment;
    [SerializeField] GameObject eyeEnd;
    private GameObject eyeInstance;
    private bool canInstanceEye = true;

    private bool hitWallRight;
    private bool hitWallLeft;
    private float lastTimeTouchedGround = 0;

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

        eyeThrowAnimation.enabled = false;

        originalAcceleration = acceleration;

        canControl = true;
    }

    void Update()
    {
        if (canControl) { InputRegister(); }
        CollisionCheck();
        Project();
    }

    private void FixedUpdate() // make the player shoot some rectangles an his eye at the end to simulate the rope when he doesnt hit anything
    {
        Move();
        HookCheck();
        RollCheck();

        rb.velocity = hittedHook ? new Vector2(tempVelocity.x, tempVelocity.y) : new Vector2(tempVelocity.x, rb.velocity.y);
    }

    private void Project()
    {
        Vector2 direction = new Vector2(horizontalMovement, verticalMovement).normalized;

        float distanceFromPlayer = 4f;
        projection.transform.position = transform.position + new Vector3(direction.x * distanceFromPlayer, direction.y * distanceFromPlayer, 0f);
        projection.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

        projection.color = Physics2D.Raycast(transform.position, direction, maxRopeSize, ~playerLayer) ? projectionYellow : projectionGray;
        projection.enabled = (horizontalMovement == 0 && verticalMovement == 0) ? false : true;
    }


    private void Move() {
        acceleration = isOnGround ? originalAcceleration / 2 : originalAcceleration;
        if (tempVelocity.x > 0 && hitWallRight && ( Time.time - lastTimeTouchedGround > 0.5f)) {
            tempVelocity.x = 0;
        }
        if (tempVelocity.x < 0 && hitWallLeft && ( Time.time - lastTimeTouchedGround > 0.5f))
        {
            tempVelocity.x = 0;
        }
        if (isOnGround && !playerHooked && !isRolling)
        {
            tempVelocity.x += Mathf.Abs(tempVelocity.x) < maxSpeed ? horizontalMovement * acceleration : 0;
            tempVelocity.x += Mathf.Abs(tempVelocity.x) > 0 ? -tempVelocity.x / 10 : 0;

            
        }
        else if (hittedHook) {
            speedOnHook += Mathf.Abs(speedOnHook) > 0? -Mathf.Sign(speedOnHook) * accelerationOnRope/100: speedOnHook;
            speedOnHook += Mathf.Abs(speedOnHook) < maxSpeedOnRope? horizontalMovement * accelerationOnRope / 50: 0;
            if (rb.velocity.magnitude < 0.75f)
            {
                speedOnHook = -speedOnHook;
            }
            speedOnHook += Mathf.Abs(speedOnHook) >= 0 && Mathf.Abs(speedOnHook) <= 1 ? horizontalMovement * accelerationOnRope : 0;
            tempVelocity.x = speedOnHook;
        }


    }

    private void HookCheck() {
        Vector2 direction = new Vector2(horizontalMovement, verticalMovement).normalized;
        eyeThrowAnimation.transform.parent = null; // making this because of a bug when rotating
        eyeThrowAnimation.transform.position = transform.position;
        eyeThrowAnimation.gameObject.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        if (canPlayEyeThrowAnimation && playerHooked) { 
            eyeThrowAnimation.enabled = true;
            eyeThrowAnimation.Play("EyeThrow");
            eyeThrowAnimation.speed = 1;
            canPlayEyeThrowAnimation = false;
            eyeThrowAnimation.GetComponent<SpriteRenderer>().enabled = true;
        }

            if (playerHooked)
            {
                
                if (!hittedHook )
                {
                    directionHooked = new Vector2(horizontalMovement, verticalMovement);
                    hit = Physics2D.Raycast(transform.position, directionHooked, maxRopeSize, ~playerLayer);
                    speedOnHook = tempVelocity.x;
                    Invoke("HookActivation",eyeThrowAnimation.GetCurrentAnimatorStateInfo(0).length);
                    
                }
                else

                {
                    if (!isOnGround) rb.gravityScale = ropeGravity * 2;
                    //tempVelocity.y = verticalMovement < 0 ? verticalMovement * acceleration : tempVelocity.y;


                    lineRenderer.SetPosition(0, transform.position);
                    lineRenderer.SetPosition(1, hitPoint);
                    lineRenderer.enabled = true;
                    distanceJoint.connectedAnchor = hitPoint;
                    distanceJoint.enabled = true;


                    distanceJoint.distance = distanceJoint.distance < maxRopeSize / 2 ? distanceJoint.distance + 1 : distanceJoint.distance - 1;
                    distanceJoint.distance = distanceJoint.distance < maxRopeSize / 2 + 1 || distanceJoint.distance > maxRopeSize / 2 - 1 ? maxRopeSize / 2 : distanceJoint.distance;
                    distanceJoint.distance = distanceJoint.distance < 3 ? 3 : distanceJoint.distance;


                }
            }
            else
            {
                canPlayEyeThrowAnimation = true;
                eyeThrowAnimation.GetComponent<SpriteRenderer>().enabled = false;
                rb.gravityScale = originalGravity;
                lineRenderer.enabled = false;
                distanceJoint.enabled = false;
                hittedHook = false;
                
                if (eyeInstance != null)
                {
                    Destroy(eyeInstance);
                    canInstanceEye = true;
                }
            }
    }
    private void HookActivation() {
        hittedHook = hit;
        if (hittedHook)
        {
            eyeThrowAnimation.enabled = false;
            canPlayEyeThrowAnimation = false;
            eyeThrowAnimation.GetComponent<SpriteRenderer>().enabled = false;
            
            hitPoint = hit ? hit.point : Vector2.zero;
            if (canInstanceEye) { 
                eyeInstance = Instantiate(eyeEnd, (Vector3)hitPoint + new Vector3(0, 0, -5), Quaternion.identity);
                canInstanceEye = false;
            }
        }
    }

    private void RollCheck() {
        if (isRolling)
        {
            rb.freezeRotation = false;

        }
        else {
            rb.freezeRotation = true;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        
    }

    private void CollisionCheck() {
        isOnGround = Physics2D.CapsuleCast(capsuleCollider.bounds.center, capsuleCollider.bounds.size - new Vector3(0.2f, 0f, 0f)
            , capsuleCollider.direction, 0,Vector2.down, 0.1f, ~playerLayer);
        lastTimeTouchedGround = isOnGround ? Time.time : lastTimeTouchedGround;
        hitWallLeft = Physics2D.CapsuleCast(capsuleCollider.bounds.center, capsuleCollider.bounds.size - new Vector3(0, 0.2f, 0f)
            , capsuleCollider.direction, 0, Vector2.left, 0.1f, ~playerLayer);
        hitWallRight = Physics2D.CapsuleCast(capsuleCollider.bounds.center, capsuleCollider.bounds.size - new Vector3(0, 0.2f, 0f)
            , capsuleCollider.direction, 0, Vector2.right, 0.1f, ~playerLayer);

    }
    private void InputRegister() {
        horizontalMovement = Input.GetAxis("Horizontal");
        verticalMovement = Input.GetAxis("Vertical");
        playerHooked = Input.GetKey(KeyCode.Mouse0);
        isRolling = Input.GetKey(KeyCode.Space);
        if (playerHooked) isRolling = true;

    }
}
