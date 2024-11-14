using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.InputSystem;

public class RagdollController : MonoBehaviour
{
    #region 변수
    private Animator animator;
    private Rigidbody[] ragdollBodies;
    private Collider[] ragdollColliders;
    private Rigidbody rb;
    private Vector3 originalGravity;
    private Vector3 playerVelocity;
    private CameraController cameraController;
    public Transform hipsObject; // hips 오브젝트를 인스펙터에서 설정
    private CapsuleCollider collider;
    #endregion

    private void Start()
    {
        animator = GetComponent<Animator>();
        ragdollBodies = GetComponentsInChildren<Rigidbody>();
        ragdollColliders = GetComponentsInChildren<Collider>();
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();
        cameraController = Camera.main.GetComponent<CameraController>();
        originalGravity = Physics.gravity;
        // Hips Transform 찾기
        foreach (var body in ragdollBodies)
        {
            if (body.name == "Hips") // Hips 오브젝트의 이름을 사용
            {
                hipsObject = body.transform;
                break;
            }
        }
        EnableRagdoll(false);
    }
    public void EnableRagdoll(bool enable)
    {
        animator.enabled = !enable;
        collider.enabled = !enable;
        //rb.isKinematic = !enable;


        Physics.gravity = enable ? new Vector3(0, -8, 0) : originalGravity;
        foreach (var body in ragdollBodies)
        {
            if (body != rb)
            {
                body.isKinematic = !enable;
                if (enable)
                {
                    body.velocity = Vector3.zero;
                    body.angularVelocity = Vector3.zero;
                }
            }
        }
        foreach (var collider in ragdollColliders)
        {
            if (collider.gameObject != this.gameObject)
            {
                collider.enabled = enable;
            }
        }
        foreach (var ragdollCollider in ragdollColliders)
        {
            if (ragdollCollider.gameObject != this.gameObject)
            {
                ragdollCollider.enabled = enable;
            }
        }
        if (cameraController != null)
        {
            cameraController.SetRagdollState(enable);
        }
        if (enable)
        {
            // 레그돌 활성화 시 캐릭터의 트랜스폼 위치를 힙의 위치로 설정
            transform.position = hipsObject.position;
        }
        else
        //if (!enable)
        {
            // 레그돌 비활성화 시 플레이어의 위치를 마지막 레그돌 위치로 설정
           Vector3 finalPosition = GetRagdollFinalPosition();
           transform.position = finalPosition;
        }
    }
    private Vector3 GetRagdollFinalPosition()
    {
        // 레그돌의 모든 Rigidbody 중 하나의 위치를 반환 (예: 첫 번째 Rigidbody)
        foreach (var body in ragdollBodies)
        {
            if (body.gameObject != this.gameObject)
            {
                return body.position;
            }
        }
        return transform.position;
    }
}
