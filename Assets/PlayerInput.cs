using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    void Update()
    {
        UpdateGrounded();
        UpdateInput();
        UpdateMovement();
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

    void FixedUpdate()
    {
        //rb.MoveRotation(???);
        rb.MovePosition(rb.position + positionInput * MoveSpeed * Time.fixedDeltaTime);
    }
    void UpdateMovement()
    {
        var rotation = Input.GetAxis("Horizontal");
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
