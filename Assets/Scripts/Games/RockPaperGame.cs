using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public enum GameChoice
{
    Rock=0,
    Paper=1,
    Scissors=2,
    none=3,
}
public enum GameStatus
{
    tie,
    win,
    lost,
}
public class RockPaperGame : MonoBehaviour, IPlayable,IGame
{
    GameChoice lastGameChoice = GameChoice.none;
    Player lastPlayerChoice;
    int NumberOfCurrentPlayers;
    Player currentPlayer;
    [SerializeField] GameObject gamePanel;
    PhotonView pv;
    float ammountOfTime;
    [SerializeField] string gameName;
    [SerializeField] TMP_InputField amountToPlayerFor;
    [SerializeField] TextMeshProUGUI amountText,announce;
    bool GameWorking = false;
    [SerializeField] Sprite sprite;
    public Sprite getSprite() { return sprite; }

    public delegate void WinLostGame(Player player, in IPlayable puzzle);
    public static event WinLostGame winGame;
    public static event WinLostGame lostGame;
    public string getName() { return gameName; }
    public float getAmmountOfTime() { return ammountOfTime; }


    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        lastPlayerChoice = null;
        NumberOfCurrentPlayers = 0;
        ammountOfTime = 0;
        winGame += WinAnnouce;
        lostGame += LostAnnouce;
    }
    IEnumerator desactivateText()
    {
        yield return new WaitForSeconds(2f);
        announce.text = "";
    }
    void WinAnnouce(Player player, in IPlayable puzzle)
    {
        if(player.isHim())
        {
            announce.text = "You won 20 seconds";
            StartCoroutine(desactivateText());
        }
        
    }
    void LostAnnouce(Player player, in IPlayable puzzle)
    {
        if(player.isHim())
        {
            StartCoroutine(desactivateText());
            announce.text = "You lost 20 seconds";
        }
        
    }

    public void FindPlayer()
    {
        if(!string.IsNullOrEmpty(amountToPlayerFor.text) && ammountOfTime == 0 && !GameWorking)
        {
            if(float.TryParse(amountToPlayerFor.text,out float amount) && amount!=0)
            {
                Player player = findPlayer(PhotonNetwork.LocalPlayer.ActorNumber);
                GlobalUI.instance.ShowMsgForEveryOne(player.NickName + " wants to play rock paper scissors for " + amount + " seconds");
                pv.RPC("setAmount", RpcTarget.AllBuffered, amount);
            }
        }
    }

    [PunRPC]
    void setAmount(float amount)
    {
        ammountOfTime = amount;
        GameWorking = true;
        amountText.text = amount.ToString();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && NumberOfCurrentPlayers < 2)
        {
            Player player = other.GetComponent<Player>();
            if (player.isHim())
            {
                currentPlayer = player;
                pv.RPC("PlayerEnterQuit", RpcTarget.AllBuffered, true);
                gamePanel.SetActive(true);
                player.closePuzzle(true);
            }

        }
    }


    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            var player = other.GetComponent<Player>();
            if(player.isHim())
            {
                pv.RPC("PlayerEnterQuit", RpcTarget.AllBuffered, false);
                gamePanel.SetActive(false);
                player.closePuzzle();
                if (currentPlayer == null) return;
                if (currentPlayer.getActorNumber() == player.getActorNumber())
                    pv.RPC("updateCurrentPlayerChoice", RpcTarget.AllBuffered);
            }
        }
    }
    [PunRPC]
    void updateCurrentPlayerChoice()
    {
        currentPlayer = null;
        lastGameChoice = GameChoice.none;
    }
    public void CloseGame()
    {
        Player player = findPlayer(PhotonNetwork.LocalPlayer.ActorNumber);

        pv.RPC("PlayerEnterQuit", RpcTarget.AllBuffered, false);
        gamePanel.SetActive(false);
        player.closePuzzle();
        if (currentPlayer == null) return;
        if (currentPlayer.getActorNumber() == player.getActorNumber())
            pv.RPC("updateCurrentPlayerChoice", RpcTarget.AllBuffered);

    }

    public void takeAnswer(int type)
    {

        pv.RPC("SetPlayerAnswer", RpcTarget.All, type, PhotonNetwork.LocalPlayer.ActorNumber);

    }

    GameStatus DetermineWinner(GameChoice player1, GameChoice player2)
    {
        if (player1 == player2)
        {
            // It's a tie
            return GameStatus.tie;
        }
        else if (player1 == GameChoice.Paper && player2 == GameChoice.Rock ||
                 player1 == GameChoice.Rock && player2 == GameChoice.Scissors ||
                 player1 == GameChoice.Scissors && player2 == GameChoice.Paper)
        {
            // Player 1 wins
            return GameStatus.win;
        }
        else
        {
            // Player 2 wins
            return GameStatus.lost;
        }
    }

    [PunRPC]
    void PlayerEnterQuit(bool didEnter)
    {
        if (didEnter)
            NumberOfCurrentPlayers++;
        else
        {
            NumberOfCurrentPlayers--;
        }
            

        if (NumberOfCurrentPlayers <= 0)
        {
            ammountOfTime = 0;
            GameWorking = false;
            amountText.text = ammountOfTime.ToString();
        }
            
    }
    Player findPlayer(int actorNumb)
    {
        Player player = null;
        var players = GameManager.instance.playerList;
        foreach (var item in players)
        {
            // Debug.Log("list" + item.getActorNumber());
            if (item.getActorNumber() == actorNumb)
            {

                player = item;
                break;
            }
        }
        return player;
    }
    [PunRPC]
    void SetPlayerAnswer(int type, int actorNumb)
    {
        if (!GameWorking || ammountOfTime == 0) return;
        Player player = findPlayer(actorNumb);
        if (player == null) return;
        if (lastPlayerChoice == null)
        {
            lastPlayerChoice = player;
            lastGameChoice = (GameChoice)type;
            //Debug.LogError("first" + lastGameChoice);
        }
        else if (lastPlayerChoice.getActorNumber() != player.getActorNumber())
        {
            var gameCondition = DetermineWinner((GameChoice)type, lastGameChoice);
           //Debug.LogError(gameCondition);
            if (gameCondition == GameStatus.win)
            {
                if (player.isHim())
                    winGame?.Invoke(player, this); 
                if (lastPlayerChoice.isHim())
                    lostGame?.Invoke(lastPlayerChoice, this);
            }
            else if (gameCondition == GameStatus.lost)
            {
                if (lastPlayerChoice.isHim())
                    winGame?.Invoke(lastPlayerChoice, this);
                if (player.isHim())
                    lostGame?.Invoke(player, this);
            }
            lastPlayerChoice = null;
            lastGameChoice = GameChoice.none;
        }

    }

}
