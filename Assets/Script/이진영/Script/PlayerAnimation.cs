using System.Collections;
using System.Collections.Generic;
using UnityEditor.Purchasing;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    #region ����
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
        grabber = GetComponent<ObjectGrabber>(); // grabber �ʱ�ȭ
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
    // ����
    public void Attack()
    {
        anim.SetBool("Attack", true);
        StartCoroutine(StopAttack());
    }
    //���� ����
    private IEnumerator StopAttack()
    {
        yield return new WaitForSeconds(0.2f);
        anim.SetBool("Attack", false);
    }
    //���� 
    public void Jumping()
    {
        anim.SetBool("Jump", true);
        StartCoroutine(StopJumping());
    }
    //���� ����
    private IEnumerator StopJumping()
    {
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("Jump", false);
    }
    // ���� ű
    public void JumpKick()
    {
        anim.SetBool("JumpAction", true);
        StartCoroutine(StopJumpKick());
    }
    // ����ű ����
    private IEnumerator StopJumpKick()
    {
        yield return new WaitForSeconds(1.0f);
        anim.SetBool("JumpAction", false);
    }
    // ����
    public void Stunned()
    {
        anim.SetBool("Stun", true);
        StartCoroutine(StopStunned());
        PlayReverse("StanUp");
    }
    // ���� ����
    private IEnumerator StopStunned()
    {
        yield return new WaitForSeconds(0.1f);
        anim.SetBool("Stun", false);
        PlayReverse("StanUP");
    }
    // ������ ����
    public bool IsAttacking()
    {
        return anim.GetBool("Attack");
    }
    // ����ű ����
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
    // ���⳪ �÷��̾ ������� ������
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