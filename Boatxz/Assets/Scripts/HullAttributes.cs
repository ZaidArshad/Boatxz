using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HullAttributes : MonoBehaviour {
    [SerializeField] GameObject torpedo;
    private PlayerDetector lastCheckpoint;
    public Transform startingPosition;
    private int playerNumber;
    private float torpedoCooldown = 1;

    private void Start() {
        playerNumber = MultiplayerManager.Instance.join(gameObject);
        startingPosition = MultiplayerManager.Instance.getStartingPosition(playerNumber);
        goToOriginalStart();
    }

    private void OnCollisionEnter(Collision collider) {
        if (collider.transform.tag == "Lava") {
            MultiplayerManager.Instance.leave(playerNumber);
        }
        if (collider.transform.tag == "Torpedo") {
            Rigidbody hull = gameObject.GetComponent<Rigidbody>();
            Vector3 direction = transform.position-collider.transform.position;
            hull.AddForceAtPosition(direction.normalized*1000, collider.transform.position);
            Destroy(collider.gameObject);
        }
    }

    public void setLastCheckpoint(PlayerDetector checkpoint) {
        lastCheckpoint = checkpoint;
    }

    public PlayerDetector getLastCheckpoint() {
        return lastCheckpoint;
    }

    public int getPlayerNumber() {
        return playerNumber;
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

    private void Update() {
        torpedoCooldown -= Time.deltaTime;
    }

    public void OnRB(InputAction.CallbackContext context) {
        if (context.performed) {
            if (MultiplayerManager.Instance.gameMode != GameMode.MultiplayerBattle) reset();
        }
    }

    public void OnB(InputAction.CallbackContext context) {
        if (context.performed) {
            if (!MultiplayerManager.Instance.isGameStarted()) MultiplayerManager.Instance.leave(playerNumber);
        }
    }

    public void OnStart(InputAction.CallbackContext context) {
        if (context.performed) {
            MultiplayerManager.Instance.startGame();
        }
    }

    public void OnRT(InputAction.CallbackContext context) {
        if (context.performed) {
            if (MultiplayerManager.Instance.gameMode == GameMode.MultiplayerBattle) {
                shootTorpedo();
            }
        }
    }

    private void shootTorpedo() {
        if (torpedoCooldown < 0) { 
            Vector3 position = transform.GetChild(1).position;
            Quaternion rotation = transform.GetChild(1).rotation;
            GameObject missle = Instantiate(torpedo, position, rotation);
            torpedoCooldown = 1;
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
