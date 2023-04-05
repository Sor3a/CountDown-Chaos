using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
public enum puzzleType
{
    WinTime=0,
    WasteOthersTime=1,
    WinObstackle=2,
}
public interface IPlayable
{
    public string getName();
    public float getAmmountOfTime();
}
public abstract class Puzzle : MonoBehaviour,IPlayable
{
    //private static int numberOfPuzzles=0;
    [SerializeField] int puzzId;

    public int id { private set; get; }

    [SerializeField] protected int NumberOfTrys;
    [SerializeField] protected TextMeshProUGUI tryNumber;
    [SerializeField] protected float AmountOfTime;
    [SerializeField] protected puzzleType type;
    protected Player playerSolvingPuzzle { private set; get; }
    [SerializeField] GameObject puzzle;
    bool isOpened;


    [SerializeField] string puzzleName;
    public Sprite puzzleSprite;
    [SerializeField] GameObject marker;

    public delegate void WinLostPuzzle(Player player,in IPlayable puzzle);
    public static event WinLostPuzzle winPuzzle;
    public static event WinLostPuzzle lostPuzzle;
    public abstract void WinPuzzle();
    public abstract void LostPuzzle();

    public puzzleType getPuzzleType() { return type; }
    public string getName() { return puzzleName; }
    public float getAmmountOfTime() //Returns the amount of time that this puzzle give or get
    {   return AmountOfTime;}
    protected void Intitialize()
    {
        isOpened = false;
        //marker.SetActive(false);
        playerSolvingPuzzle = null;
        id = puzzId;
        if (tryNumber)
            tryNumber.text = NumberOfTrys.ToString();
    }
    public void ActivateMarker()
    {
        marker.SetActive(true);
    }
    protected void CompletePuzzle(bool win)
    {
        marker.SetActive(false);
        if (win)
            winPuzzle?.Invoke(playerSolvingPuzzle, this);
        else
            lostPuzzle?.Invoke(playerSolvingPuzzle, this);

    }
    private void OnTriggerEnter(Collider other)
    {
        var objectEntering = other.gameObject;
        if (objectEntering.tag == "Player")
        {
            var player = objectEntering.GetComponent<Player>();
            if (player.canPlayerDoPuzzle(id)) //check if the player have the puzzle on his task list
                OpenPuzzle(player);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        var objectEntering = other.gameObject;
        if (objectEntering.tag == "Player")
        {
            var player = objectEntering.GetComponent<Player>();
            if (player == playerSolvingPuzzle)
                ClosePuzzle();
        }  
    }
    protected virtual void OpenPuzzle(Player player)
    {
       
        puzzle.SetActive(true);
        playerSolvingPuzzle = player;
        isOpened = true;
    }
    public virtual void ClosePuzzle() // close the puzzle and not doing it
    {
        if(isOpened)
        {
            if(playerSolvingPuzzle.isHim())
            {
                isOpened = false;
                playerSolvingPuzzle.closePuzzle();
                puzzle.SetActive(false);
                playerSolvingPuzzle = null;

            }
        }
    }
}
