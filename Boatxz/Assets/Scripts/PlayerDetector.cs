using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour {
    [SerializeField] GameObject topBar;
    [SerializeField] Material redMaterial;
    [SerializeField] Material greenMaterial;
    private Transform nextCheckpoint;

    private bool isPassed = false;
    private bool canPass = false;

    // Update is called once per frame
    private void OnTriggerEnter(Collider collider) {
        if (collider.tag == "Player") {
            if (MultiplayerManager.Instance.gameMode == GameMode.Lobby) {
                loadNewScene();
            }
            else if (MultiplayerManager.Instance.gameMode == GameMode.Speedrun)  {
                if (!isPassed && canPass) {
                    isPassed = true;
                    topBar.GetComponent<Renderer>().material = redMaterial;
                    collider.transform.parent.GetComponent<HullAttributes>().setLastCheckpoint(transform.gameObject);
                    if (nextCheckpoint != null) {
                        nextCheckpoint.GetChild(3).gameObject.GetComponent<PlayerDetector>().setCanPass(true);
                    }
                    else {
                        Debug.Log("Finished");
                    }
                }
            }
        }
    }

    public void setNextCheckpoint(Transform checkpoint) {
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
}
