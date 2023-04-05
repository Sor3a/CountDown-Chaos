using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public List<Player> playerList { private set; get; }
    [SerializeField] Transform MapOrigin;
    Vector3 mapPosition;

    /// </real life to map unit transformation>
    /// 50m RL ---> 1000m map
    /// 1m RL ----> 20m map
    /// </summary>
    public Vector3 GetPositionInMap(Vector3 currentPos) // it changes from the z to the y since we are going from 3d to 2d
    {
        return (new Vector3(currentPos.x - mapPosition.x, currentPos.z - mapPosition.z, 0)) * 20f;
    }

    public void addPlayer(Player player)
    {
        playerList.Add(player);
    }
    public void RemovePlayer(Player player)
    {
        playerList.Remove(player);
    }
    public void checkForWinner()
    {
        int n = 0;
        Player winner = null;
        foreach (var player in playerList)
        {
            if (player.playerLost == false)
            {
                n++;
                winner = player;
            }
        }
        if(n==1)
        {
            if (winner)
            {
                GlobalUI.instance.ShowMsgForEveryOne(winner.NickName + " have won the game");

                //Do somthing like restart or idk
            }
                
        }
    }
    private void Awake()
    {
        if(instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        playerList = new List<Player>();
        mapPosition = MapOrigin.position;
    }
}
