using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum GameMode {Lobby, Speedrun, MultiplayerRace, MultiplayerBattle, BoatHunt};
public class MultiplayerManager : MonoBehaviour {
    [SerializeField] Camera startingCam;
    [SerializeField] Transform[] startingSpot = new Transform[4];
    [SerializeField] Material[] materials = new Material[4];

    public GameMode gameMode;
    public int numOfPlayers = 0;
    
    public static MultiplayerManager Instance;
    GameObject[] joinedPlayers = new GameObject[4];
    private bool gameStarted = false;
    private bool gameFinished = false;
    private const int STARTING_OFFSET = -20;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            if (gameMode == GameMode.Lobby) startGame();
        }
    }

    public Transform getStartingPosition(int playerNumber) {
        return startingSpot[playerNumber];
    }

    public void startGame() {
        if (gameMode != GameMode.Speedrun) {
            for (int i = 0; i < 2; i++) {
                if (joinedPlayers[i] == null) return;
            }
        }
        if (startingCam != null) Destroy(startingCam);
        gameStarted = true;
    }

    public int join(GameObject player) {
        if (!gameStarted) {
            for (int i = 0; i < joinedPlayers.Length; i++) {
                if (joinedPlayers[i] == player) return i;
                if (joinedPlayers[i] == null) {
                    numOfPlayers++;
                    joinedPlayers[i] = player;
                    player.GetComponent<Renderer>().material = materials[i];
                    if (i == 0 && gameMode == GameMode.BoatHunt) {
                        player.GetComponent<HullAttributes>().becomeHunter();
                    }
                    return i;
                }
            }
        }
        return -1;
    }

    public void finishGame() {
        Debug.Log("game finished");
        gameFinished = true;
    }

    public bool isGameFinished() {
        return gameFinished;
    }

    public void leave(int playerNumber) {
        Destroy(joinedPlayers[playerNumber]);
        joinedPlayers[playerNumber] = null;
        numOfPlayers--;
        if (numOfPlayers < 2 && isGameStarted()) finishGame();
    }


    public void singlePlayer() {
        if (gameMode == GameMode.Speedrun || gameMode == GameMode.Lobby) startGame();
    }

    public bool isSinglePlayer() {
        return (gameMode == GameMode.Speedrun || gameMode == GameMode.Lobby);
    }

    public bool isGameStarted() {
        return gameStarted;
    }
}
