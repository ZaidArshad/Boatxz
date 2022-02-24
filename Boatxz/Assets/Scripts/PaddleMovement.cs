using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleMovement : MonoBehaviour {
    [SerializeField] GameObject leftPaddle, rightPaddle;

    // Update is called once per frame
    void Update() {
        float horizontalInput = 0;
        float verticalInput = 0;
        float range = 40;

        horizontalInput = Input.GetAxis("LeftX");
        verticalInput = Input.GetAxis("LeftY");
        leftPaddle.transform.eulerAngles = new Vector3(-range*horizontalInput, -range*verticalInput, 0f);
        
        
        horizontalInput = Input.GetAxis("RightX");
        verticalInput = Input.GetAxis("RightY");
        rightPaddle.transform.eulerAngles = new Vector3(-range*horizontalInput, range*verticalInput, 0f);
        

        
        // if (deadZone < horizontalInput || horizontalInput < -deadZone) {
        //     Debug.Log("h: " + horizontalInput);
        // }
        // if (deadZone < verticalInput || verticalInput < -deadZone) {
        //     Debug.Log("v: " + verticalInput);
        // }
    }
}
