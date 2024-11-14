using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    #region ����
    public Transform player; // �÷��̾��� Transform
    public Transform hips; // Hips Transform
    public float mouseSensitivity = 100.0f; // ���콺 ����
    public float distanceFromPlayer = 8.0f; // ī�޶�� �÷��̾� ������ �Ÿ�
    public float heightOffset = 0.0f; // ī�޶��� ���� ������
    private float rotationX = 0.0f;
    private float rotationY = 0.0f;
    private bool RagdollActive = false; // ���׵� ���� Ȯ�� ����
    private Vector3 velocity = Vector3.zero;
    public float smoothTime = 0.03f; // ī�޶� �̵��� �ε巯�� ����
    public PlayerInput playerInput;
    private InputAction lookAction;   
    #endregion
    private void Awake()
    {
        lookAction = playerInput.actions["MouseMove"];
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        // ��Ʈ�ѷ� ������ ��ƽ
        Vector2 lookDelta = lookAction.ReadValue<Vector2>();
        rotationX += lookDelta.x * mouseSensitivity * Time.deltaTime;
        rotationY -= lookDelta.y * mouseSensitivity * Time.deltaTime;
        rotationY = Mathf.Clamp(rotationY, -35, 60);
        // ���콺 
        //rotationX += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        //rotationY -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        //rotationY = Mathf.Clamp(rotationY, -35, 60); // Y�� ȸ�� ������ �����Ͽ� ī�޶� �������� �ʵ��� ��
        // ī�޶��� ȸ��
        transform.rotation = Quaternion.Euler(rotationY, rotationX, 0);
        Vector3 targetPosition;
        if (!RagdollActive)
        {
            targetPosition = player.position - transform.forward * distanceFromPlayer + Vector3.up * heightOffset;
        }
        else
        {
            targetPosition = hips.position - transform.forward * distanceFromPlayer + Vector3.up * heightOffset;
        } 
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
    public void SetRagdollState(bool isActive)
    {
        RagdollActive = isActive;
    }
}
