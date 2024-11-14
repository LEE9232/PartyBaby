using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject exitPopup;
    public GameObject blurBackground;
    private Animator exitPopupAnimator;

    private bool isPopupActive = false;

    private static GameManager instance;

    // GameManager�� �̱������� �����ϱ� ���� ������Ƽ
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

    }

    void Start()
    {
        // GameManager ������Ʈ�� �� ��ȯ �� �ı����� �ʵ��� ����
        //DontDestroyOnLoad(gameObject);

        exitPopupAnimator = exitPopup.GetComponent<Animator>();
        exitPopup.SetActive(false);
        blurBackground.SetActive(false);
    }

    public void ShowExitPopup()
    {
        exitPopup.SetActive(true);
        blurBackground.SetActive(true);

        if (exitPopupAnimator != null)
        {
            exitPopupAnimator.Play("PopupAnimation"); // �ִϸ��̼� ���
        }
        isPopupActive = true;
    }

    public void CloseExitPopup()
    {
        exitPopup.SetActive(false);
        blurBackground.SetActive(false);
        isPopupActive = false;
    }

    public void ExitGame()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }

    public bool IsPopupActive()
    {
        return isPopupActive;
    }
}
