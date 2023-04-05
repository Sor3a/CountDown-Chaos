using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class Timer : MonoBehaviour
{
    [SerializeField] float timerLength = 120f;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI addedTimeUI;
    Player player;
    PhotonView pv;
    bool lost = false;
    private float timer;

    float r;
    float g;
    private void OnEnable()
    {
        pv = GetComponent<PhotonView>();
        timer = timerLength;
        player = GetComponent<Player>();
        timerText.color = new Color(0, 1, 0);
        r = 0;
        g = 1;
    }
    float alpha;
    // Update is called once per frame
    void setTextColor()
    {
        g = timer / timerLength > 1 ? 1 : timer / timerLength;
        r = 1 - g;
        timerText.color = new Color(r, g, 0);

    }
    public void AddTime(float time) //time should be with seconds
    {
        timer += time;
        alpha = 1;
        addedTimeUI.text = "+" + time.ToString() + "s";
        addedTimeUI.color = new Color(0, 1, 0, alpha);
    }
    public void GetTime(float time)
    {
        //Debug.Log(timer);
        timer -= time;
        alpha = 1;
        addedTimeUI.text = "-" + time.ToString() + "s";
        addedTimeUI.color = new Color(1, 0, 0, alpha);
    }
    void Update()
    {
        if (!pv.IsMine) return;
        if (alpha > 0)
        {
            alpha -= Time.deltaTime * 0.2f;
            var currentColor = addedTimeUI.color;
            addedTimeUI.color = new Color(currentColor.r, currentColor.g, 0, alpha); // b = 0 since we r not using it
        }
        if (lost) return;
        timer -= Time.deltaTime;

        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        setTextColor();


        if (timer <= 0f)
        {
            lost = true;
            player.PlayerLost();
            // Timer has run out
            // Do something here, such as ending the game or triggering an event
        }

    }
}
