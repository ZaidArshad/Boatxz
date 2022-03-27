using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEngine.UI;

/// <summary>
/// Parents class to a collection of checkpoints
/// Sets up the linked list of checkpoints
/// </summary>
public class Checkpoints : MonoBehaviour {
    [SerializeField] Text timeText; 
    private Stopwatch timer = new Stopwatch();

    void Start() {
        setUpCheckpoints();
    }

    void Update() {
        if (MultiplayerManager.Instance.gameMode == GameMode.Speedrun) timeText.text = timer.Elapsed.ToString("g");
    }

    private void setUpCheckpoints() {
        PlayerDetector childDetector;
        for (int i = 0; i < transform.childCount; i++) {
            childDetector = transform.GetChild(i).GetChild(3).GetComponent<PlayerDetector>();
            if (MultiplayerManager.Instance.gameMode == GameMode.Speedrun) childDetector.setTimer(timer);
            if (i == 0) childDetector.setCanPass(true);

            // Linking the nodes
            if (i < transform.childCount-1) childDetector.setNextCheckpoint(transform.GetChild(i+1).GetChild(3).GetComponent<PlayerDetector>());
        }
    }
}
