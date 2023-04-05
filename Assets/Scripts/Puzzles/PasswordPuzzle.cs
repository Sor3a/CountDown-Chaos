using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PasswordPuzzle : Puzzle
{
    [SerializeField] string correctAnswer;
    [SerializeField] TMP_InputField answer;



    private void Awake()
    {
        Intitialize(); //from the parent puzzle 
    }

    public void checkAnswer()
    {
        if(answer.text.ToUpper().Equals(correctAnswer))
        {
            WinPuzzle();
        }
        else
        {
            NumberOfTrys--;
            tryNumber.text = NumberOfTrys.ToString();
            if (NumberOfTrys <= 0)
                LostPuzzle();
        }
    }

    public override void LostPuzzle()
    {
        if (playerSolvingPuzzle != null)
        {
            CompletePuzzle(false);
            //playerSolvingPuzzle.FinishPuzzle(id,false);
            ClosePuzzle();
        }
    }

    public override void WinPuzzle()
    {
        if(playerSolvingPuzzle!=null)
        {
            // winPuzzle
            //Puzzle.winPuzzle(playerSolvingPuzzle,this);
            CompletePuzzle(true);
           // playerSolvingPuzzle.GetTimeForOthers(AmountOfTime);
            //playerSolvingPuzzle.FinishPuzzle(id,true);
            ClosePuzzle();
        }
    }

}
