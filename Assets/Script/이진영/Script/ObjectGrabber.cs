using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectGrabber : MonoBehaviour
{
    #region ����
    public KeyCode weaponKey = KeyCode.F;
    public Transform holdPosition;
    public Transform WeaponPosition;
    private PlayerInput playerInput;
    private InputAction PlayerGrabAction;
    private InputAction WeaponGrab;
    // �÷��̾� ��� ��
    // ���ø� �� ������ ���� ũ��
    public float liftForce = 60f; // ���ø��� ��
    public float grabRange = 0.5f; // ��� ����
    public float grabRadius = 0.8f; // ��� ����
    // ���� ��� �� , ��ġ.
    public float WeaponForce = 10f; // ���ø��� ��
    public float WeapongrabRange = 0.5f; // ��� ����
    public float WeapongrabRadius = 0.5f; // ��� ����
    // ���� ��� �ִ� ������Ʈ�� �����ϴ� ����
    private GameObject grabbedObject = null;
    private GameObject weaponObject = null;
    // ���� ������Ʈ�� Rigidbody�� �����ϴ� ����
    private Rigidbody grabbedRigidbody = null;
    private Rigidbody weaponRigidbody = null; 
    private Collider grabbedCollider = null;
    // ���� �θ� ������ ����
    private Transform originalParent = null;
    // ��� ������ ����Ȯ��
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
    // ������Ʈ�� ���� �Լ�
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
            // ���� ������Ʈ�� �θ� ������ ���� �θ�� ����
            grabbedObject.transform.SetParent(originalParent);
            // ������Ʈ�� ������ ������ ����
            grabbedRigidbody.isKinematic = false;
            grabbedCollider.enabled = true;
            grabbedRigidbody.AddForce(transform.up * liftForce * 2, ForceMode.Impulse);
            grabbedRigidbody.AddForce(transform.forward * liftForce * 5, ForceMode.Impulse);
            // ���� ���� �ʱ�ȭ
            grabbedObject = null;
            grabbedRigidbody = null;
            grabbedCollider = null;
            originalParent = null;
            isGrabbingPlayer = false;
        }
        if (weaponObject != null)
        {
            weaponObject.GetComponent<Rigidbody>(); // �ùٸ� Rigidbody ��������
            weaponObject.transform.SetParent(originalParent);
            weaponRigidbody.AddForce(transform.up * WeaponForce, ForceMode.Impulse);
            weaponRigidbody.AddForce(transform.forward * WeaponForce, ForceMode.Impulse);
            // ���� ���� �ʱ�ȭ
            weaponObject = null;
            originalParent = null;
            isGrabbingWeapon = false;
        }
    }



    // ���� ������Ʈ�� ���ø��� �Լ�
    private void LiftObject()
    {
        // ������Ʈ ��ġ�� holdPosition���� ��� ������Ʈ
        grabbedObject.transform.position = holdPosition.position;
    }
    private void WeaponLiftObject()
    {
        // ������Ʈ ��ġ�� holdPosition���� ��� ������Ʈ
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
    // ĳ��Ʈ ���� Ȯ���ϱ����� �ۼ�
    private void OnDrawGizmos()
    {      
        // SphereCast�� ���� ��ġ�� ���� ����
        Vector3 start = transform.position;
        //Vector3 direction = transform.forward * grabRange;
        Vector3 direction = transform.forward * WeapongrabRange;
        // SphereCast�� ������ ǥ���ϴ� �� �׸���
        Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(start, grabRadius);
        //Gizmos.DrawWireSphere(start + direction, grabRadius);
        Gizmos.DrawWireSphere(start, WeapongrabRadius);
        Gizmos.DrawWireSphere(start + direction, WeapongrabRadius);
        // ���������� ���������� �� �׸���
        Gizmos.DrawLine(start, start + direction);
    }
}
