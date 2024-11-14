using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class GameCountDown : MonoBehaviour
{
    public int countdownTime = 3;
    private string endgame;
    public TextMeshProUGUI countdownDisplay;
    public TextMeshProUGUI gameEndText;
    public PlayerController PlayerMoveCount;
    public ChangeSceneScript changScene;
    private bool Endbool = false; 
    void Start()
    {
        StartCoroutine(Gamecount());
    }
    private void Update()
    {
        // 승리 UI 적용할곳
    }
    IEnumerator Gamecount()
    {
        while (countdownTime > 0)
        {
            countdownDisplay.text = countdownTime.ToString();
            yield return new WaitForSeconds(1);
            countdownTime--;
        }
        countdownDisplay.text = "Game Start!";
        PlayerMoveCount.CountDown = true;
        yield return new WaitForSeconds(1);
        countdownDisplay.gameObject.SetActive(false);
    }
    public void WinnerText()
    {        
        gameEndText.text = "Game End!";
        gameEndText.gameObject.SetActive(true);
        StartCoroutine(GameEndText());
    }


    public IEnumerator GameEndText()
    {
        yield return new WaitForSeconds(3.0f);
        changScene.mainSceneChage();
    }
}
