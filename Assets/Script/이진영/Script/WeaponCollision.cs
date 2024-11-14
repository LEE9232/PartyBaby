using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollision : MonoBehaviour
{
    public PlayerController[] weaponOwners;
    private void Awake()
    {
        if (weaponOwners == null)
        {
            Debug.LogError("WeaponOwner가 초기화되지 않았습니다. " + gameObject.name);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 충돌한 객체가 플레이어인지 확인
        if (collision.collider.CompareTag("Player"))
        {
            var otherPlayer = collision.collider.GetComponent<PlayerController>();
            if (otherPlayer != null)
            {
                foreach (var owner in weaponOwners)
                {
                    if (owner.currentState == PlayerState.OneHandedWeaponAttack && otherPlayer != owner)
                    {
                        otherPlayer.SetStunned(true);
                        Debug.Log("Player Stunned: " + otherPlayer.name);
                        break; // 여러 플레이어를 처리하는 경우, 한 번만 처리하도록 break
                    }
                }
            }
        }
    }
    public void SetWeaponOwner(PlayerController[] playerController)
    {
        weaponOwners = playerController;
    }
}
