using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleWatcher : MonoBehaviour {
    [SerializeField] Rigidbody hull;
    [SerializeField] bool leftPaddle;
    [SerializeField] GameObject player;

    private int velocityMultiplier = 30;
    private float turnCoefficient = 0.05f;

    private float oldX;  
    private float velocity;

    private void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.tag == "Water") {
            oldX = player.transform.InverseTransformPoint(transform.position).x;
            hull.drag = 1f;
        }
    }

    private void OnTriggerExit(Collider collider) {
        if (collider.gameObject.tag == "Water") {
            hull.drag = 0.2f;
        }
    }

    private void OnTriggerStay(Collider collider) {
        velocity = (oldX - player.transform.InverseTransformPoint(transform.position).x) * velocityMultiplier;
        oldX = player.transform.InverseTransformPoint(transform.position).x;
        Debug.Log(velocity);

        if (collider.gameObject.tag == "Water") {

            hull.AddRelativeForce(new Vector3(velocity, -velocity, 0f), ForceMode.Acceleration);
            if (leftPaddle) {
                hull.AddTorque(0f, velocity*turnCoefficient, 0, ForceMode.Acceleration);
            }
            else {
                hull.AddTorque(0f, -(velocity*turnCoefficient), 0, ForceMode.Acceleration);
            }
        }
    }
}
