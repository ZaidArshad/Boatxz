using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public enum GameMode {Lobby, Speedrun, MultiplayerRace, MultiplayerBattle, BoatHunt};
public class MultiplayerManager : MonoBehaviour {
    [SerializeField] GameObject startingCam;
    [SerializeField] Transform[] startingSpot = new Transform[4];
    [SerializeField] Material[] materials = new Material[4];
    [SerializeField] GameObject screenCanvas;

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
        }
    }

    private void Start() {
        if (gameMode == GameMode.Lobby) startGame();
    }

    public Transform getStartingPosition(int playerNumber) {
        return startingSpot[playerNumber];
    }

    public void startGame() {
        if (gameMode != GameMode.Speedrun && gameMode != GameMode.Lobby) {
            for (int i = 0; i < 2; i++) {
                if (joinedPlayers[i] == null) return;
            }
        }
        if (startingCam != null) {
            displayP4Block();
            Destroy(startingCam);
        }
        gameStarted = true;
    }

    public int join(GameObject player) {
        if (isSinglePlayer()) return 0;
        if (!gameStarted) {
            for (int i = 0; i < joinedPlayers.Length; i++) {
                if (joinedPlayers[i] == player) return i;
                if (joinedPlayers[i] == null) {
                    numOfPlayers++;
                    joinedPlayers[i] = player;
                    player.GetComponent<Renderer>().material = materials[i];
                    if (gameMode == GameMode.BoatHunt) {
                        if (i == 0) {
                            player.GetComponent<HullAttributes>().becomeHunter();
                        }
                        else {
                            player.GetComponent<HullAttributes>().becomeHunted();
                        }
                    }
                    
                    return i;
                }
            }
        }
        return -1;
    }

    private GameObject getRemainingPlayer() {
        for (int i = 0; i < joinedPlayers.Length; i++) {
            if (joinedPlayers[i] != null) {
                return joinedPlayers[i];
            }
        }
        return null;
    }

    public void finishGame() {
        gameFinished = true;
    }

    private void displayP4Block() {
        if (screenCanvas != null) {
            if (numOfPlayers == 3) screenCanvas.transform.GetChild(0).GetComponent<RawImage>().color = new Color(0, 0, 0, 255);
            else screenCanvas.transform.GetChild(0).GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
        }
    }

    public bool isGameFinished() {
        return gameFinished;
    }

    public void leave(int playerNumber) {
        Destroy(joinedPlayers[playerNumber]);
        if (joinedPlayers[playerNumber] != null) {
            numOfPlayers--;
            joinedPlayers[playerNumber] = null;
            if (numOfPlayers < 2 && isGameStarted()) {
                if (gameMode == GameMode.MultiplayerBattle) getRemainingPlayer().GetComponent<HullAttributes>().showPrompt("Winner");
                finishGame();
            }
            displayP4Block();
        }
    }


    public void singlePlayer() {
        if (gameMode == GameMode.Speedrun || gameMode == GameMode.Lobby) startGame();
    }

    public bool isSinglePlayer() {
        return (gameMode == GameMode.Speedrun || gameMode == GameMode.Lobby);
    }

    public bool isRaceMode() {
        return (gameMode == GameMode.Speedrun || gameMode == GameMode.MultiplayerRace);
    }

    public bool isFightingMode() {
        return (gameMode == GameMode.MultiplayerBattle || gameMode == GameMode.BoatHunt);
    }

    public bool isGameStarted() {
        return gameStarted;
    }
}
