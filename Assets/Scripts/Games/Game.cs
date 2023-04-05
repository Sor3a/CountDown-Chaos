using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public interface IGame
{
    public Sprite getSprite();
}

public abstract class Game : MonoBehaviour,IPlayable,IGame
{
    [SerializeField] protected int NumberOfTrys;
    [SerializeField] protected TextMeshProUGUI tryNumber;
    [SerializeField] GameObject game;
    [SerializeField] string GameName;
    [SerializeField] float AmountOfTime;
    [SerializeField] Sprite gameSprite;
    [SerializeField] GameObject marker;
    protected Player playerPlayingGame { private set; get; }
    public string getName() {return GameName;}
    public float getAmmountOfTime(){return AmountOfTime;}
    public Sprite getSprite() { return gameSprite; }

    public delegate void WinLostPuzzle(Player player, in IPlayable game);
    public static event WinLostPuzzle winGame;
    public static event WinLostPuzzle lostGame;
    protected void Intitialize()
    {
        playerPlayingGame = null;
        if (tryNumber)
            tryNumber.text = NumberOfTrys.ToString();
    }
    private void OnTriggerEnter(Collider other)
    {
        var objectEntering = other.gameObject;
        if (objectEntering.tag == "Player")
        {
            var player = objectEntering.GetComponent<Player>();
            if (NumberOfTrys > 0 && player.isHim())
            {
                openGame(player);
                player.closePuzzle(true);
            }
                
        }
    }
    private void OnTriggerExit(Collider other)
    {
        var objectEntering = other.gameObject;
        if (objectEntering.tag == "Player")
        {
            var player = objectEntering.GetComponent<Player>();
            if (player == playerPlayingGame)
            {
                CloseGame();
                
            }
                
        }
    }
    protected void CompleteGame(bool win)
    {
        if (win)
            winGame?.Invoke(playerPlayingGame, this);
        else
            lostGame?.Invoke(playerPlayingGame, this);

        marker.SetActive(false);
    }
    void openGame(Player player)
    {
        game.SetActive(true);
        playerPlayingGame = player;
    }
    public void CloseGame()
    {
        if(game.activeSelf)
        {
            if(playerPlayingGame.isHim())
            {
                playerPlayingGame.closePuzzle();
                game.SetActive(false);
                playerPlayingGame = null;
               
            }
        }
    }
    public virtual void WinGame()
    {
        CompleteGame(true);
        NumberOfTrys--;
        tryNumber.text = NumberOfTrys.ToString();
        if (NumberOfTrys <= 0)
            CloseGame();
    }
    public virtual void LostGame()
    {
        CompleteGame(false);
        NumberOfTrys--;
        tryNumber.text = NumberOfTrys.ToString();
        if (NumberOfTrys <= 0)
            CloseGame();
    }


}
