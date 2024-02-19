using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.XR;

public class PlayerController : MonoBehaviour
{
    public float horizontalMovement;
    private float verticalMovement;
    public Vector2 tempVelocity;

    public bool canControl;
    public bool isAlive = true;
    private bool isFliped = false;

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
    private float stopTime;
    private float stopCooldown = 1;

    public bool isOnGround = false;
    private CapsuleCollider2D capsuleCollider;

    private float originalAcceleration;
    private float speedOnHook;




    [SerializeField] public float maxRopeSize;
    [SerializeField] public LayerMask playerLayer;
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

        stopTime = 0;

        eyeThrowAnimation.enabled = false;

        originalAcceleration = acceleration;

        canControl = true;
    }

    void Update()
    {
        if (isAlive)
        {
            if (canControl) { InputRegister(); }
            CollisionCheck();

        }
        else {
            horizontalMovement = 0;
            playerHooked = false;
            tempVelocity = Vector3.zero;
        }

    }


    private void FixedUpdate()
    {
        if (isAlive) {
            if (canControl) Move();
            SideCheck();
            HookCheck();
            RollCheck();
        }


        rb.velocity = hittedHook ? new Vector2(tempVelocity.x, tempVelocity.y) : new Vector2(tempVelocity.x, rb.velocity.y);
    }

    private void SideCheck()
    {
        if (tempVelocity.x < 0 && !isFliped)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            isFliped = true;
        }
        else if (tempVelocity.x > 0 && isFliped)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
            isFliped = false;
        }
    }

    private void Move() {
        acceleration = isOnGround ? originalAcceleration / 2 : originalAcceleration;
        if (isOnGround) { tempVelocity.y = 0; }
        if (tempVelocity.x > 0 && hitWallRight && (Time.time - lastTimeTouchedGround > 0.5f)) {
            tempVelocity.x = 0;
        }
        if (tempVelocity.x < 0 && hitWallLeft && (Time.time - lastTimeTouchedGround > 0.5f))
        {
            tempVelocity.x = 0;
        }
        if (isOnGround && !playerHooked && !isRolling)
        {
            tempVelocity.x += Mathf.Abs(tempVelocity.x) < maxSpeed ? horizontalMovement * acceleration : 0;
            tempVelocity.x += Mathf.Abs(tempVelocity.x) > 0 ? -tempVelocity.x / 10 : 0;


        }
        else if (hittedHook) {

            speedOnHook += Mathf.Abs(speedOnHook) > 0 ? -Mathf.Sign(speedOnHook) * accelerationOnRope / 100 : speedOnHook;
            speedOnHook += Mathf.Abs(speedOnHook) < maxSpeedOnRope ? horizontalMovement * accelerationOnRope / 50 : 0;
            if (Input.GetAxisRaw("Horizontal") == 0)
            {
                if (stopTime <= stopCooldown)
                {
                    stopTime += Time.deltaTime;
                }
                else
                {
                    speedOnHook = Mathf.Abs(speedOnHook) > 0 ? speedOnHook / 25 : 0;
                    if (Mathf.Abs(speedOnHook) > 0 && Mathf.Abs(speedOnHook) < 1) speedOnHook = 0;
                }

            }
            else {
                stopTime = 0;
            }
            if (Mathf.Sign(speedOnHook) != Input.GetAxisRaw("Horizontal") && horizontalMovement != 0) {
                speedOnHook *= -1;
                speedOnHook += horizontalMovement * accelerationOnRope / 150;
            }
            speedOnHook += Mathf.Abs(speedOnHook) >= 0 && Mathf.Abs(speedOnHook) <= 1 ? horizontalMovement * accelerationOnRope : 0;
            tempVelocity.x = speedOnHook;
        }


    }

    public float getMagnitude() {
        return rb.velocity.magnitude;
    }

    private void HookCheck() {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 direction = (mousePosition - playerPosition).normalized;
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

            if (!hittedHook)
            {
                directionHooked = (mousePosition - playerPosition).normalized; ;
                hit = Physics2D.Raycast(transform.position, directionHooked, maxRopeSize, ~playerLayer);
                speedOnHook = tempVelocity.x;
                Invoke("HookActivation", eyeThrowAnimation.GetCurrentAnimatorStateInfo(0).length);

            }
            else

            {
                if (!isOnGround) rb.gravityScale = ropeGravity * 2;
                //tempVelocity.y = verticalMovement < 0 ? verticalMovement * acceleration : tempVelocity.y;




                if (hit.rigidbody == null) {
                    distanceJoint.connectedBody = null;
                    distanceJoint.connectedAnchor = hitPoint;
                    lineRenderer.SetPosition(0, transform.position);
                    lineRenderer.SetPosition(1, hitPoint);
                    lineRenderer.enabled = true;

                }
                else {
                    distanceJoint.connectedAnchor = hit.collider.bounds.center;
                    lineRenderer.SetPosition(0, transform.position);
                    lineRenderer.SetPosition(1, hit.collider.bounds.center);
                    lineRenderer.enabled = true;
                    eyeInstance.transform.position = hit.collider.bounds.center + new Vector3(0, 0, -5);
                }
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
                if (hit.rigidbody == null) {
                    eyeInstance = Instantiate(eyeEnd, (Vector3)hitPoint + new Vector3(0, 0, -5), Quaternion.identity);
                    canInstanceEye = false;
                }
                if (hit.rigidbody != null)
                {
                    eyeInstance = Instantiate(eyeEnd, hit.collider.bounds.center + new Vector3(0, 0, -5), Quaternion.identity);
                    canInstanceEye = false;
                }

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
            , capsuleCollider.direction, 0, Vector2.down, 0.1f, ~playerLayer);
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
        if (!isOnGround)
        {
            isRolling = true;
        }
        if (playerHooked) isRolling = true;

    }

}
