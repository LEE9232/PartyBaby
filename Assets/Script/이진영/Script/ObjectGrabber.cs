using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectGrabber : MonoBehaviour
{
    #region 변수
    public KeyCode weaponKey = KeyCode.F;
    public Transform holdPosition;
    public Transform WeaponPosition;
    private PlayerInput playerInput;
    private InputAction PlayerGrabAction;
    private InputAction WeaponGrab;
    // 플레이어 잡는 힘
    // 들어올릴 때 적용할 힘의 크기
    public float liftForce = 60f; // 들어올리는 힘
    public float grabRange = 0.5f; // 잡기 범위
    public float grabRadius = 0.8f; // 잡기 범위
    // 무기 잡는 힘 , 위치.
    public float WeaponForce = 10f; // 들어올리는 힘
    public float WeapongrabRange = 0.5f; // 잡기 범위
    public float WeapongrabRadius = 0.5f; // 잡기 범위
    // 현재 잡고 있는 오브젝트를 참조하는 변수
    private GameObject grabbedObject = null;
    private GameObject weaponObject = null;
    // 잡은 오브젝트의 Rigidbody를 참조하는 변수
    private Rigidbody grabbedRigidbody = null;
    private Rigidbody weaponRigidbody = null; 
    private Collider grabbedCollider = null;
    // 원래 부모를 저장할 변수
    private Transform originalParent = null;
    // 잡는 중인지 여부확인
    public bool isGrabbingWeapon = false;
    public bool isGrabbingPlayer = false;
    #endregion
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        PlayerGrabAction = playerInput.actions["PlayerGrab"];
        WeaponGrab = playerInput.actions["WeaponGrab"];
    }
    private void Update()
    {
        bool PlayGrabbing = PlayerGrabAction.triggered;
        bool WeaponGrabbing = WeaponGrab.triggered;
        if (PlayGrabbing)
        {
            if (grabbedObject == null)
            {  
                TryGrab();
            }
            else
            {   
                Release();
            }
        }
        if (WeaponGrabbing)
        {
            if (weaponObject == null)
            {      
                Weapongrab();
            }
            else
            {     
                WeaponRelease();
            }
        }
        if (grabbedObject != null)
        {
            LiftObject();
        }
        if(weaponObject != null)
        {
            WeaponLiftObject();
        }
    }
    private void TryGrab()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, grabRadius, transform.forward, out hit, grabRange))
        {
            if (hit.collider.CompareTag("Player"))
            {
                grabbedObject = hit.collider.gameObject;
                grabbedRigidbody = grabbedObject.GetComponent<Rigidbody>();
                grabbedCollider = grabbedObject.GetComponent<Collider>();
                originalParent = grabbedObject.transform.parent;
                grabbedRigidbody.isKinematic = true;
                grabbedCollider.enabled = false;
                grabbedObject.transform.position = holdPosition.position;
                grabbedObject.transform.SetParent(holdPosition);
                isGrabbingPlayer = true;
            }
        }
    }
    private void Weapongrab()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, WeapongrabRadius, transform.forward, out hit, WeapongrabRange))
        {        
            if (hit.collider.CompareTag("Weapon"))
            {
                weaponObject = hit.collider.gameObject;
                weaponRigidbody = weaponObject.GetComponent<Rigidbody>();
                originalParent = weaponObject.transform.parent;
                weaponObject.transform.position = WeaponPosition.position;
                weaponObject.transform.SetParent(WeaponPosition);
                isGrabbingWeapon = true;
            }
        }
    }
    // 오브젝트를 놓는 함수
    private void Release()
    {
        if (grabbedObject != null)
        {
            grabbedObject.transform.SetParent(originalParent);
            grabbedRigidbody.isKinematic = false;
            grabbedCollider.enabled = true; 
            grabbedRigidbody.AddForce(transform.forward * liftForce * 8, ForceMode.Acceleration);
            grabbedObject = null;
            grabbedRigidbody = null;
            grabbedCollider = null;
            originalParent = null;
            isGrabbingPlayer = false;
        }
    }
    private void WeaponRelease()
    {
        if (weaponObject != null)
        {
            weaponObject.GetComponent<Rigidbody>();
            weaponObject.transform.SetParent(originalParent);
            weaponRigidbody.AddForce(transform.up * WeaponForce, ForceMode.Impulse);
            weaponRigidbody.AddForce(transform.forward * WeaponForce, ForceMode.Impulse);
            weaponObject = null;
            originalParent = null;
            isGrabbingWeapon = false;
        }
    }
    public void AllRelease()
    {
        if (grabbedObject != null)
        {
            // 잡은 오브젝트의 부모 설정을 원래 부모로 복원
            grabbedObject.transform.SetParent(originalParent);
            // 오브젝트의 물리적 고정을 해제
            grabbedRigidbody.isKinematic = false;
            grabbedCollider.enabled = true;
            grabbedRigidbody.AddForce(transform.up * liftForce * 2, ForceMode.Impulse);
            grabbedRigidbody.AddForce(transform.forward * liftForce * 5, ForceMode.Impulse);
            // 참조 변수 초기화
            grabbedObject = null;
            grabbedRigidbody = null;
            grabbedCollider = null;
            originalParent = null;
            isGrabbingPlayer = false;
        }
        if (weaponObject != null)
        {
            weaponObject.GetComponent<Rigidbody>(); // 올바른 Rigidbody 가져오기
            weaponObject.transform.SetParent(originalParent);
            weaponRigidbody.AddForce(transform.up * WeaponForce, ForceMode.Impulse);
            weaponRigidbody.AddForce(transform.forward * WeaponForce, ForceMode.Impulse);
            // 참조 변수 초기화
            weaponObject = null;
            originalParent = null;
            isGrabbingWeapon = false;
        }
    }



    // 잡은 오브젝트를 들어올리는 함수
    private void LiftObject()
    {
        // 오브젝트 위치를 holdPosition으로 계속 업데이트
        grabbedObject.transform.position = holdPosition.position;
    }
    private void WeaponLiftObject()
    {
        // 오브젝트 위치를 holdPosition으로 계속 업데이트
        weaponObject.transform.position = WeaponPosition.position;
        weaponObject.transform.rotation = WeaponPosition.rotation;
    }
    public void SetWeaponIsGrabbing(bool value)
    {
        isGrabbingWeapon = value;
    }
    public bool GetWeaponIsGrabbing()
    {
        return isGrabbingWeapon;
    }
    public void SetPlayerIsGrabbing(bool value)
    {
        isGrabbingPlayer = value;
    }
    public bool GetPlayerWeaponIsGrabbing()
    {
        return isGrabbingPlayer;
    }
    // 캐스트 범위 확인하기위해 작성
    private void OnDrawGizmos()
    {      
        // SphereCast의 시작 위치와 방향 설정
        Vector3 start = transform.position;
        //Vector3 direction = transform.forward * grabRange;
        Vector3 direction = transform.forward * WeapongrabRange;
        // SphereCast의 범위를 표시하는 원 그리기
        Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(start, grabRadius);
        //Gizmos.DrawWireSphere(start + direction, grabRadius);
        Gizmos.DrawWireSphere(start, WeapongrabRadius);
        Gizmos.DrawWireSphere(start + direction, WeapongrabRadius);
        // 시작점에서 끝점까지의 선 그리기
        Gizmos.DrawLine(start, start + direction);
    }
}
