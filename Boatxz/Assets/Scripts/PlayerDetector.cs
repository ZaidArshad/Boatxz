using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class PlayerDetector : MonoBehaviour {
    [SerializeField] GameObject topBar;
    [SerializeField] Material redMaterial;
    [SerializeField] Material greenMaterial;
    public PlayerDetector nextCheckpoint;

    private bool isPassed = false;
    private bool canPass = false;
    public Stopwatch timer;

    // Update is called once per frame
    private void OnTriggerEnter(Collider collider) {
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
        }
    }

    public void setNextCheckpoint(PlayerDetector checkpoint) {
        nextCheckpoint = checkpoint;
    }

    public void setCanPass(bool pass) {
        canPass = pass;
        topBar.GetComponent<Renderer>().material = greenMaterial;
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
            collider.transform.parent.GetComponent<HullAttributes>().setLastCheckpoint(this);
            if (nextCheckpoint != null) {
                nextCheckpoint.setCanPass(true);
            }
            else {
                timer.Stop();
            }
        }
    }

    private void race(Collider collider) {
        PlayerDetector lastCheckpoint = collider.transform.parent.GetComponent<HullAttributes>().getLastCheckpoint();
        if (lastCheckpoint == null || lastCheckpoint.nextCheckpoint == this) {
            collider.transform.parent.GetComponent<HullAttributes>().setLastCheckpoint(this);
            if (nextCheckpoint == null) {
                UnityEngine.Debug.Log("finish");
            }
        }
    }
}
