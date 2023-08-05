using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TaskUI : MonoBehaviour
{
    bool isFinished;
    [SerializeField] TextMeshProUGUI nameOfTask;
    [SerializeField] Image state,TaskType; //finished 
    [SerializeField] Sprite finished, notFinished,steal,win;

    int taskId;
    private void OnEnable()
    {
        isFinished = false;
    }
    public int getTaskIdHolded()
    {
        return taskId;
    }
    public void InitializeUI(int id, string taskeName, puzzleType type,bool Finished = false)
    {
        isFinished = Finished;
        taskId = id;
        nameOfTask.text = taskeName;
        nameOfTask.color = Color.white;
        var sprite = type == puzzleType.WinTime ? win : steal;
        TaskType.sprite = sprite;
        if (!Finished)
            state.sprite = notFinished;
    }
    public void FinishTask(bool win)
    {
        if(!isFinished)
        {
            isFinished = true;
            nameOfTask.fontStyle = FontStyles.Strikethrough;
            if (win)
            {
                state.sprite = finished;
                nameOfTask.color = new Color(0.5f, 1f, 0.5f);
            }
            else
                nameOfTask.color = new Color(1.0f, 0.5f, 0.5f);
                
        }

    }
}
