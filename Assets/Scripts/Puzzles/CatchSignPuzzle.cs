using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CatchSignPuzzle : Puzzle
{
    string correctAnswer;
    [SerializeField] TMP_InputField answer;
    int indexKeySelected;
    [SerializeField] TextMeshProUGUI puzzleText;
    [SerializeField] float time;


    string[] keys =
    {
        "copycopy",
        "doyouhear ",
        "didyoucopy",
        
    };
    bool gamePlaying = false;

    public override void ClosePuzzle()
    {
        base.ClosePuzzle();
        gamePlaying = false;
    }
    private void Awake()
    {
        Intitialize(); //from the parent puzzle 
        indexKeySelected = Random.Range(0, keys.Length);
        correctAnswer = keys[indexKeySelected];
    }
    IEnumerator giveHimTimeThenLose()
    {
        yield return new WaitForSeconds(5f);
        LostPuzzle();
    }
    IEnumerator showPartOfPuzzle(int index,float time)
    {
        yield return new WaitForSeconds(time / 3f);
        puzzleText.text = correctAnswer.Substring(index, 2);
        yield return new WaitForSeconds(time);
        puzzleText.text = "";
        if (index + 2 < keys[indexKeySelected].Length)
            StartCoroutine(showPartOfPuzzle(index + 2, time / 1.2f));
        else
            StartCoroutine(giveHimTimeThenLose()); 
    }
    public void Ready()
    {
        if(!gamePlaying)
        {
            indexKeySelected = Random.Range(0, keys.Length);
            correctAnswer = keys[indexKeySelected];
            gamePlaying = true;
            StartCoroutine(showPartOfPuzzle(0, time));
        }
    }
    public void checkAnswer()
    {
        if (answer.text.ToLower().Replace(" ","").Equals(correctAnswer.Replace(" ", "")))
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
            
            ClosePuzzle();
        }
    }

    public override void WinPuzzle()
    {
        if (playerSolvingPuzzle != null)
        {
            CompletePuzzle(true);
            ClosePuzzle();
        }
    }
}
