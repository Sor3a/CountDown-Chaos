using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [SerializeField] List<Menu> menues;
    [SerializeField] TextMeshProUGUI debugger;
    public void OpenMenu(MenuTypes type)
    {
        debugger.text = "";
        foreach (var menu in menues)
        {
            if (menu.type == type)
            {
                menu.gameObject.SetActive(true);
            }  
            else
                menu.gameObject.SetActive(false);
        }
    }
    public void CloseMenu(MenuTypes type)
    {
       var menu= menues.Find(x => x.type == type);
        menu.gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
