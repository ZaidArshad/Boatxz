using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class HullAttributes : MonoBehaviour {
    [SerializeField] GameObject torpedo;
    [SerializeField] Material hunterMaterial;
    [SerializeField] Text prompt;

    private PlayerDetector lastCheckpoint;
    public Transform startingPosition;
    private int playerNumber;
    private float torpedoCooldown = 1;

    private Vector3 HUNTER_SCALE = new Vector3(6.90999985f*2,0.529999971f*2,2f*2);
    private const int RIGHTING_MULTIPLIER = 10;

    private void Start() {
        playerNumber = MultiplayerManager.Instance.join(gameObject);
        startingPosition = MultiplayerManager.Instance.getStartingPosition(playerNumber);
        goToOriginalStart();
        MultiplayerManager.Instance.singlePlayer();
    }

    public void becomeHunter() {
        gameObject.GetComponent<Renderer>().material = hunterMaterial;
    }

    public void showPrompt(string msg) {
        prompt.text = msg;
        prompt.color = new Color(255, 255, 255, 255);
    }

    public void becomeHunted() {
        setVelocityMultiplier(20);
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

    public void setVelocityMultiplier(int multiplier) {
        Transform leftPaddle = transform.GetChild(0).GetChild(1).GetChild(1);
        Transform rightPaddle = transform.GetChild(0).GetChild(2).GetChild(1);
        leftPaddle.GetComponent<HullMovement>().setVelocityMultiplier(multiplier);
        rightPaddle.GetComponent<HullMovement>().setVelocityMultiplier(multiplier);
    }

    private void Update() {
        torpedoCooldown -= Time.deltaTime;
        selfRight();
    }

    private void selfRight() {
        if ((transform.eulerAngles.x > 0.1 || transform.eulerAngles.z > 0.1)) {
            Quaternion rotation = transform.rotation;
            rotation.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, Time.deltaTime*RIGHTING_MULTIPLIER);
        }
    }

    public void OnRB(InputAction.CallbackContext context) {
        if (context.performed) {
            if (MultiplayerManager.Instance.gameMode != GameMode.MultiplayerBattle) {
                MultiplayerManager.Instance.reset(playerNumber);
            }
        }
    }

    public void OnB(InputAction.CallbackContext context) {
        if (context.performed) {
            if (MultiplayerManager.Instance.isGameFinished()) {
                UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            }
            else if (!MultiplayerManager.Instance.isGameStarted() && !MultiplayerManager.Instance.isSinglePlayer()) {
                MultiplayerManager.Instance.leave(playerNumber);
            }
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

    public void stopForces() {
        transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
        transform.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

    public void goToOriginalStart() {
        goToPosition(startingPosition);
    }

    public void goToPosition(Transform pos) {
        transform.position = pos.position;
        transform.rotation = pos.rotation;
        stopForces();
    }
}
