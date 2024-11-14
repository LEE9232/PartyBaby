using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopLabel : MonoBehaviour
{
    #region private
    private bool faceOnActive = true;
    private bool skinOnActive = false;

    private bool faceOffActive = false;
    private bool skinOffActive = true;
    #endregion

    #region public
    public GameObject faceOn;
    public GameObject faceOff;
    public GameObject skinOn;
    public GameObject skinOff;
    #endregion

    // face를 눌렀을 때, skin은 off상태가 되어야한다.
    // ㄴ face_01 On face_02 Off, skin_01 Off skin_02 On

    public void OnFaceClick()
    {
        // face on 비활성화일때만  if를 실행하겠다.
        // 이유는 : 비활성화는 1번만 호출이되면 활성화가 되기때문
        // 비활성화를 누르면 활성화가 되기때문에 1번만 호출하면 됌.
        if (!faceOnActive)
        {
            // 값을 지정해주는 부분
            // faceOffActive = !faceOffActive
            // !faceOffActive 이 값을 faceOffActive 적용하겠다. 이기때문에
            // faceOffActive =  이 초기 값이 false 였다면 true로 바꾼다.
            // faceOffActive =  이 초기 값이 true 였다면 false로 바꾼다.
            faceOffActive = !faceOffActive; // true -> false
            faceOnActive = !faceOnActive; // false -> true

            // 실제 유니티에서 적용되는 부분
            // button Object를
            // SetActive(faceOffActive) 껏다 켯다 기능을 이용해, 활성 비활성을 해줄것이다.
            faceOff.SetActive(faceOffActive);
            faceOn.SetActive(faceOnActive);

            // 얼굴을 눌러서 얼굴이 활성화가 될거니까 스킨아 넌 꺼져.
            skinOnActive = !skinOnActive;
            skinOn.SetActive(skinOnActive);

            skinOffActive = !skinOffActive;
            skinOff.SetActive(skinOffActive);
        }
        // 이미 너가 활성화 상태라면..
        // 넌 그냥 활성화야....
        else
        {
            // face On을 계속 true 상태로 두겠다
            faceOnActive = true; // 넌 계속 on 상태로 true여야해.
            faceOn.SetActive(faceOnActive);
            // 난 얼굴 활성화를 광클할거니까 스킨 비활성화 창은 넌 계속 true해라.
            skinOffActive = true;
            skinOff.SetActive(skinOffActive);
        }
    }

    public void OnSkinClick()
    {
        if (!skinOnActive) 
        {
            skinOnActive=!skinOnActive;
            skinOn.SetActive(skinOnActive);

            skinOffActive = !skinOffActive;
            skinOff.SetActive(skinOffActive);

            faceOffActive = !faceOffActive;
            faceOff.SetActive(faceOffActive);
            
            faceOnActive=!faceOnActive;
            faceOn.SetActive(faceOnActive);
        }
        else
        {
            faceOffActive=true;
            faceOff.SetActive(faceOffActive);

            skinOnActive = true;
            skinOn.SetActive(skinOnActive);
        }
    }
}