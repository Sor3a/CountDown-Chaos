using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsUI : MonoBehaviour
{
    bool muted = false;
    [SerializeField] PlayerControllor playerControllor;
    public void MuteAudios()
    {
        muted = !muted;
        var sources = FindObjectsOfType<AudioSource>();
        foreach (var item in sources)
        {
            item.mute = muted;
        }
    }

    public void SetSensetivity(float value)
    {
        playerControllor.setSensetivity(value*300f);
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
