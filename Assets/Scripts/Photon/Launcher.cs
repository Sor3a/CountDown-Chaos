using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class Launcher : MonoBehaviourPunCallbacks
{
    MenuManager menu;
    [SerializeField] TMP_InputField roomName,joinRoomName,nickName;
    [SerializeField] TextMeshProUGUI roomNameTxt;
    [SerializeField] TextMeshProUGUI debugger;
    //[SerializeField] GameObject problem;
    private void Awake()
    {
        menu = GetComponent<MenuManager>();
    }
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
        
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("lobby joined");
        menu.OpenMenu(MenuTypes.nickNameRoom);
    }
    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomName.text)) return;
        PhotonNetwork.CreateRoom(roomName.text,new Photon.Realtime.RoomOptions { MaxPlayers=3 });
        menu.OpenMenu(MenuTypes.Loading);
    }
    public void JoinRoom()
    {
        if (string.IsNullOrEmpty(joinRoomName.text)) return;
        PhotonNetwork.JoinRoom(joinRoomName.text);
        
    }
    
    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.LoadLevel(1);
        else
        {
            debugger.text = "only owner can start the game";
        }
    }
    public void ConfirmNickName()
    {
        if (string.IsNullOrEmpty(nickName.text))
        {
            debugger.text = "Enter NickName to pass";
            return;
        }
        PhotonNetwork.NickName = nickName.text;
        menu.OpenMenu(MenuTypes.General);
    }
    public override void OnJoinedRoom()
    {
       // bool showButtom = PhotonNetwork.CurrentRoom.PlayerCount == 1;
        menu.OpenMenu(MenuTypes.JoinedRoom);
        roomNameTxt.text += " " + PhotonNetwork.CurrentRoom.Name;
    }
}
