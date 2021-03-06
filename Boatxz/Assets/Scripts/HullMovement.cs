using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the forces applied to the hull to turn and move forward / backwards
/// Applied to the paddle tip
/// </summary>
public class HullMovement : MonoBehaviour {
    [SerializeField] Rigidbody hull;
    [SerializeField] bool leftPaddle;
    [SerializeField] GameObject player;

    public const int VELOCITY_MULTIPLIER = 30;
    public const int HUNTER_VELOCITY_MULTIPLIER = 60;
    private const float TURN_COEFFIECENT = 0.10f;
    private const float ELEVATION_COEFFIECENT = 0f;
    private const float WATER_DRAG = 0.5f;
    private const float WATER_ANGULAR_DRAG = 0.5f;
    private const float DRAG = 0.1f;
    private const float ANGULAR_DRAG = 0.1f;

    private float oldX;  
    private float velocity;
    private int velocityMultiplier = VELOCITY_MULTIPLIER;
    
    void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.tag == "Water") {
            oldX = player.transform.InverseTransformPoint(transform.position).x;
            hull.drag = WATER_DRAG;
            hull.angularDrag = WATER_ANGULAR_DRAG;
        }
    }

    void OnTriggerExit(Collider collider) {
        if (collider.gameObject.tag == "Water") {
            hull.drag = DRAG;
            hull.angularDrag = ANGULAR_DRAG;
        }
    }

    void OnTriggerStay(Collider collider) {
        velocity = (oldX - player.transform.InverseTransformPoint(transform.position).x) * velocityMultiplier;
        oldX = player.transform.InverseTransformPoint(transform.position).x;

        if (collider.gameObject.tag == "Water") {
            moveHull();
        }
    }

    public void setVelocityMultiplier(int multiplier) {
        velocityMultiplier = multiplier;
    }

    private void moveHull() {
        if (MultiplayerManager.Instance.isGameStarted()) {
            hull.AddRelativeForce(new Vector3(velocity, 0f, 0f), ForceMode.Acceleration);
            if (leftPaddle) {
                hull.AddTorque(0f, velocity*TURN_COEFFIECENT, 0, ForceMode.Acceleration);
            }
            else {
                hull.AddTorque(0f, -(velocity*TURN_COEFFIECENT), 0, ForceMode.Acceleration);
            }
        }
    }
}
