using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameEnd : MonoBehaviour
{
    private PlayerController[] players; // ��� �÷��̾ ��� �迭

    void Start()
    {
        // ��� Player ������Ʈ�� ���� ������Ʈ�� ã�� �迭�� ����
        players = FindObjectsOfType<PlayerController>();
    }

    void Update()
    {
        // Ȱ��ȭ�� �÷��̾ ���͸�
        PlayerController[] activePlayers = players.Where(p => p.isActive).ToArray();
        if (activePlayers.Length == 1)
        {
            PlayerController winner = activePlayers[0];
            Debug.Log($"The winner is Player with tag {winner.gameObject.tag}");
        }
    }
    public void EliminatePlayer(PlayerController player)
    {
        player.Eliminate();
    }
}
