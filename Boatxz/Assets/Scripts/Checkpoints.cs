using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEngine.UI;

public class Checkpoints : MonoBehaviour {
    [SerializeField] Text timeText; 
    PlayerDetector childDetector;
    Stopwatch timer = new Stopwatch();

    private void Awake() {
        if (MultiplayerManager.Instance.gameMode == GameMode.Speedrun) childDetector.timer = timer;
    }

    private void Start() {
        setUpCheckpoints();
    }

    private void setUpCheckpoints() {
        for (int i = 0; i < transform.childCount; i++) {
            childDetector = transform.GetChild(i).GetChild(3).GetComponent<PlayerDetector>();
            if (i == 0) childDetector.setCanPass(true);
            if (i < transform.childCount-1) childDetector.setNextCheckpoint(transform.GetChild(i+1).GetChild(3).GetComponent<PlayerDetector>());
        }
    }

    private void Update() {
        if (MultiplayerManager.Instance.gameMode == GameMode.Speedrun) timeText.text = timer.Elapsed.ToString("g");
    }
}
