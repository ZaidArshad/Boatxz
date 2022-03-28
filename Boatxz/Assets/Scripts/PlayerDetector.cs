using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

/// <summary>
/// Controls actions when a player enters a checkpoint
/// To be applied on a checkpoint's hitbox
/// </summary>
public class PlayerDetector : MonoBehaviour {
    [SerializeField] GameObject topBar;
    [SerializeField] Material redMaterial;
    [SerializeField] Material greenMaterial;
    public PlayerDetector nextCheckpoint;

    private bool isPassed = false;
    private bool canPass = false;
    private Stopwatch timer { set; get; }

    void OnTriggerEnter(Collider collider) {
        if (collider.tag == "Player") {
            if (MultiplayerManager.Instance.gameMode == GameMode.Lobby) {
                loadNewScene();
            }
            else if (MultiplayerManager.Instance.gameMode == GameMode.Speedrun)  {
                speedRun(collider);
            }
            else if (MultiplayerManager.Instance.gameMode == GameMode.MultiplayerRace)  {
                race(collider);
            }
            else if (MultiplayerManager.Instance.gameMode == GameMode.Level)  {
                goToNextLevel(collider);
            }
        }
    }

    public void setNextCheckpoint(PlayerDetector checkpoint) {
        nextCheckpoint = checkpoint;
    }

    public void setCanPass(bool pass) {
        canPass = pass;
        topBar.GetComponent<Renderer>().material = greenMaterial;
    }

    public void setTimer(Stopwatch stopWatch) {
        timer = stopWatch;
    }

    private void loadNewScene() {
        GameMode mode = gameObject.GetComponentInParent<CheckpointAttributes>().gameMode;
        if (mode == GameMode.Speedrun) {
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
        else if (mode == GameMode.MultiplayerRace) {
            UnityEngine.SceneManagement.SceneManager.LoadScene(2);
        }
        else if (mode == GameMode.BoatHunt) {
            UnityEngine.SceneManagement.SceneManager.LoadScene(3);
        }
        else if (mode == GameMode.MultiplayerBattle) {
            UnityEngine.SceneManagement.SceneManager.LoadScene(4);
        }
    }

    private void speedRun(Collider collider) {
        if (!isPassed && canPass) {
            if (!timer.IsRunning) timer.Start();
            isPassed = true;
            topBar.GetComponent<Renderer>().material = redMaterial;
            collider.transform.parent.GetComponent<HullAttributes>().lastCheckpoint = this;
            if (nextCheckpoint != null) {
                nextCheckpoint.setCanPass(true);
            }
            else {
                collider.transform.parent.GetComponent<HullAttributes>().showPrompt("Press B to Return to Lobby");
                MultiplayerManager.Instance.finishGame();
                timer.Stop();
            }
        }
    }

    private int place = 1;
    private void race(Collider collider) {
        PlayerDetector lastCheckpoint = collider.transform.parent.GetComponent<HullAttributes>().lastCheckpoint;
        if (lastCheckpoint == null || lastCheckpoint.nextCheckpoint == this) {
            collider.transform.parent.GetComponent<HullAttributes>().lastCheckpoint = this;
            if (nextCheckpoint == null) {
                collider.transform.parent.GetComponent<HullAttributes>().showPrompt("Position: "+ place);
                place++;
                MultiplayerManager.Instance.finishGame();
            }
        }
    }

    private void goToNextLevel(Collider collider) {
        int buildIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        UnityEngine.Debug.Log(buildIndex + " " + UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings);
        
        if (UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings > buildIndex+1) {
            UnityEngine.SceneManagement.SceneManager.LoadScene(buildIndex+1);
        }
        else {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }
}
