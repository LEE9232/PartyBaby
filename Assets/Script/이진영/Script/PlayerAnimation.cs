using System.Collections;
using System.Collections.Generic;
using UnityEditor.Purchasing;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    #region 변수
    private Animator anim;
    private ObjectGrabber grabber;
    private bool isRecevering = false;
    private bool isStuning = false;
    [SerializeField]
    public bool isGrabing = false;
    public bool isGrabPlay = false;
    #endregion
    void Start()
    {
        anim = GetComponent<Animator>();
        grabber = GetComponent<ObjectGrabber>(); // grabber 초기화
    }
    private void Update()
    {
        if (grabber != null)
        {
            bool currentGrabPlay = grabber.GetPlayerWeaponIsGrabbing();
            if (isGrabPlay != currentGrabPlay)
            {
                isGrabPlay = currentGrabPlay;
                anim.SetBool("GrabPlayer", isGrabPlay);
            }         
            isGrabing = grabber.GetWeaponIsGrabbing();
        }
        if (isRecevering)
        {
            anim.SetBool("StanUP", true);
            isRecevering = false;
        }      
    }
    public void SetRunningSpeed(float speed)
    {
        anim.SetFloat("RunningSpeed", speed);
    }
    // 공격
    public void Attack()
    {
        anim.SetBool("Attack", true);
        StartCoroutine(StopAttack());
    }
    //공격 중지
    private IEnumerator StopAttack()
    {
        yield return new WaitForSeconds(0.2f);
        anim.SetBool("Attack", false);
    }
    //점프 
    public void Jumping()
    {
        anim.SetBool("Jump", true);
        StartCoroutine(StopJumping());
    }
    //점프 중지
    private IEnumerator StopJumping()
    {
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("Jump", false);
    }
    // 점프 킥
    public void JumpKick()
    {
        anim.SetBool("JumpAction", true);
        StartCoroutine(StopJumpKick());
    }
    // 점프킥 중지
    private IEnumerator StopJumpKick()
    {
        yield return new WaitForSeconds(1.0f);
        anim.SetBool("JumpAction", false);
    }
    // 기절
    public void Stunned()
    {
        anim.SetBool("Stun", true);
        StartCoroutine(StopStunned());
        PlayReverse("StanUp");
    }
    // 기절 중지
    private IEnumerator StopStunned()
    {
        yield return new WaitForSeconds(0.1f);
        anim.SetBool("Stun", false);
        PlayReverse("StanUP");
    }
    // 공격중 여부
    public bool IsAttacking()
    {
        return anim.GetBool("Attack");
    }
    // 점프킥 여부
    public bool IsJumpAttacking()
    {
        return anim.GetBool("JumpAction");
    }

    public void PlayReverse(string StanUP)
    {     
        anim.Play(StanUP, 0, 1f);
        isRecevering = true;
        StartCoroutine(ResetAnimation());
    }

    private IEnumerator ResetAnimation()
    {
        yield return new WaitForSeconds(0.1f);   
        isRecevering = false;
        anim.SetBool("StanUP", false);
    }
    // 무기나 플레이어를 잡았을때 대기상태
    public void Grabing(bool Grabbing)
    {   
        isGrabing = Grabbing;
        anim.SetBool("Grabed", isGrabing);
    }
    public void GrabPlayer(bool Grabbing)
    { 
        isGrabPlay = Grabbing;
        anim.SetBool("GrabPlayer" , isGrabPlay);   
    }
    public void GrabRunning(float speed)
    {
        anim.SetFloat("GrabRunning", speed);
    }
    public void GrabAttack()
    {
        anim.SetBool("WeaponAttack", true);    
        StartCoroutine(StopGrabAttack());
    }
    private IEnumerator StopGrabAttack()
    {
        yield return new WaitForSeconds(0.2f);
        anim.SetBool("WeaponAttack", false);
    }  
    public bool IsWeaponAttacking()
    {
        return anim.GetBool("WeaponAttack");
    }
}