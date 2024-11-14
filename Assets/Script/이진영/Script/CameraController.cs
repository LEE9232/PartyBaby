using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    #region 변수
    public Transform player; // 플레이어의 Transform
    public Transform hips; // Hips Transform
    public float mouseSensitivity = 100.0f; // 마우스 감도
    public float distanceFromPlayer = 8.0f; // 카메라와 플레이어 사이의 거리
    public float heightOffset = 0.0f; // 카메라의 높이 오프셋
    private float rotationX = 0.0f;
    private float rotationY = 0.0f;
    private bool RagdollActive = false; // 레그돌 상태 확인 변수
    private Vector3 velocity = Vector3.zero;
    public float smoothTime = 0.03f; // 카메라 이동의 부드러움 정도
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
        // 컨트롤러 오른쪽 스틱
        Vector2 lookDelta = lookAction.ReadValue<Vector2>();
        rotationX += lookDelta.x * mouseSensitivity * Time.deltaTime;
        rotationY -= lookDelta.y * mouseSensitivity * Time.deltaTime;
        rotationY = Mathf.Clamp(rotationY, -35, 60);
        // 마우스 
        //rotationX += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        //rotationY -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        //rotationY = Mathf.Clamp(rotationY, -35, 60); // Y축 회전 각도를 제한하여 카메라가 뒤집히지 않도록 함
        // 카메라의 회전
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
