using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerMangment : Puzzle
{
    [SerializeField] PowerUIManager powerManger;

    private void Awake()
    {
        Intitialize(); //from the parent puzzle 
    }

    public void checkGame(PowerHolder holder)
    {
        bool didwin= false;
        if(!powerManger.CheckCatchAndWin(holder, out didwin))
        {
            LostPuzzle();
        }
        else
        {
            if (didwin)
                WinPuzzle();
        }
    }
    public override void LostPuzzle()
    {
        if (playerSolvingPuzzle != null)
        {
            CompletePuzzle(false);
            //playerSolvingPuzzle.FinishPuzzle(id, false);
            ClosePuzzle();
        }
    }
    public override void WinPuzzle()
    {
        if (playerSolvingPuzzle != null)
        {
            CompletePuzzle(true);
            //playerSolvingPuzzle.addTimeToPlayer(AmountOfTime);
            //playerSolvingPuzzle.FinishPuzzle(id, true);
            ClosePuzzle();
        }
    }
}
