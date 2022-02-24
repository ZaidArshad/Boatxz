using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleWatcher : MonoBehaviour {
    [SerializeField] Rigidbody hull;
    [SerializeField] bool leftPaddle;

    private void OnTriggerStay(Collider collider) {
        if (collider.gameObject.tag == "Water") {
            hull.AddRelativeForce(new Vector3(3f, 0f, 0f), ForceMode.Acceleration);
            if (leftPaddle) {
                hull.AddTorque(0f, 1f, 0f, ForceMode.Acceleration);
            }
            else {
                hull.AddTorque(0f, -1f, 0f, ForceMode.Acceleration);
            }
        }
    }
}
