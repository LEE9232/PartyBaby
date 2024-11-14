using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // SceneManager�� ����ϱ� ���� ����

public class ChangeSceneScript : MonoBehaviour
{
    // joinroomSceneChange �̸��� �Լ� ����
    public void joinroomSceneChange()
    {
        // SceneManager �޼����� LoadScene �Լ��� ���� JoinRommUI.scene���� �� ��ȯ
        SceneManager.LoadScene("JoinRoomUI");
    }

    public void mainSceneChage()
    {
        // MainUI Scene ��ȯ
        SceneManager.LoadScene("MainUI");
    }
    //public void GameRoom()
    //{
    //    SceneManager.LoadScene("Lost Temple Scene");
    //    Debug.Log("���ӷ� ����");
    //}

    public void GameScene()
    {

        //LoadingSceneController.Instance.LoadScene("Lost Temple Scene");
        SceneManager.LoadScene("Lost Temple Scene");
        Debug.Log("ȣ��");

    }

    public void ToLobbyScene()
    {
        LoadingSceneController.Instance.LoadScene("JoinRoomUI");
    }

}
