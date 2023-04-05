using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TasksUIMain : MonoBehaviour
{
    List<TaskUI> tasks;
    Color[] colors;//these are the colors each task have it's own color ==> based on the type we gonna choose the color
    [SerializeField] GameObject taskUI;
    private void Awake()
    {
        tasks = new List<TaskUI>();
        if (colors == null)
        {
            colors = new Color[]
            {
            new Color(0.5f, 1f, 0.5f),
            new Color(1.0f, 0.5f, 0.5f),
            new Color(1f, 1f, 0.5f),
            };
        }
    }
   // new Color(0.0f, 0.53f, 0.89f),
            //new Color(1.0f, 0.3f, 0.64f),
           // new Color(0.41f, 0.97f, 0.15f),
    public void FinishTask(int id,bool didWin)
    {
        var taskFinished = tasks.Find(x => x.getTaskIdHolded() == id);
        taskFinished.FinishTask(didWin);
    }
    public void IntilizeTasks(ref List<Puzzle> puzzles)
    {
        foreach (var puzzle in puzzles)
        {
            var taskUI = Instantiate(this.taskUI, transform).GetComponent<TaskUI>();
            tasks.Add(taskUI);

            //taskUI.InitializeUI(puzzle.id, puzzle.getName(), colors[(int)puzzle.getPuzzleType()]);
            taskUI.InitializeUI(puzzle.id, puzzle.getName(), Color.white);
        }
    }
}
