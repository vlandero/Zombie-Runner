using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed;
    private float groundDrag;
    private float jumpForce;
    private float forceOnSlope;
    private float jumpCooldown;
    private float airMultiplier;
    private float maxSlopeAngle;

    bool readyToJump;
    private RaycastHit slopeHit;

    private KeyCode jumpKey = KeyCode.Space;

    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform orientation;

    bool grounded;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    bool exitingSlope;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        moveSpeed = PlayerManager.instance.moveSpeed;
        groundDrag = PlayerManager.instance.groundDrag;
        jumpForce = PlayerManager.instance.jumpForce;
        forceOnSlope = PlayerManager.instance.forceOnSlope;
        jumpCooldown = PlayerManager.instance.jumpCooldown;
        airMultiplier = PlayerManager.instance.airMultiplier;
        maxSlopeAngle = PlayerManager.instance.maxSlopeAngle;
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f * transform.localScale.y + 0.1f, groundMask);
        PlayerInput();
        SpeedControl();
        if(grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0f;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void PlayerInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");


        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(10f * moveSpeed * GetSlopeMovementDirection(), ForceMode.Force);

            if(rb.velocity.y > 0f)
            {
                rb.AddForce(Vector3.down * forceOnSlope, ForceMode.Force);
            }
        }
        rb.useGravity = !OnSlope();
        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        if(OnSlope() && !exitingSlope)
        {
            if(rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            if(flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void Jump()
    {
        exitingSlope = true;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
        exitingSlope = false;
    }

    private void StopMovement()
    {
        rb.velocity = Vector3.zero;
    }

    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f * transform.localScale.y + 0.2f))
        {
            float angle = Vector3.Angle(slopeHit.normal, Vector3.up);
            return angle < maxSlopeAngle && angle != 0f;
        }
        return false;
    }

    private Vector3 GetSlopeMovementDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }


}
