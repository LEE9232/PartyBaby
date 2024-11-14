using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameEnd : MonoBehaviour
{
    private PlayerController[] players; // 모든 플레이어를 담는 배열

    void Start()
    {
        // 모든 Player 컴포넌트를 가진 오브젝트를 찾아 배열에 저장
        players = FindObjectsOfType<PlayerController>();
    }

    void Update()
    {
        // 활성화된 플레이어만 필터링
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
