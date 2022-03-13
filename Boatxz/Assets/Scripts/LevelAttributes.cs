using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelAttributes : MonoBehaviour {
    [SerializeField] private Transform startingPosition;
    [SerializeField] Camera startingCam;
    public static LevelAttributes Instance;
    int numOfPlayers = 0;

    private void Awake() {
        if (Instance == null) Instance = this;
    }

    public Transform getStartingPosition() {
        return startingPosition;
    }

    public void newPlayerJoined() {
        numOfPlayers++;
        if (numOfPlayers > 1) {
            if (startingCam != null) Destroy(startingCam);
            startingPosition.Translate(0,0,-20);
        }
    }
}
