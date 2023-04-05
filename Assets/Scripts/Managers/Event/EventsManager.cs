using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
public class EventsManager : MonoBehaviour
{
    [SerializeField] List<EventShowUI> announcers;
    //[SerializeField] TextMeshProUGUI announceText;
    PhotonView pv;
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        

    }
    void Start()
    {
        Puzzle.winPuzzle += PlayerWinPuzzle;
        Puzzle.lostPuzzle += PlayerLostPuzzle;

        Game.winGame += PlayerWinPuzzle;
        Game.lostGame += PlayerLostPuzzle;

        RockPaperGame.winGame += PlayerWinPuzzle;
        RockPaperGame.lostGame += PlayerLostPuzzle;
    }
    private void OnDestroy()
    {
        Puzzle.winPuzzle -= PlayerWinPuzzle;
        Puzzle.lostPuzzle -= PlayerLostPuzzle;

        Game.winGame -= PlayerWinPuzzle;
        Game.lostGame -= PlayerLostPuzzle;

        RockPaperGame.winGame -= PlayerWinPuzzle;
        RockPaperGame.lostGame -= PlayerLostPuzzle;
    }

    [PunRPC]
    void Annouce(string text,bool win)
    {
        int index = announcers.Count-2;
        var currentColor = win ? new Color(0.1f, 1, 0.1f) : new Color(1f, 0.1f, 0.1f);
        while (index >= 0)
        {
            var announcer = announcers[index];
            if (!announcers[index].isDisalbe())
                announcers[index + 1].ChangeText(announcer.getText(), announcer.currentColor);

            index--;
        }
        announcers[0].ChangeText(text, currentColor);


        //currentAnnouncer.text = text;
        //if(win)
        //{
        //    currentAnnouncer.color = new Color(0.1f, 1, 0.1f);
        //}
        //else
        //    currentAnnouncer.color = new Color(1f, 0.1f, 0.1f);
        // StartCoroutine(DisableText(currentAnnouncer));
    }
    void PlayerWinPuzzle(Player player,in IPlayable puzzle)
    {
        string typeOfAction = "";
        if (puzzle is Game)
            typeOfAction = " has won ";
        else
            typeOfAction = " has finished ";
        pv.RPC("Annouce", RpcTarget.All, player.NickName + typeOfAction + puzzle.getName(),true);
    }
    void PlayerLostPuzzle(Player player,in IPlayable puzzle)
    {
        pv.RPC("Annouce", RpcTarget.All, player.NickName + " has lost " + puzzle.getName(), false);
    }

  
}
