using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleMovement : MonoBehaviour {
    [SerializeField] bool leftPaddle;

    // Update is called once per frame
    void Update() {
        float horizontalInput = 0;
        float verticalInput = 0;
        if (leftPaddle) {
            horizontalInput = Input.GetAxis("LeftX");
            verticalInput = Input.GetAxis("LeftY");
            transform.eulerAngles = new Vector3(-20*horizontalInput, -20*verticalInput, 0f);
        }
        else {
            horizontalInput = Input.GetAxis("RightX");
            verticalInput = Input.GetAxis("RightY");
            transform.eulerAngles = new Vector3(-20*horizontalInput, 20*verticalInput, 0f);
        }

        
        // if (deadZone < horizontalInput || horizontalInput < -deadZone) {
        //     Debug.Log("h: " + horizontalInput);
        // }
        // if (deadZone < verticalInput || verticalInput < -deadZone) {
        //     Debug.Log("v: " + verticalInput);
        // }
    }
}
