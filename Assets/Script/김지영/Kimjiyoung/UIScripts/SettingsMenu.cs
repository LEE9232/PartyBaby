using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public bool SettingsMenuActive = false;
    public GameObject settingsMenuOnOff;

    public void OnSettingsMenuButton()
    {
        SettingsMenuActive = !SettingsMenuActive;
        settingsMenuOnOff.SetActive(SettingsMenuActive);
    }
}
