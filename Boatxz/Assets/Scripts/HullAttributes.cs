using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HullAttributes : MonoBehaviour
{
    private GameObject lastCheckpoint;
    private Vector3 startingPosition;
    private Quaternion startingRotation;

    private void Awake() {
        startingPosition = LevelAttributes.Instance.getStartingPosition().position;
        startingRotation = LevelAttributes.Instance.getStartingPosition().rotation; 
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
}
