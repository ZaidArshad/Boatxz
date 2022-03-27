using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

/// <summary>
/// Controls the different player attributes and game modes
/// </summary>
public enum GameMode {Lobby, Speedrun, MultiplayerRace, MultiplayerBattle, BoatHunt};
public class MultiplayerManager : MonoBehaviour {
    [SerializeField] GameObject startingCam;
    [SerializeField] Transform[] startingSpot = new Transform[4];
    [SerializeField] Material[] materials = new Material[4];
    [SerializeField] GameObject screenCanvas;

    public GameMode gameMode;
    public int numOfPlayers = 0;
    public static MultiplayerManager Instance;
    private GameObject[] joinedPlayers = new GameObject[4];

    private bool gameStarted = false;
    private bool gameFinished = false;
    private const int STARTING_OFFSET = -20;
    private PlayerInputManager playerInputManager;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }

    void Start() {
        if (gameMode == GameMode.Lobby) startGame();
        playerInputManager = gameObject.GetComponent<PlayerInputManager>();
    }

    public Transform getStartingPosition(int playerNumber) {
        return startingSpot[playerNumber];
    }

    public void startGame() {
        if (!isSinglePlayer()) {
            for (int i = 0; i < 2; i++) {
                if (joinedPlayers[i] == null) return;
            }
        }
        if (startingCam != null) {
            displayP4Block();
            Destroy(startingCam);
        }
        if (playerInputManager != null) playerInputManager.DisableJoining();
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

    public void finishGame() {
        gameFinished = true;
    }

    public bool isGameFinished() {
        return gameFinished;
    }

    public void leave(int playerNumber) {
        if (joinedPlayers[playerNumber] != null && !gameFinished) {
            Destroy(joinedPlayers[playerNumber]);
            numOfPlayers--;
            joinedPlayers[playerNumber] = null;
            if (numOfPlayers < 2 && isGameStarted()) {
                if (gameMode == GameMode.MultiplayerBattle) getRemainingPlayer().GetComponent<HullAttributes>().showPrompt("Winner");
                finishGame();
            }
            refreshSplitScreen();
        }
    }

    public void singlePlayer() {
        if (isSinglePlayer()) startGame();
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

    private GameObject getRemainingPlayer() {
        for (int i = 0; i < joinedPlayers.Length; i++) {
            if (joinedPlayers[i] != null) {
                return joinedPlayers[i];
            }
        }
        return null;
    }

    private void displayP4Block() {
        if (screenCanvas != null) {
            if (numOfPlayers == 3) screenCanvas.transform.GetChild(0).GetComponent<RawImage>().color = new Color(0, 0, 0, 255);
            else screenCanvas.transform.GetChild(0).GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
        }
    }

    private GameObject[] getAlivePlayers() {
        GameObject[] alive = new GameObject[numOfPlayers];
        int index = 0;
        for (int i = 0; i < 4; i++) {
            if (joinedPlayers[i] != null) {
                alive[index] = joinedPlayers[i];
                index++;
            }
        }
        return alive;
    }

    /*
    joinedPlayers[playerNumber].transform.GetChild(0).GetChild(0).GetComponent<Camera>().rect = new Rect (0, 0, 1, 1);
    1 Player view
    1: Rect(0, 0, 1, 1)

    2 Player views
    1: Rect(0, 0, 0.5, 1)
    2: Rect(0.5, 0, 0.5, 1)
    
    4 Player views
    1: Rect(0, 0.5, 0.5, 0.5)
    2: Rect(0.5, 0.5, 0.5, 0.5)
    3: Rect(0, 0, 0.5, 0.5)
    4: Rect(0.5, 0, 0.5, 0.5)
    */
    private void refreshSplitScreen() {
        GameObject[] alive = getAlivePlayers();
        Vector2[] blockPivots = {new Vector2(1,0), new Vector2(0,0), new Vector2(1,1), new Vector2(0,1)};
        if (numOfPlayers == 1) {
            alive[0].transform.GetChild(0).GetChild(0).GetComponent<Camera>().rect = new Rect (0, 0, 1, 1);
        }
        if (numOfPlayers == 2) {
            alive[0].transform.GetChild(0).GetChild(0).GetComponent<Camera>().rect = new Rect(0, 0, 0.5f, 1);
            alive[1].transform.GetChild(0).GetChild(0).GetComponent<Camera>().rect = new Rect(0.5f, 0, 0.5f, 1);
        }
        else if (numOfPlayers == 3) {
            for (int i = 0; i < 4; i++) {
                if (joinedPlayers[i] == null) {
                    screenCanvas.transform.GetChild(0).GetComponent<RectTransform>().pivot = blockPivots[i];
                }
            }
        }
        displayP4Block();
    }

}
