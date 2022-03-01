using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleWatcher : MonoBehaviour {
    [SerializeField] Rigidbody hull;
    [SerializeField] bool leftPaddle;
    [SerializeField] GameObject player;

    private int velocityMultiplier = 20;
    private float turnCoefficient = 0.05f;
    private float zRotationRange = 5.0f;

    private float oldX;  
    private float velocity;

    private void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.tag == "Water") {
            oldX = player.transform.InverseTransformPoint(transform.position).x;
            hull.drag = 2;
        }
    }

    private void OnTriggerExit(Collider collider) {
        if (collider.gameObject.tag == "Water") {
            hull.drag = 0.5f;
        }
    }

    private void OnTriggerStay(Collider collider) {
        velocity = (oldX - player.transform.InverseTransformPoint(transform.position).x) * velocityMultiplier;
        oldX = player.transform.InverseTransformPoint(transform.position).x;
        Debug.Log(velocity);

        if (collider.gameObject.tag == "Water") {
            float rotZ = 0;
            if (velocity > 0) {
                rotZ = 1;
            }
            else if (velocity < 0) {
                rotZ = -1;
            }
            if (-5 > transform.rotation.z || transform.rotation.z > 5) rotZ = 0;

            hull.AddRelativeForce(new Vector3(velocity, -velocity, 0f), ForceMode.Acceleration);
            if (leftPaddle) {
                hull.AddTorque(0f, 0, rotZ, ForceMode.Acceleration);
            }
            else {
                hull.AddTorque(0f, -(0), rotZ, ForceMode.Acceleration);
            }
        }
    }
}
