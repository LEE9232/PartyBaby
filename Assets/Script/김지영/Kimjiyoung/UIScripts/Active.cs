using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Active : MonoBehaviour
{
    public KeyCode SettingOkey = KeyCode.O;
    private bool SettionWindow = false; 

    // 초기화 false 
    private bool settingActive = false;
    // 활성, 비활성 할 오브젝트를 선택
    public GameObject settingOnOff;

    private bool shopActive = false;
    public GameObject shopOnOff;

    private bool mainCharActive = true;
    public GameObject mainCharOnOff;

    // GamgeManager 인스턴스에 접근하기 위한 참조
    private GameManager gameManager;

    private bool isExitPopupActive = false;

    private void Start()
    {
        // 시작부터 인스펙터에서 비활성안하고 로직으로 비활성화 해서 시작하겠다는 초기화.
        // 초기화 값이 true : (!settingActive) 초기값이 false : (settingActive)
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
        //1. !settingActive                  :  초기값이 false 시작을 했지만 !가 있으니 true가 됐다.
        //2. settingActive = !settingActive  :  !settingActive 가 true 됐으니
        //3. settingActive                   :  true 가 된다.
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

    // O 키 누를시 Setting창 나오게
    private void HandleSettingKey()
    {
        if (Input.GetKeyDown(SettingOkey))
        {
            settingActive = !settingActive;
            settingOnOff.SetActive(settingActive);
        }
    }


}
