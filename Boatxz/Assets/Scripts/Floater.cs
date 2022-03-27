using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// From https://www.youtube.com/watch?v=eL_zHQEju8s&ab_channel=TomWeiland
/// <summary>
/// Controls the hull's bobbing effect and gravity
/// </summary>
public class Floater : MonoBehaviour {
    private const float DEPTH_BEFORE_SUBMERGED = 1f;
    private const float DISPLACEMENT_AMOUNT = 6f;
    private bool onWater = true;

    void FixedUpdate() {
        Rigidbody hull = transform.GetComponent<Rigidbody>();
        if ((MultiplayerManager.Instance.gameMode != GameMode.MultiplayerBattle && MultiplayerManager.Instance.gameMode != GameMode.Lobby) || onWater) {
            float displacementMultiplier = Mathf.Clamp01((-transform.position.y) / DEPTH_BEFORE_SUBMERGED) * DISPLACEMENT_AMOUNT;
            hull.AddForce(new Vector3(0f, Mathf.Abs(Physics.gravity.y)* displacementMultiplier, 0f), ForceMode.Acceleration);
        }
    }

    void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.tag == "Water") {
            onWater = true;
        }
    }

    void OnTriggerExit(Collider collider) {
        if (collider.gameObject.tag == "Water") {
            onWater = false;
        }
    }
}
