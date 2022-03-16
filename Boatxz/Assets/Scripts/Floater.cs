using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour {
    // From https://www.youtube.com/watch?v=eL_zHQEju8s&ab_channel=TomWeiland
    public Rigidbody hull;
    public float depthBeforeSubmerged = 1f;
    public float displacementAmount = 3f;
    private bool onWater = true;

    private void FixedUpdate() {
        if (MultiplayerManager.Instance.gameMode != GameMode.MultiplayerBattle || onWater) {
            float displacementMultiplier = Mathf.Clamp01((-transform.position.y) / depthBeforeSubmerged) * displacementAmount;
            hull.AddForce(new Vector3(0f, Mathf.Abs(Physics.gravity.y)* displacementMultiplier, 0f), ForceMode.Acceleration);
        }
    }

    private void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.tag == "Water") {
            onWater = true;
        }
    }

    private void OnTriggerExit(Collider collider) {
        if (collider.gameObject.tag == "Water") {
            onWater = false;
        }
    }
}
