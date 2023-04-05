using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerHolder : MonoBehaviour
{
    [SerializeField] float startValue;
    [SerializeField] float endValue;
    [SerializeField] float speed;
    Scrollbar bar;
    float cursorPosition;
    int direction = 1;
    bool moving;
    private void Awake()
    {
        bar = GetComponent<Scrollbar>();
        cursorPosition = bar.value;
        direction = 1;
        moving = true;
    }

    private void Update()
    {
        if (!moving) return;
        cursorPosition += speed * direction * Time.deltaTime;
        if(cursorPosition>0.95f)
        {
            direction = -1;
            cursorPosition = 0.95f;
        }
        else if(cursorPosition<0.05f)
        {
            direction = 1;
            cursorPosition = 0.05f;
        }
        bar.value = cursorPosition;
    }

    public bool DidCatchIt()
    {
        if (bar.value < endValue && bar.value > startValue)
        {
            moving = false;
            return true;
        }
        return false;
    }
}
