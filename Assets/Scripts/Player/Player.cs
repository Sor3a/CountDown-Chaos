using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    List<Puzzle> puzzlesNeedToSolve;
    PhotonView pv;
    Timer timer;
    [SerializeField] TextMeshProUGUI nickName;
    [SerializeField] TasksUIMain tasksUIParent;//the parent of the tasks
    [SerializeField] MapUIManager mapManager;
    [SerializeField] RectTransform playerInMap;
    
    public bool playerDoingPuzzle { private set; get; }
    public bool playerLost { private set; get; }
    public string NickName { private set; get; }
    int actorNumber;
    [PunRPC]
    void setUpActorNumber(int numb)
    {
        actorNumber = numb;
    }
    public int getActorNumber()
    {
        return actorNumber;
    }
    private void OnEnable()
    {
        pv = GetComponent<PhotonView>();
        GameManager.instance.addPlayer(this);
        if (!pv.IsMine) return;
        pv.RPC("setUpActorNumber", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.ActorNumber);
        playerDoingPuzzle = false;
        puzzlesNeedToSolve = new List<Puzzle>();
        timer = GetComponent<Timer>();
        FindObjectOfType<PuzzleManager>().FillPlayerPuzzles(ref puzzlesNeedToSolve);
        StartCoroutine(showTasks());
        //NickName = PhotonNetwork.NickName;
        pv.RPC("setPlayerNickName", RpcTarget.AllBuffered, PhotonNetwork.NickName);
        playerLost = false;
        Puzzle.winPuzzle += FinishPuzzleWin;
        Puzzle.lostPuzzle += FinishPuzzleLost;
        Game.winGame += FinishGameWin;
        Game.lostGame += FinishGameLost;

        RockPaperGame.winGame += FinishGameWin;
        RockPaperGame.lostGame += FinishGameLost;

        
    }
    IEnumerator showTasks()
    {
        yield return new WaitForSeconds(4f);
        ShowPuzzles();
    }
    private void OnDestroy()
    {
        Puzzle.winPuzzle -= FinishPuzzleWin;
        Puzzle.lostPuzzle -= FinishPuzzleLost;

        Game.winGame -= FinishGameWin;
        Game.lostGame-= FinishGameLost;

        RockPaperGame.winGame -= FinishGameWin;
        RockPaperGame.lostGame -= FinishGameLost;
        GameManager.instance.RemovePlayer(this);
    }
    void FinishGameWin(Player player, in IPlayable puzzle_)
    {
       
        if (getActorNumber() == player.getActorNumber() && ((puzzle_ is Game)||(puzzle_ is RockPaperGame)))
        {
            addTimeToPlayer(puzzle_.getAmmountOfTime());
        }
    }
    void FinishGameLost(Player player, in IPlayable puzzle_)
    {
        //Debug.LogError(puzzle_.getAmmountOfTime());
        if (getActorNumber() == player.getActorNumber() && ((puzzle_ is Game) || (puzzle_ is RockPaperGame)))
        {
            getTimeForPlayer(puzzle_.getAmmountOfTime());
        }
    }
    void FinishPuzzleWin(Player player,in IPlayable puzzle_)
    {
        if(this == player && puzzle_ is Puzzle puzzle)
        {
            if (puzzle.getPuzzleType() == puzzleType.WinTime)
            {
                addTimeToPlayer(puzzle.getAmmountOfTime());
            }
            else if (puzzle.getPuzzleType() == puzzleType.WasteOthersTime)
            {
                GetTimeForOthers(puzzle.getAmmountOfTime());
            }
            FinishPuzzle(puzzle.id, true);
        }
    }
    void FinishPuzzleLost(Player player, in IPlayable puzzle_)
    {
        if(player == this && puzzle_ is Puzzle puzzle)
        {
            FinishPuzzle(puzzle.id, false);
        }
    }

    public void PlayerLost()
    {
        pv.RPC("PlayerLose", RpcTarget.AllBuffered);
        puzzlesNeedToSolve.Clear();
        GlobalUI.instance.ShowMsgForEveryOne(PhotonNetwork.LocalPlayer.NickName + " have lose the game");
        GameManager.instance.checkForWinner();
    }


    [PunRPC]
    void setPlayerNickName(string nickName_)
    {
        NickName = nickName_;
        nickName.text = nickName_;
        nickName.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }

    [PunRPC]
    void PlayerLose()
    {
        playerLost = true;
    }
    void addTimeToPlayer(float value)
    {
        timer.AddTime(value);
    }
    void GetTimeForOthers(float value) //for the player to get time from other players
    {
        if (isHim())
            pv.RPC("getTimeFromOther", RpcTarget.Others, value);
    }
    void getTimeForPlayer(float value)
    {
        timer.GetTime(value);
    }

    [PunRPC]
    void getTimeFromOther(float value)
    {
        List<Player> players = GameManager.instance.playerList;

        foreach (var player in players)
        {
            //Timer timer = player.GetComponent<Timer>();
            if (player.TryGetComponent(out Timer timer))
            {
                timer.GetTime(value);
                return;
            }
        }
    }
    void FinishPuzzle(int puzzleId,bool didWin) //called when finish puzzle or lost it
    {
        if (!isHim()) return;
        var puzzle = puzzlesNeedToSolve.Find(x => (x.id == puzzleId));
        tasksUIParent.FinishTask(puzzleId, didWin);
        puzzlesNeedToSolve.Remove(puzzle);
        mapManager.FinishTask(puzzleId);
        //ShowPuzzles();//update the UI
    }
    public bool isHim()
    {
        return pv.IsMine;
    }
    public bool canPlayerDoPuzzle(int puzzleId)
    {
        if (!pv.IsMine) return false;
        if (puzzlesNeedToSolve.Find(x => (x.id == puzzleId))) //check if the puzzle exist in the player puzzles
        {
            playerDoingPuzzle = true;
            return true;
        }     
        return false;
    }
    public void closePuzzle(bool close = false)
    {
        playerDoingPuzzle = close;
    }

    void ShowPuzzles() // show or update the UI of puzzle
    {
        tasksUIParent.IntilizeTasks(ref puzzlesNeedToSolve);
        mapManager.gameObject.SetActive(true);
        mapManager.InitializeMap(ref puzzlesNeedToSolve);
        mapManager.gameObject.SetActive(false);
    }



    private void Update()
    {
        if (!pv.IsMine) return;
        Vector3 position = GameManager.instance.GetPositionInMap(transform.position);
        //Debug.Log(position);
        playerInMap.anchoredPosition = position;

        if(Input.GetKeyDown(KeyCode.M))
        {
            mapManager.gameObject.SetActive(!mapManager.gameObject.activeSelf);
        }
        
    }
}
