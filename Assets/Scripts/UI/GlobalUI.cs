using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class GlobalUI : MonoBehaviour
{
    public static GlobalUI instance;
    PhotonView pv;
    [SerializeField] TextMeshProUGUI Announce;
    void Awake()
    {
        if(instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        pv = GetComponent<PhotonView>();
    }
    public void ShowMsgForEveryOne(string msg)
    {
        pv.RPC("showText", RpcTarget.All, msg);
    }
    public void ClearMsgForEveryOne()
    {
        pv.RPC("clearText", RpcTarget.All);
    }

    IEnumerator disableMSG()
    {
        yield return new WaitForSeconds(7f);
        Announce.text = "";
    }
    [PunRPC]
    void showText(string msg)
    {
        StopAllCoroutines();
        Announce.text = msg;
        StartCoroutine(disableMSG());
    }
    [PunRPC]
    void clearText()
    {
        Announce.text = "";
    }
}

