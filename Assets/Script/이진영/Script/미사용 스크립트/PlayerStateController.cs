using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public enum PlayerState
{
    Idle,
    UnarmedAttack,
    Running,
    OneHandedWeaponAttack,
    TwoHandedWeaponAttack,
    Jump,
    JumpAttack,
    Stunned
}

public class PlayerStateController : MonoBehaviour
{
    private float stunDuration = 1.0f;
    private float stunTimer;
    public bool isJumpkicking = false;
    public bool isStuning = false;

    private PlayerAnimation playerAnim;
    private PlayerState currentState = PlayerState.Idle;
    private RagdollController ragdollController;
    private PlayerMovementController playerMovementController;

    void Start()
    {
        playerAnim = GetComponent<PlayerAnimation>();
        ragdollController = GetComponent<RagdollController>();
        playerMovementController = GetComponent<PlayerMovementController>();
    }

    void Update()
    {
        HandleStateTransitions();
    }

    void HandleStateTransitions()
    {
        switch (currentState)
        {
            case PlayerState.Idle:
                if (Input.GetKeyDown(KeyCode.Space) && playerMovementController.isGround)
                {
                    ChangeState(PlayerState.Jump);
                }
                if (Input.GetMouseButtonDown(0) && playerMovementController.isGround)
                {
                    ChangeState(PlayerState.UnarmedAttack);
                }
                if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
                {
                    ChangeState(PlayerState.Running);
                }
                break;

            case PlayerState.Running:
                if (Input.GetKeyDown(KeyCode.Space) && playerMovementController.isGround)
                {
                    ChangeState(PlayerState.Jump);
                }
                if (Input.GetMouseButtonDown(0) && playerMovementController.isGround)
                {
                    ChangeState(PlayerState.UnarmedAttack);
                }
                if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
                {
                    ChangeState(PlayerState.Idle);
                }
                break;

            case PlayerState.Jump:
                if (playerMovementController.isGround)
                {
                    ChangeState(PlayerState.Idle);
                }
                if (Input.GetMouseButtonDown(0))
                {
                    ChangeState(PlayerState.JumpAttack);
                }
                break;

            case PlayerState.UnarmedAttack:
                if (!playerAnim.IsAttacking())
                {
                    ChangeState(PlayerState.Idle);
                }
                break;

            case PlayerState.JumpAttack:
                playerAnim.JumpKick();
                isJumpkicking = true;
                playerMovementController.PerformJumpKick(transform.forward);
                if (playerMovementController.isGround)
                {
                    isJumpkicking = false;
                    isStuning = true;
                    ChangeState(PlayerState.Stunned);
                }
                break;

            case PlayerState.Stunned:
                stunTimer -= Time.deltaTime;
                if (stunTimer <= 0)
                {
                    isStuning = false;
                    ChangeState(PlayerState.Idle);
                }
                break;
        }
    }

    public void ChangeState(PlayerState newState)
    {
        if (currentState == newState)
            return;

        OnStateExit(currentState);
        currentState = newState;
        OnStateEnter(currentState);
    }

    void OnStateEnter(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.Idle:
                Debug.Log("Entering Idle State");
                break;
            case PlayerState.UnarmedAttack:
                Debug.Log("Entering UnarmedAttack State");
                playerAnim.Attack();
                break;
            case PlayerState.Running:
                Debug.Log("Entering Running State");
                break;
            case PlayerState.Jump:
                Debug.Log("Entering Jump State");
                playerMovementController.ApplyJumpForce();
                playerAnim.Jumping();
                break;
            case PlayerState.JumpAttack:
                Debug.Log("Entering JumpAttack State");
                playerAnim.JumpKick();
                break;
            case PlayerState.Stunned:
                Debug.Log("Entering Stunned State");
                stunTimer = stunDuration;
                isJumpkicking = false;
                ragdollController.EnableRagdoll(true);
                break;
        }
    }

    void OnStateExit(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.Idle:
                Debug.Log("Exiting Idle State");
                break;
            case PlayerState.UnarmedAttack:
                Debug.Log("Exiting UnarmedAttack State");
                break;
            case PlayerState.Running:
                Debug.Log("Exiting Running State");
                break;
            case PlayerState.Jump:
                Debug.Log("Exiting Jump State");
                break;
            case PlayerState.JumpAttack:
                Debug.Log("Exiting JumpAttack State");
                break;
            case PlayerState.Stunned:
                Debug.Log("Exiting Stunned State");
                ragdollController.EnableRagdoll(false);
                break;
        }
    }

    public void HandleCollision(Collision collision)
    {
        if (currentState == PlayerState.JumpAttack && collision.collider.CompareTag("Ground"))
        {
            ChangeState(PlayerState.Stunned);
        }
    }
}
*/