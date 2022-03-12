using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {
    PlayerDetector childDetector;
    private void Start() {
        setUpCheckpoints();
    }

    private void setUpCheckpoints() {
        for (int i = 0; i < transform.childCount; i++) {
            childDetector = transform.GetChild(i).GetChild(3).GetComponent<PlayerDetector>();
            if (i == 0) childDetector.setCanPass(true);
            if (i < transform.childCount-1) childDetector.setCheckpoint(transform.GetChild(i+1));
        }
    }
}
