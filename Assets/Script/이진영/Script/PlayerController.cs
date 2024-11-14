using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
// 상태 종류
public enum PlayerState
{
    Idle,                    // 대기 상태
    UnarmedAttack,           // 비무장 공격 상태
    Running,                 // 달리는 상태
    OneHandedWeaponAttack,   // 한손 무기 공격 상태
    HandedGrabIdle,          // 잡은 상태의 대기상태
    HandedGrabRunning,       // 잡은 상태의 달리기
    Jump,                    // 점프 상태
    JumpAttack,              // 점프 공격 상태
    Stunned                  // 기절 상태  
}
public class PlayerController : MonoBehaviour
{
    #region 변수
    public float moveSpeed = 5.0f;
    public float rotationSpeed = 5.0f;
    public float jumpForce = 10f;
    public float gravity = 9.81f;
    private float stunDuration = 2.0f;
    private float stunend = 3.0f;
    private Camera mainCamera;
    private Rigidbody rb;
    private PlayerAnimation playerAnim;
    private RagdollController ragdollController;

    [SerializeField]
    private ObjectGrabber grabber;

    private Vector3 playerVelocity;
    private Vector3 jumpKickDirection;
    private bool isGround;
    private bool isJumping = false;
    private bool isJumpkicking = false;
    public bool CountDown = false;
    // 스턴 관련 변수
    public bool LongStun { get; set; }
    public bool isStuning { get; set; }

    [SerializeField]
    public bool isGrabPlayer = false;
    public bool isGrabing = false;
    // 현재 상태
    public PlayerState currentState { get; private set; }
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction attackAction;

    public ChangeSceneScript changScene;
    //점프 후 점프 공격을 위한 아이들전환 대기조건
    private bool isLandingDelayed = false;
    private bool hasLanded = false; // 착지 여부를 추적하는 플래그
    private float landingDelay = 0.2f; // 착지 후 상태 전환을 지연할 시간

    public bool isActive = true; // 게임종료때 사용
    private PlayerController[] allPlayers; // 모든 플레이어를 담는 배열

    [SerializeField]
    private GameCountDown gameCountDown;

    #endregion

    #region 플레이어 사운드 - 김용수
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip attackSound;
    [SerializeField]
    private AudioClip hitSound;
   
    #endregion
    #region 파티클 - 김지영
    public ParticleSystem particleDust; // 파티클
    public ParticleSystem particleStun;
    #endregion

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        attackAction = playerInput.actions["Attack"];
        particleDust.Stop();
        particleStun.Stop();

    }
    private void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<PlayerAnimation>();
        ragdollController = GetComponent<RagdollController>();
        rb.freezeRotation = true;
        allPlayers = FindObjectsOfType<PlayerController>();
    }

    private void Update()
    {
        // 자신의 상태가 비활성화되었을 때는 체크하지 않음
        if (!isActive) return;
        // 활성화된 플레이어만 필터링
        PlayerController[] activePlayers = allPlayers.Where(p => p.isActive).ToArray();
        if (activePlayers.Length == 1 && activePlayers[0] == this)
        {
            // 자신이 마지막으로 남은 플레이어일 때 승리 처리
            //Debug.Log($"The winner is Player with tag {gameObject.tag}");
            // 게임 종료 처리 코드 작성
            gameCountDown.WinnerText();
            //changScene.mainSceneChage();

        }
        isGround = Physics.Raycast(transform.position, Vector3.down, 1.0f);
        isJumping = !isGround;
        if (grabber != null)
        {
            isGrabing = grabber.GetWeaponIsGrabbing();
            isGrabPlayer = grabber.GetPlayerWeaponIsGrabbing();
        }
        if (!isJumpkicking && !isStuning)
        {
            HandleMovement();
            HandleStateTransitions();
        }

    }
    private void HandleMovement()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);

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
        if (isGrabPlayer)
        {
            rb.velocity = new Vector3(move.x * moveSpeed, rb.velocity.y, move.z * moveSpeed);
            float moveGrabSpeedMagnitude = move.magnitude * moveSpeed;
            playerAnim.GrabRunning(moveGrabSpeedMagnitude);
        }
        if (!isGrabPlayer)
        {
            rb.velocity = new Vector3(move.x * moveSpeed, rb.velocity.y, move.z * moveSpeed);
            float moveSpeedMagnitude = move.magnitude * moveSpeed;
            playerAnim.SetRunningSpeed(moveSpeedMagnitude);
        }
    }
    public void HandleStateTransitions()
    {
        bool jumpButton = jumpAction.triggered;
        bool attackButton = attackAction.triggered;
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        switch (currentState)
        {
            case PlayerState.Idle:
                if (jumpButton && isGround && !isGrabPlayer && !isGrabing)
                {
                    ChangeState(PlayerState.Jump);
                }
                else if (attackButton && isGround && !isGrabPlayer && !isGrabing)
                {
                    ChangeState(PlayerState.UnarmedAttack);
                }
                else if (moveInput != Vector2.zero)
                {
                    ChangeState(PlayerState.Running);
                }
                else if (isGround && isGrabing)
                {
                    ChangeState(PlayerState.HandedGrabIdle);
                }
                else if (isGrabPlayer && moveInput != Vector2.zero)
                {
                    ChangeState(PlayerState.HandedGrabRunning);
                }
                break;

            case PlayerState.Running:
                if (jumpButton && isGround)
                {
                    ChangeState(PlayerState.Jump);
                }
                else if (attackButton)
                {
                    if (isGrabing)
                    {
                        ChangeState(PlayerState.OneHandedWeaponAttack);
                    }
                    else if (isGround)
                    {
                        ChangeState(PlayerState.UnarmedAttack);
                    }
                }
                else if (isGrabing || isGrabPlayer)
                {
                    if (moveInput == Vector2.zero)
                    {
                        ChangeState(PlayerState.HandedGrabIdle);
                    }
                }
                break;
            case PlayerState.OneHandedWeaponAttack:
                if (!playerAnim.IsWeaponAttacking() || !isGrabPlayer)
                {
                    
                    StartCoroutine(AttackDelay());
                }
                break;
            case PlayerState.HandedGrabIdle:
                if (!isGrabing)
                {
                    ChangeState(PlayerState.Idle);
                }
                else if (moveInput != Vector2.zero && isGrabPlayer)
                {
                    ChangeState(PlayerState.HandedGrabRunning);
                }

                // 수정 체크할곳
                else if (isGrabPlayer) 
                {
                    ChangeState(PlayerState.HandedGrabIdle);
                }
                if (jumpButton && isGround && !isGrabPlayer)
                {
                    ChangeState(PlayerState.Jump);
                }
                if (attackButton && !isGrabPlayer)
                {
                    
                    ChangeState(PlayerState.OneHandedWeaponAttack);
                }
                if (moveInput != Vector2.zero && !isGrabPlayer)
                {
                    ChangeState(PlayerState.Running);
                }
                break;
            case PlayerState.HandedGrabRunning:
                if (moveInput == Vector2.zero && !isGrabPlayer)
                {
                    ChangeState(PlayerState.Idle);
                }
                if (moveInput == Vector2.zero && isGrabPlayer)
                {
                    ChangeState(PlayerState.HandedGrabIdle);
                }
                break;
            case PlayerState.Jump:
                if (attackButton && isJumping)
                {
                    if (isGrabing && !isGrabPlayer)
                    {
                        //PlayAttackSound();
                        ChangeState(PlayerState.OneHandedWeaponAttack);
                    }
                    else if (isGrabPlayer)
                    {
                        ChangeState(PlayerState.HandedGrabIdle);
                    }
                    else if (!playerAnim.IsJumpAttacking())
                    {
                        ChangeState(PlayerState.JumpAttack);
                        playerAnim.JumpKick();
                    }
                }
                if (isGround)
                {
                    if (!hasLanded)
                    {
                        hasLanded = true;
                        StartCoroutine(DelayedLanding());
                    
                    }
                }
                break;
            case PlayerState.UnarmedAttack:
                if (!playerAnim.IsAttacking() && !isGrabPlayer)
                {
                    ChangeState(PlayerState.Idle);
                }
                break;
            case PlayerState.JumpAttack:
                if (isGround)
                {
                    if (!hasLanded)
                    {
                        hasLanded = true;
                        StartCoroutine(DelayedLanding());
                    }
                    playerAnim.JumpKick();
                    playerVelocity = Vector3.zero;
                    isJumpkicking = false;
                    isStuning = true;
                    ChangeState(PlayerState.Stunned);
                }
                else
                {
                    jumpKickDirection = transform.forward;
                    isJumpkicking = true;
                    rb.velocity = jumpKickDirection * moveSpeed;
                    rb.AddForce(transform.forward * moveSpeed * 150.0f, ForceMode.Impulse);
                }
                break;

            case PlayerState.Stunned:
                break;
        }
    }
    private IEnumerator DelayedLanding()
    {
        isLandingDelayed = true;
        yield return new WaitForSeconds(landingDelay);
        if (isGround)
        {
            if (isGrabing)
            {
                ChangeState(PlayerState.HandedGrabIdle);
            }
            else
            {
                ChangeState(PlayerState.Idle);
            }
        }
        isLandingDelayed = false;
        hasLanded = false; // 착지 상태 플래그를 초기화
    }
    private IEnumerator AttackDelay()
    {
        
        yield return new WaitForSeconds(0.4f);
        ChangeState(PlayerState.HandedGrabIdle);
    }
    public void ChangeState(PlayerState newState)
    {
        if (currentState == newState)
            return;
 
       //Debug.Log($"상태 {currentState} to {newState}");
       // Debug.Log($"점프 {isJumping} to {newState}");
        OnStateExit(currentState);
        currentState = newState;
        OnStateEnter(currentState);
    }
    public void OnStateEnter(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.Idle:
                particleDust.Stop();
                particleStun.Stop();
                playerAnim.Grabing(isGrabing);
                break;
            case PlayerState.UnarmedAttack:
                playerAnim.Attack();
                break;
            case PlayerState.Running:
                particleDust.Play();
                break;

            case PlayerState.OneHandedWeaponAttack:
                
                playerAnim.GrabAttack();

                break;
            case PlayerState.HandedGrabIdle:
                particleDust.Stop();
                particleStun.Stop();
                if (isGrabPlayer)
                {
                    playerAnim.GrabPlayer(isGrabPlayer);
                }
                if (isGrabing)
                {
                    playerAnim.Grabing(isGrabing);
                }
                isJumping = !isGrabPlayer;
                break;
            case PlayerState.HandedGrabRunning:
                particleDust.Play();
                isGrabing = grabber.GetWeaponIsGrabbing();
                isGrabPlayer = grabber.GetPlayerWeaponIsGrabbing();
                break;
            case PlayerState.Jump:
                particleDust.Stop();
                if (!isJumping)
                {
                    isJumping = true;
                    playerAnim.Jumping();
                    playerVelocity.y = Mathf.Sqrt(jumpForce * 500.0f * gravity);
                    rb.AddForce(Vector3.up * playerVelocity.y, ForceMode.Impulse);
                }
                break;
            case PlayerState.JumpAttack:
                playerAnim.JumpKick();
                break;

            case PlayerState.Stunned:
                PlayHitSound(); // 효과음 재생
                particleStun.Play();
                ragdollController.EnableRagdoll(true);
                playerVelocity = Vector3.zero;
                StartCoroutine(StopMove());
                break;
        }
    }
    public void OnStateExit(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.Idle:               
                break;

            case PlayerState.UnarmedAttack:
                break;

            case PlayerState.Running:
                particleDust.Stop();
                break;

            case PlayerState.OneHandedWeaponAttack:
                PlayAttackSound();
                break;

            case PlayerState.HandedGrabIdle:
                break;

            case PlayerState.HandedGrabRunning:
                particleDust.Stop();
                break;

            case PlayerState.Jump:
                isJumping = false;
                break;

            case PlayerState.JumpAttack:           
                break;

            case PlayerState.Stunned:            
                ragdollController.EnableRagdoll(false);
                playerAnim.Stunned();
                break;
        }
    }
    IEnumerator StopMove()
    {
        if (!LongStun)
        {
            yield return new WaitForSeconds(stunDuration);
            isStuning = false;
            ChangeState(PlayerState.Idle);
            isJumpkicking = false;
        }
        else if (LongStun)
        {
            yield return new WaitForSeconds(6.0f);
            isStuning = false;
            ChangeState(PlayerState.Idle);
            LongStun = false;
            isJumpkicking = false;
        }
    }
    // 스턴 상태로 변경하는 메소드
    public void SetStunned(bool stunned)
    {
        LongStun = stunned;
        isStuning = stunned;
        if (stunned)
        {
            if (grabber.GetPlayerWeaponIsGrabbing() && grabber.GetWeaponIsGrabbing())
            {
                grabber.AllRelease(); // 잡고 있는 오브젝트 놓기
            }   
            ChangeState(PlayerState.Stunned);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (currentState == PlayerState.JumpAttack)
        {
            if (collision.collider.CompareTag("Ground"))
            {
                ChangeState(PlayerState.Stunned);
            }
            else if (collision.collider.CompareTag("Player"))
            {
                var otderPlayer = collision.collider.GetComponent<PlayerController>();
                if (otderPlayer != null)
                { 
                    otderPlayer.SetStunned(true);
                }
            }
        }
        if (collision.collider.CompareTag("Finish"))
        {
            ragdollController.EnableRagdoll(true);
            isStuning = true;
            isJumpkicking = true;
            rb.isKinematic = true;
            StartCoroutine(EndObjectActive());
        }
    }
    IEnumerator EndObjectActive()
    {
        yield return new WaitForSeconds(3.0f);
        Eliminate();
    }
    public void Eliminate()
    {
        isActive = false;
        gameObject.SetActive(false);
    }


    // 사운드 : 김용수
    private void PlayAttackSound()
    {
        if (audioSource && attackSound)
        {
            audioSource.PlayOneShot(attackSound);
        }
    }
    // 사운드 : 김용수
    private void PlayHitSound()
    {
        if (audioSource && hitSound)
        {
            audioSource.PlayOneShot(hitSound);
        }
    }

    public void Method()
    {
        throw new System.NotImplementedException();
    }
}
