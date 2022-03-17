using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HullAttributes : MonoBehaviour {
    [SerializeField] GameObject torpedo;
    [SerializeField] Material hunterMaterial;
    private PlayerDetector lastCheckpoint;
    public Transform startingPosition;
    private int playerNumber;
    private float torpedoCooldown = 1;

    private Vector3 HUNTER_SCALE = new Vector3(6.90999985f*2,0.529999971f*2,2f*2); 

    private void Start() {
        playerNumber = MultiplayerManager.Instance.join(gameObject);
        startingPosition = MultiplayerManager.Instance.getStartingPosition(playerNumber);
        goToOriginalStart();
    }

    public void becomeHunter() {
        setVelocityMultiplier(HullMovement.HUNTER_VELOCITY_MULTIPLIER);
        gameObject.GetComponent<Renderer>().material = hunterMaterial;
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
        if (collider.transform.tag == "Hull") {
            if (playerNumber == 0 && MultiplayerManager.Instance.gameMode == GameMode.BoatHunt) {
                MultiplayerManager.Instance.leave(collider.gameObject.GetComponent<HullAttributes>().getPlayerNumber());
            }
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

    public void setVelocityMultiplier(int multiplier) {
        Transform leftPaddle = transform.GetChild(0).GetChild(1).GetChild(1);
        Transform rightPaddle = transform.GetChild(0).GetChild(2).GetChild(1);
        leftPaddle.GetComponent<HullMovement>().setVelocityMultiplier(multiplier);
        rightPaddle.GetComponent<HullMovement>().setVelocityMultiplier(multiplier);
    }

    private void Update() {
        torpedoCooldown -= Time.deltaTime;
    }

    public void OnRB(InputAction.CallbackContext context) {
        if (context.performed) {
            if ((MultiplayerManager.Instance.gameMode != GameMode.MultiplayerBattle && MultiplayerManager.Instance.gameMode != GameMode.BoatHunt)
                || (MultiplayerManager.Instance.gameMode == GameMode.BoatHunt && playerNumber == 0)) reset();
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
