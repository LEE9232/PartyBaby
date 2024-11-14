using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Active : MonoBehaviour
{
    public KeyCode SettingOkey = KeyCode.O;
    private bool SettionWindow = false; 

    // �ʱ�ȭ false 
    private bool settingActive = false;
    // Ȱ��, ��Ȱ�� �� ������Ʈ�� ����
    public GameObject settingOnOff;

    private bool shopActive = false;
    public GameObject shopOnOff;

    private bool mainCharActive = true;
    public GameObject mainCharOnOff;

    // GamgeManager �ν��Ͻ��� �����ϱ� ���� ����
    private GameManager gameManager;

    private bool isExitPopupActive = false;

    private void Start()
    {
        // ���ۺ��� �ν����Ϳ��� ��Ȱ�����ϰ� �������� ��Ȱ��ȭ �ؼ� �����ϰڴٴ� �ʱ�ȭ.
        // �ʱ�ȭ ���� true : (!settingActive) �ʱⰪ�� false : (settingActive)
        //settingOnOff.SetActive(settingActive);
        gameManager = GameManager.Instance;
    }

    void Update()
    {

        HandleEscKey();
        HandleSettingKey();
    }

    public void OnSettingClick()
    {
        //1. !settingActive                  :  �ʱⰪ�� false ������ ������ !�� ������ true�� �ƴ�.
        //2. settingActive = !settingActive  :  !settingActive �� true ������
        //3. settingActive                   :  true �� �ȴ�.
        settingActive = !settingActive;
        settingOnOff.SetActive(settingActive);
    }

    public void OnShopClick()
    {
        shopActive = !shopActive;
        shopOnOff.SetActive(shopActive);

        mainCharActive = !mainCharActive;
        mainCharOnOff.SetActive(mainCharActive);
    }

    private void HandleEscKey()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {            
            if (isExitPopupActive)
            {
                gameManager.CloseExitPopup();
                isExitPopupActive=false;
            }
            else if (settingActive)
            {
                settingActive = false;
                settingOnOff.SetActive(false);
            }
            else if (shopActive)
            {
                shopActive = false;
                shopOnOff.SetActive(false);

                mainCharActive = true;
                mainCharOnOff.SetActive(true);
            }
            else
            {
                //GameManager.Instance.ShowExitPopup();
                gameManager.ShowExitPopup();
                isExitPopupActive = true;
            }


        }
    }

    public void OnExitButtonClick()
    {
        if (isExitPopupActive)
        {
            gameManager.CloseExitPopup();
            isExitPopupActive=false;
        }
        else
        {
            gameManager.ShowExitPopup();
            isExitPopupActive=true;
        }
    }

    // O Ű ������ Settingâ ������
    private void HandleSettingKey()
    {
        if (Input.GetKeyDown(SettingOkey))
        {
            settingActive = !settingActive;
            settingOnOff.SetActive(settingActive);
        }
    }


}
