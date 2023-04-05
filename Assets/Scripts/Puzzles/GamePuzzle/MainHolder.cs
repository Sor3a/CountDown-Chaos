using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainHolder : MonoBehaviour
{
    List<Holder> holders;
    private void Awake()
    {
        holders = new List<Holder>();
        foreach (Transform item in transform)
        {
            holders.Add(item.GetComponent<Holder>());
        }
    }
    public void InitializeGame(char[,] game)
    {
        int k = 0;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                holders[k++].SetKey(game[i, j]);
            }
        }
    }
    public char[,] getAnswer()
    {
        char[,] answer = new char[3, 3];
        int k = 0;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                answer[i,j]= holders[k++].getKey();
            }
        }
        return answer;
    }
}
