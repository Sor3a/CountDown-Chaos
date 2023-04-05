using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapUIManager : MonoBehaviour
{
    [SerializeField] TaskMapUi taskUI;
    List<TaskMapUi> tasks;
    public void InitializeMap(ref List<Puzzle> puzzles)
    {
        tasks = new List<TaskMapUi>();
        foreach (var item in puzzles)
        {
            var task = Instantiate(taskUI, transform);
            var position = GameManager.instance.GetPositionInMap(item.transform.position);
            task.Initialize(item.puzzleSprite, item.id, position);
            tasks.Add(task);
        }

        List<IGame> games = FindObjectsOfType<Game>().ToList<IGame>();
        games.Add(FindObjectOfType<RockPaperGame>());
        foreach (var game in games)
        {
            if(game is MonoBehaviour obj)
            {
                var task = Instantiate(taskUI, transform);
                var position = GameManager.instance.GetPositionInMap(obj.transform.position);
                task.Initialize(game.getSprite(), 0, position);
            }
        }
    }

    public void FinishTask(int taskID)
    {
       var finishedTask= tasks.Find(x => x.TaskId == taskID);
        finishedTask.FinishTask();
    }
}
