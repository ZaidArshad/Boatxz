using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerManager : MonoBehaviour {
    [SerializeField] private Transform startingSpot;
    [SerializeField] Camera startingCam;
    
    public static MultiplayerManager Instance;
    GameObject[] joinedPlayers = new GameObject[2];
    bool gameStarted = false;

    private const int STARTING_OFFSET = -20;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }

    public Vector3 getStartingPosition(int playerNumber) {
        Vector3 position = startingSpot.position;
        position.z += playerNumber*STARTING_OFFSET;
        return position;
    }

    public Quaternion getStartingRotation() {
        return startingSpot.rotation;
    }

    public void startGame() {
        for (int i = 0; i < joinedPlayers.Length; i++) {
            if (joinedPlayers[i] == null) return;
        }
        Destroy(startingCam);
        gameStarted = true;
    }

    public int join(GameObject player) {
        if (!gameStarted) {
            for (int i = 0; i < joinedPlayers.Length; i++) {
                if (joinedPlayers[i] == player) return i;
                if (joinedPlayers[i] == null) {
                    joinedPlayers[i] = player;
                    return i;
                }
            }
        }
        return -1;
    }

    public void leave(int playerNumber) {
        if (!gameStarted) {
            Destroy(joinedPlayers[playerNumber]);
            joinedPlayers[playerNumber] = null;
        }
    }
}
