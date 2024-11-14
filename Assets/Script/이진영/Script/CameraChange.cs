using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChange : MonoBehaviour
{
    #region º¯¼ö
    public GameObject CameraUI;
    public GameObject KeyMenu;
    private bool CameraActive = false; 
    private bool KeyActive = true;
    public  KeyCode CameraKey = KeyCode.B;
    #endregion
    private void Start()
    {
        StartCoroutine(KeyMenual());
    }
    private void Update()
    {
        CameraOnOff();
    }
    public void CameraOnOff()
    {
        if (Input.GetKeyDown(CameraKey))
        {
            CameraActive = !CameraActive;
            CameraUI.SetActive(CameraActive);
        }
    }
    IEnumerator KeyMenual()
    {
        yield return new WaitForSeconds(8.0f);
        KeyActive = false ;
        KeyMenu.SetActive(KeyActive);  
    }
}
