using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour {
    // From https://www.youtube.com/watch?v=eL_zHQEju8s&ab_channel=TomWeiland
    public Rigidbody hull;
    public float depthBeforeSubmerged = 1f;
    public float displacementAmount = 3f;

    private void FixedUpdate() {
        if (transform.position.y < 0f) {
            float displacementMultiplier = Mathf.Clamp01((-transform.position.y) / depthBeforeSubmerged) * displacementAmount;
            hull.AddForce(new Vector3(0f, Mathf.Abs(Physics.gravity.y)* displacementMultiplier, 0f), ForceMode.Acceleration);
        }
    }
}
