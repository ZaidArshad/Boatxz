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
            if (!isPassed && canPass) {
                isPassed = true;
                topBar.GetComponent<Renderer>().material = redMaterial;
                if (nextCheckpoint != null) {
                    nextCheckpoint.GetChild(3).gameObject.GetComponent<PlayerDetector>().setCanPass(true);
                }
                else {
                    Debug.Log("Finished");
                }
            }
        }
    }

    public void setCheckpoint(Transform checkpoint) {
        nextCheckpoint = checkpoint;
    }

    public void setCanPass(bool pass) {
        canPass = pass;
        topBar.GetComponent<Renderer>().material = greenMaterial;
    }
}
