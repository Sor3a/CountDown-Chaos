using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerManager : MonoBehaviour
{
    PhotonView pv;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }
    private void Start()
    {
        if (pv.IsMine)
            CreatePlayerControllor();
    }
    void CreatePlayerControllor()
    {
        PhotonNetwork.Instantiate("Player", GameManager.instance.GetPlayerSpawnPosition(), Quaternion.identity);
    }
}
