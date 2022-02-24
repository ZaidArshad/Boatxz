using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleMovement : MonoBehaviour {
    [SerializeField] GameObject leftPaddle, rightPaddle;
    [SerializeField] float range = 40;

    // Update is called once per frame
    void Update() {
        float horizontalInput = 0;
        float verticalInput = 0;
        Vector3 p = leftPaddle.transform.parent.eulerAngles;

        horizontalInput = Input.GetAxis("LeftX");
        verticalInput = Input.GetAxis("LeftY");
        leftPaddle.transform.eulerAngles = new Vector3(-range*horizontalInput+p.x, -range*verticalInput+p.y, p.z);
        
        
        horizontalInput = Input.GetAxis("RightX");
        verticalInput = Input.GetAxis("RightY");
        rightPaddle.transform.eulerAngles = new Vector3(-range*horizontalInput+p.x, range*verticalInput+p.y, p.z);
    }
}
