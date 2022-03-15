using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HullAttributes : MonoBehaviour
{
    private PlayerDetector lastCheckpoint;
    public Transform startingPosition;
    private int playerNumber;

    private void Start() {
        playerNumber = MultiplayerManager.Instance.join(gameObject);
        startingPosition = MultiplayerManager.Instance.getStartingPosition(playerNumber);
        goToOriginalStart();
    }

    public void setLastCheckpoint(PlayerDetector checkpoint) {
        lastCheckpoint = checkpoint;
    }

    public PlayerDetector getLastCheckpoint() {
        return lastCheckpoint;
    }

    public void reset() {
        if (lastCheckpoint != null) {
                transform.eulerAngles = new Vector3(
                    lastCheckpoint.transform.eulerAngles.x,
                    lastCheckpoint.transform.eulerAngles.y-90,
                    lastCheckpoint.transform.eulerAngles.z);
                transform.position = lastCheckpoint.transform.position;
                stopForces();
            }
            else {
                goToOriginalStart();
            }
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

    private void stopForces() {
        transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
        transform.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

    private void goToOriginalStart() {
        transform.position = startingPosition.position;
        transform.rotation = startingPosition.rotation;
        stopForces();
    }
}
