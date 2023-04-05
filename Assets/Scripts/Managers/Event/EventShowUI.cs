using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EventShowUI : MonoBehaviour
{
    TextMeshProUGUI text;
    float alpha;
    public Color currentColor { private set; get; }
    [SerializeField] float disablingSpeed;
    bool startDisabling;
    public bool isDisalbe()
    {
        if (text.text == "")
            return true;
        if (alpha <= 0.01f)
            return true;
        return false;
    }
    public string getText()
    {
        return text.text;
    }
    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        alpha = 1;
    }
    IEnumerator disableText()
    {
        yield return new WaitForSeconds(disablingSpeed);
        startDisabling = true;
    }
    public void ChangeText(string newText, Color color)
    {
        text.text = newText;
        text.color = color;
        currentColor = color;
        alpha = 1;
        startDisabling = false;
        StopAllCoroutines();
        StartCoroutine(disableText());
    }
    private void Update()
    {
        if (startDisabling && alpha>0)
        {
            
            text.color = new Color(currentColor.r, currentColor.g, currentColor.b,alpha);
            alpha -= Time.deltaTime * 0.5f;
        }
    }
}
