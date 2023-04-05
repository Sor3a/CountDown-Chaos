using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePuzzle : Puzzle
{
    char[,] puzzle;
    char[] values = { 'A', 'B', 'C' };
    [SerializeField] MainHolder puzzleHolder;
    List<Holder> selected;
    private void Awake()
    {
        selected = new List<Holder>();
        Intitialize();
        puzzle = new char[3,3];
    }
    void CreateRandomPuzzle()
    {
        int[] counts = { 0, 0, 0 };
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                // Keep generating random values until we find one that hasn't been used 3 times
                int index;
                do
                {
                    index = Random.Range(0, 3);
                } while (counts[index] >= 3);

                // Assign the value to the matrix and update the counts array
                puzzle[i, j] = values[index];
                counts[index]++;
            }
        }
    }
    public void select(Holder holder)
    {
        selected.Add(holder);
        if (selected.Count==2)
        {
            char first = selected[0].getKey();
            selected[0].SetKey(selected[1].getKey());
            selected[1].SetKey(first);
            selected.Clear();
        }
    }
    bool IsPuzzleCorrect(char[,] answer)
    {
        HashSet<char> rowChars = new HashSet<char>();
        HashSet<char> colChars = new HashSet<char>();

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
               
                if (!rowChars.Add(answer[i, j]))
                {
                    return false;
                }
                
                if (!colChars.Add(answer[j, i]))
                {
                    return false;
                }
            }
            rowChars.Clear(); // reset row characters for next iteration
            colChars.Clear(); // reset column characters for next iteration
        }
        return true;
    }
    public void checkAnswer()
    {
        var answer = puzzleHolder.getAnswer();
        if (IsPuzzleCorrect(answer))
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
    protected override void OpenPuzzle(Player player)
    {
        //Debug.Log("opend2");
        base.OpenPuzzle(player);
        CreateRandomPuzzle();
        if (puzzleHolder.gameObject.activeSelf)
            puzzleHolder.InitializeGame(puzzle);
    }
    public void finsihTuto()
    {
        puzzleHolder.InitializeGame(puzzle);
    }
    
    public override void WinPuzzle()
    {
        if (playerSolvingPuzzle != null)
        {
            CompletePuzzle(true);
            //playerSolvingPuzzle.addTimeToPlayer(AmountOfTime);
            //playerSolvingPuzzle.FinishPuzzle(id,true);
            ClosePuzzle();
        }
    }
}
