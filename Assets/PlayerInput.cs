using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TouchEventType
{
    Move = 1,
    Camera = 2,
}

public class TouchEvent
{
    TouchEventType type;
    TouchPhase touchEvent;
    public Vector2 start;
    public Vector2 delta;
}

public class PlayerInput : MonoBehaviour
{
    public float MoveSpeed = 10.0f;
    public float TurnSpeed = 2.0f;
    public float JumpSpeed = 5.0f;
    public GameObject Projectile;

    Animator animator;
    GameObject marker;
    GameObject[] targets;
    int targetIndex = 0;
    Rigidbody rb;
    bool isGrounded;
    Vector3 positionInput;
    Quaternion rotationInput;
    new Collider collider;
    Touch moveTouch;
    Touch cameraTouch;

    public GameObject Target
    {
        get
        {
            return targets[targetIndex];
        }
    }

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        marker = GameObject.Find("Marker");
        targets = GameObject.FindGameObjectsWithTag("Target");
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        moveTouch.phase = TouchPhase.Ended;
        moveTouch.fingerId = -1;
        cameraTouch.phase = TouchPhase.Ended;
        cameraTouch.fingerId = -1;
    }

    void Update()
    {
        UpdateGrounded();
        UpdateInput();
        UpdateMovement();
        UpdateTouches();
    }

    void UpdateGrounded()
    {
        isGrounded = Physics.Raycast(transform.position, -Vector3.up, collider.bounds.extents.y + 0.1f);
    }

    void UpdateInput()
    {
        /* if (Input.GetButtonDown("Fire1"))
        {
            Fire();
        } */
        if (Input.GetButtonDown("Fire2"))
        {
            NextTarget();
        }
    }

    void UpdateTouches()
    {
        foreach (var touch in Input.touches)
        {
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (touch.position.x < Screen.width * 0.4)
                    {
                        Debug.Log($"Start {touch.fingerId} {touch.position.x} < {Screen.width * 0.4}");
                        moveTouch = touch;
                    }
                    if (touch.position.x > Screen.width * 0.6)
                    {
                        Debug.Log($"Start {touch.fingerId} {touch.position.x} > {Screen.width * 0.6}");
                        cameraTouch = touch;
                    }
                    break;
                case TouchPhase.Moved:
                    if (moveTouch.fingerId == touch.fingerId)
                    {
                        moveTouch = touch;
                    }
                    if (cameraTouch.fingerId == touch.fingerId)
                    {
                        cameraTouch = touch;
                    }
                    break;
                case TouchPhase.Ended:
                    Debug.Log($"End {touch.fingerId}");
                    if (moveTouch.fingerId == touch.fingerId)
                    {
                        moveTouch = touch;
                        moveTouch.fingerId = -1;
                    }
                    if (cameraTouch.fingerId == touch.fingerId)
                    {
                        cameraTouch = touch;
                        cameraTouch.fingerId = -1;
                    }
                    break;
            }
        }
        if (moveTouch.phase == TouchPhase.Moved)
        {
            //Debug.Log($"Move {cameraTouch.fingerId} {moveTouch.deltaPosition}");
            positionInput = new Vector3(moveTouch.deltaPosition.y * -1.0f, 0, moveTouch.deltaPosition.x);
        }
        if (cameraTouch.phase == TouchPhase.Moved)
        {
            //Debug.Log($"Cam {cameraTouch.fingerId} {cameraTouch.deltaPosition}");
            rotationInput = Quaternion.LookRotation(transform.rotation * new Vector3(cameraTouch.deltaPosition.x, 0, 0), Vector3.up);
            transform.forward = Vector3.Slerp(transform.forward, (rotationInput * Vector3.forward) + transform.forward, Time.deltaTime * TurnSpeed);
        }
    }

    void FixedUpdate()
    {
        //rb.MoveRotation(???);
        rb.MovePosition(rb.position + positionInput * MoveSpeed * Time.fixedDeltaTime);
    }
    void UpdateMovement()
    {
        var rotation = Input.GetAxis("Horizontal"); // TODO: Strafe
        var forward = Input.GetAxis("Vertical");
        var up = Input.GetButtonDown("Jump") && isGrounded ? JumpSpeed : 0.0f;
        if (rotation != 0.0f)
        {
            rotationInput = Quaternion.LookRotation(transform.rotation * new Vector3(rotation, 0, 0), Vector3.up);
            transform.forward = Vector3.Slerp(transform.forward, (rotationInput * Vector3.forward) + transform.forward, Time.deltaTime * TurnSpeed);
        }
        else
        {
            rotationInput = Quaternion.identity;
        }
        if (forward != 0.0f)
        {
            positionInput = transform.forward * forward;
        }
        else
        {
            positionInput = Vector3.zero;
        }
        if (up != 0.0f)
        {
            rb.AddForce(Vector3.up * Mathf.Sqrt(up * -2f * Physics.gravity.y), ForceMode.VelocityChange);
        }
        if (animator)
        {
            animator.SetFloat("Speed", forward);
        }
    }

    void Fire()
    {
        var obj = Instantiate(Projectile, transform.position, transform.rotation);
        obj.GetComponent<Projectile>().Target = Target;
    }

    void NextTarget()
    {
        if (targetIndex + 1 < targets.Length)
        {
            targetIndex++;
        }
        else
        {
            targetIndex = 0;
        }
        var target = Target.transform;
        marker.transform.position = target.position + target.up * 2.0f;
    }
}
