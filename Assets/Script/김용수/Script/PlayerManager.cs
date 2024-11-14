using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform[] spawnPoints;
    private int playerCount = 0;

    private void OnEnable()
    {
        PlayerInputManager.instance.onPlayerJoined += OnPlayerJoined;
    }

    private void OnDisable()
    {
        PlayerInputManager.instance.onPlayerJoined -= OnPlayerJoined;
    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        // 생성된 플레이어 객체 가져오기
        GameObject newPlayer = playerInput.gameObject;
        // 스폰 위치 설정
        Transform spawnPoint = spawnPoints[playerCount % spawnPoints.Length];
        newPlayer.transform.position = spawnPoint.position;
        playerCount++;
    }
}
