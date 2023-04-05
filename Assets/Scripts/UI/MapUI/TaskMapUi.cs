using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskMapUi : MonoBehaviour
{
    [SerializeField] Image image;
    RectTransform rectTransform;

    private void OnEnable()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public int TaskId { private set; get; }

    public void Initialize(Sprite sprite,int id,Vector3 position)
    {
        image.sprite = sprite;
        TaskId = id;
        rectTransform.anchoredPosition = position;
    }
    public void FinishTask()
    {
        gameObject.SetActive(false);
    }

}
