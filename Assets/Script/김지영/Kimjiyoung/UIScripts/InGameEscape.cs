using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameEscape : MonoBehaviour
{
    public GameObject PopupMenu;

    private Animator PopupMenuAnimator;

    private bool MenuActive = false;

    public bool settings = false;

    public SettingsMenu _settingMenu;

    private void Start()
    {
        PopupMenuAnimator = PopupMenu.GetComponent<Animator>();
        //PopupMenu.SetActive(false);
    }

    private void Update()
    {
        HandleEscKeyClick();
    }

    private void HandleEscKeyClick()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_settingMenu.SettingsMenuActive == false)
            {
                MenuActive = !MenuActive;
                PopupMenu.SetActive(MenuActive);
                PopupMenuAnimator.Play("PopupAnimation");

            }
            if (_settingMenu.SettingsMenuActive == true)
            {
                _settingMenu.OnSettingsMenuButton();
            }

        }



    }

    public void ExitGame2()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
