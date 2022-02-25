using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleWatcher : MonoBehaviour {
    [SerializeField] Rigidbody hull;
    [SerializeField] bool leftPaddle;
    [SerializeField] int velocityMultiplier;
    [SerializeField] GameObject player;

    private float oldX;  
    private float velocity;

    private void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.tag == "Water") {
            oldX = player.transform.InverseTransformPoint(transform.position).x;
        }
    }

    private void OnTriggerStay(Collider collider) {
        velocity = -(player.transform.InverseTransformPoint(transform.position).x - oldX);
        Debug.Log(velocity*5);
        oldX = player.transform.InverseTransformPoint(transform.position).x;

        if (collider.gameObject.tag == "Water") {
            hull.AddRelativeForce(new Vector3(velocity*5, 1f, 0f), ForceMode.Acceleration);
            if (leftPaddle) {
                hull.AddTorque(0f, 1f, 0f, ForceMode.Acceleration);
            }
            else {
                hull.AddTorque(0f, -1f, 0f, ForceMode.Acceleration);
            }
        }
    }
}
