using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // SceneManager를 사용하기 위한 참조

public class ChangeSceneScript : MonoBehaviour
{
    // joinroomSceneChange 이름의 함수 선언
    public void joinroomSceneChange()
    {
        // SceneManager 메서드의 LoadScene 함수를 통해 JoinRommUI.scene으로 씬 전환
        SceneManager.LoadScene("JoinRoomUI");
    }

    public void mainSceneChage()
    {
        // MainUI Scene 전환
        SceneManager.LoadScene("MainUI");
    }
    //public void GameRoom()
    //{
    //    SceneManager.LoadScene("Lost Temple Scene");
    //    Debug.Log("게임룸 입장");
    //}

    public void GameScene()
    {

        //LoadingSceneController.Instance.LoadScene("Lost Temple Scene");
        SceneManager.LoadScene("Lost Temple Scene");
        Debug.Log("호출");

    }

    public void ToLobbyScene()
    {
        LoadingSceneController.Instance.LoadScene("JoinRoomUI");
    }

}
