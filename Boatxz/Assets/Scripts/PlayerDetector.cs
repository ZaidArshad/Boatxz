using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour {
    [SerializeField] GameObject topBar;
    [SerializeField] Material redMaterial;
    [SerializeField] Material greenMaterial;
    [SerializeField] GameObject nextCheckpoint;

    private bool isPassed = false; 

    // Update is called once per frame
    private void OnTriggerEnter(Collider collider) {
        if (collider.tag == "Player") {
            if (!isPassed) {
                isPassed = true;
                topBar.GetComponent<Renderer>().material = redMaterial;
                if (nextCheckpoint != null) nextCheckpoint.transform.GetChild(2).gameObject.GetComponent<Renderer>().material = greenMaterial;
                Debug.Log("passed");
            }
        }
    }
}
