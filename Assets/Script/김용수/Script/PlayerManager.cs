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
        // ������ �÷��̾� ��ü ��������
        GameObject newPlayer = playerInput.gameObject;
        // ���� ��ġ ����
        Transform spawnPoint = spawnPoints[playerCount % spawnPoints.Length];
        newPlayer.transform.position = spawnPoint.position;
        playerCount++;
    }
}
