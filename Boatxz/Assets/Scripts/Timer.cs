using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the timer displayed on screen
/// To be applied to a text object
/// </summary>
public class Timer : MonoBehaviour
{
    private Text text;
    private float time;
    private bool isStarted = false;

    void Start() {
        text = gameObject.GetComponent<Text>();
        time = 300;
    }

    // Update is called once per frame
    void Update() {
        if (MultiplayerManager.Instance.isGameStarted()) {
            if (!isStarted){
                isStarted = true;
                text.color = new Color(255,255,255,255);
            }
            int mins = (int) (time / 60);
            int sec = ((int) time % 60);
            string strTime;
            if (sec < 10) strTime = mins + ":0" + sec;
            else strTime = mins + ":" + sec;
             
            if (!MultiplayerManager.Instance.isGameFinished()) {
                if (time > 0) {
                    time -= Time.deltaTime;
                    text.text = strTime;
                }
                else {
                    text.text = "Prey win";
                    MultiplayerManager.Instance.finishGame();
                }
            }
            else {
                text.text = "Hunter Wins at: " + strTime;
            }
        }
    }
}
