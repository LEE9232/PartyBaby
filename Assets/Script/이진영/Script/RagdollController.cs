using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.InputSystem;

public class RagdollController : MonoBehaviour
{
    #region ����
    private Animator animator;
    private Rigidbody[] ragdollBodies;
    private Collider[] ragdollColliders;
    private Rigidbody rb;
    private Vector3 originalGravity;
    private Vector3 playerVelocity;
    private CameraController cameraController;
    public Transform hipsObject; // hips ������Ʈ�� �ν����Ϳ��� ����
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
        // Hips Transform ã��
        foreach (var body in ragdollBodies)
        {
            if (body.name == "Hips") // Hips ������Ʈ�� �̸��� ���
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
            // ���׵� Ȱ��ȭ �� ĳ������ Ʈ������ ��ġ�� ���� ��ġ�� ����
            transform.position = hipsObject.position;
        }
        else
        //if (!enable)
        {
            // ���׵� ��Ȱ��ȭ �� �÷��̾��� ��ġ�� ������ ���׵� ��ġ�� ����
           Vector3 finalPosition = GetRagdollFinalPosition();
           transform.position = finalPosition;
        }
    }
    private Vector3 GetRagdollFinalPosition()
    {
        // ���׵��� ��� Rigidbody �� �ϳ��� ��ġ�� ��ȯ (��: ù ��° Rigidbody)
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
