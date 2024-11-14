using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class PlayerMovementController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float rotationSpeed = 5.0f;
    public float jumpForce = 10f;
    public float gravity = 9.81f;

    private Camera mainCamera;
    private Rigidbody rb;
    private Vector3 playerVelocity;
    //private bool isGround;
    public bool isGround { get; private set; } 
    // Public property with a private setter
    private Vector3 jumpKickDirection;

    private PlayerStateController playerStateController;

    void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
        playerStateController = GetComponent<PlayerStateController>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, 0.3f);
        if (!playerStateController.isJumpkicking && !playerStateController.isStuning)
        {
            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            Vector3 forward = mainCamera.transform.forward;
            Vector3 right = mainCamera.transform.right;
            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();
            Vector3 desiredMoveDirection = forward * move.z + right * move.x;
            move = desiredMoveDirection.normalized;
            if (move != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(move);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
            if (!isGround)
            {
                playerVelocity.y -= gravity * Time.deltaTime;
            }
            else
            {
                playerVelocity.y = 0;
            }
            rb.velocity = new Vector3(move.x * moveSpeed, rb.velocity.y, move.z * moveSpeed);
            if (playerStateController.isJumpkicking)
            {
                rb.AddForce(jumpKickDirection * moveSpeed * Time.deltaTime, ForceMode.VelocityChange);
            }
        }
    }

    public void ApplyJumpForce()
    {
        playerVelocity.y = Mathf.Sqrt(jumpForce * 10.0f * gravity);
        rb.AddForce(Vector3.up * playerVelocity.y, ForceMode.Impulse);
    }

    public void PerformJumpKick(Vector3 direction)
    {
        jumpKickDirection = direction;
        rb.velocity = jumpKickDirection * moveSpeed;
        rb.AddForce(direction * moveSpeed * 400.0f, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision collision)
    {
        playerStateController.HandleCollision(collision);
    }
}
*/