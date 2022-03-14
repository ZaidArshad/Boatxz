using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HullAttributes : MonoBehaviour
{
    private GameObject lastCheckpoint;
    public Vector3 startingPosition;
    public Quaternion startingRotation;
    private int playerNumber;

    private void Awake() {
        if (MultiplayerManager.Instance != null) { playerNumber = MultiplayerManager.Instance.join(gameObject);
            startingPosition = MultiplayerManager.Instance.getStartingPosition(playerNumber);
            startingRotation = MultiplayerManager.Instance.getStartingRotation();
        }
        transform.position = startingPosition;
        transform.rotation = startingRotation;
    }

    public void setLastCheckpoint(GameObject checkpoint) {
        lastCheckpoint = checkpoint;
    }

    public void reset() {
        if (lastCheckpoint != null) {
                transform.eulerAngles = new Vector3(
                    lastCheckpoint.transform.eulerAngles.x,
                    lastCheckpoint.transform.eulerAngles.y-90,
                    lastCheckpoint.transform.eulerAngles.z);
                transform.position = lastCheckpoint.transform.position;
            }
            else {
                transform.position = startingPosition;
                transform.rotation = startingRotation;
            }
            transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

    public void OnRB(InputAction.CallbackContext context) {
        if (context.performed) {
            reset();
        }
    }

    public void OnB(InputAction.CallbackContext context) {
        if (context.performed) {
            MultiplayerManager.Instance.leave(playerNumber);
        }
    }

    public void OnStart(InputAction.CallbackContext context) {
        if (context.performed) {
            MultiplayerManager.Instance.startGame();
        }
    }
}
