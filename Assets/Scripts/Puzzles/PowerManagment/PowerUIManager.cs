using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUIManager : MonoBehaviour //this script is for the managment of the UI and the game of the power
{
    int number;
    private void Start()
    {
        number = 0;
    }

    public bool CheckCatchAndWin(PowerHolder holder,out bool didWin)
    {
        didWin = false;
        if (holder.DidCatchIt())
        {
            number++;
            if (number >= 3)
                didWin = true;
            return true;
        }
        return false; // he didn't even catch it
    }
}
