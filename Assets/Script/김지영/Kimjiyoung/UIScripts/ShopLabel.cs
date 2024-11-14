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

    // face�� ������ ��, skin�� off���°� �Ǿ���Ѵ�.
    // �� face_01 On face_02 Off, skin_01 Off skin_02 On

    public void OnFaceClick()
    {
        // face on ��Ȱ��ȭ�϶���  if�� �����ϰڴ�.
        // ������ : ��Ȱ��ȭ�� 1���� ȣ���̵Ǹ� Ȱ��ȭ�� �Ǳ⶧��
        // ��Ȱ��ȭ�� ������ Ȱ��ȭ�� �Ǳ⶧���� 1���� ȣ���ϸ� ��.
        if (!faceOnActive)
        {
            // ���� �������ִ� �κ�
            // faceOffActive = !faceOffActive
            // !faceOffActive �� ���� faceOffActive �����ϰڴ�. �̱⶧����
            // faceOffActive =  �� �ʱ� ���� false ���ٸ� true�� �ٲ۴�.
            // faceOffActive =  �� �ʱ� ���� true ���ٸ� false�� �ٲ۴�.
            faceOffActive = !faceOffActive; // true -> false
            faceOnActive = !faceOnActive; // false -> true

            // ���� ����Ƽ���� ����Ǵ� �κ�
            // button Object��
            // SetActive(faceOffActive) ���� �ִ� ����� �̿���, Ȱ�� ��Ȱ���� ���ٰ��̴�.
            faceOff.SetActive(faceOffActive);
            faceOn.SetActive(faceOnActive);

            // ���� ������ ���� Ȱ��ȭ�� �ɰŴϱ� ��Ų�� �� ����.
            skinOnActive = !skinOnActive;
            skinOn.SetActive(skinOnActive);

            skinOffActive = !skinOffActive;
            skinOff.SetActive(skinOffActive);
        }
        // �̹� �ʰ� Ȱ��ȭ ���¶��..
        // �� �׳� Ȱ��ȭ��....
        else
        {
            // face On�� ��� true ���·� �ΰڴ�
            faceOnActive = true; // �� ��� on ���·� true������.
            faceOn.SetActive(faceOnActive);
            // �� �� Ȱ��ȭ�� ��Ŭ�ҰŴϱ� ��Ų ��Ȱ��ȭ â�� �� ��� true�ض�.
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