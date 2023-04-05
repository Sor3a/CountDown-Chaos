using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] List<Puzzle> puzzles;

    [SerializeField] List<Puzzle> fixedListe; //this liste will be added to everyone
    public void FillPlayerPuzzles(ref List<Puzzle> puzz)
    {
        foreach (var item in fixedListe)
        {
            puzz.Add(item);
            item.ActivateMarker();
        }
        HashSet<Puzzle> PasswordPuzzles = new HashSet<Puzzle>();
        while (PasswordPuzzles.Count<2) //choose two random passwords games
        {
            PasswordPuzzles.Add(puzzles[Random.Range(0, puzzles.Count)]);
        }
        foreach (var item in PasswordPuzzles)
        {
            item.ActivateMarker();
            puzz.Add(item);
        }
    }
}
