using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Holder : MonoBehaviour
{
    TextMeshProUGUI block;
    Image image;
    char key;
    Color[] colors;
    private void Awake()
    {
        colors = new Color[]
            {
            new Color(0.75f, 0.95f, 0.82f), // Teal
            new Color(0.95f, 0.68f, 0.77f), // Coral
            new Color(0.78f, 0.77f, 0.95f) // Lavender
            };
        image = GetComponent<Image>();
        block = GetComponentInChildren<TextMeshProUGUI>();
    }
    public void SetKey(char c)
    {
        key = c;
        block.text = c.ToString();
        image.color = colors[(int)(c - 'A')]; //since 'A'-'A' = 0 ,'B'-'A' = 1 ....
    }
    public char getKey()
    {
        return key;
    }
}
