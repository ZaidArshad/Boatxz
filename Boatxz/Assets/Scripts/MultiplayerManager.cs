using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum GameMode {Lobby, Speedrun, MultiplayerRace, MultiplayerBattle, BoatHunt};
public class MultiplayerManager : MonoBehaviour {
    [SerializeField] Camera startingCam;
    [SerializeField] Transform[] startingSpot = new Transform[4];
    public GameMode gameMode;
    
    public static MultiplayerManager Instance;
    GameObject[] joinedPlayers = new GameObject[4];
    bool gameStarted = false;
    private const int STARTING_OFFSET = -20;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }

    public Transform getStartingPosition(int playerNumber) {
        return startingSpot[playerNumber];
    }

    public void startGame() {
        for (int i = 0; i < 2; i++) {
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
