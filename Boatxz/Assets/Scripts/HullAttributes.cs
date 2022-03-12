using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HullAttributes : MonoBehaviour
{
    private GameObject lastCheckpoint;
    [SerializeField] Transform startingPosition;

    private void Start() {
        string[] names = Input.GetJoystickNames();
         for (int x = 0; x < names.Length; x++) {
             Debug.Log(names[x]);
         }
    }

    private void Update() {
        bool restart = Input.GetButtonUp("P1RB");
        //Debug.Log(Input.GetAxis("P2RightX"));
        if (restart) {
            reset();
        }
    }

    public void setLastCheckpoint(GameObject checkpoint) {
        lastCheckpoint = checkpoint;
    }

    private void reset() {
        if (lastCheckpoint != null) {
                transform.eulerAngles = new Vector3(
                    lastCheckpoint.transform.eulerAngles.x,
                    lastCheckpoint.transform.eulerAngles.y-90,
                    lastCheckpoint.transform.eulerAngles.z);
                transform.position = lastCheckpoint.transform.position;
            }
            else {
                transform.position = startingPosition.position;
                transform.rotation = startingPosition.rotation;
            }
            transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }
}
