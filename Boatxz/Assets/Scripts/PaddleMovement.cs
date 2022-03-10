using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaddleMovement : MonoBehaviour {
    [SerializeField] GameObject leftPaddle, rightPaddle;
    [SerializeField] float range = 40;
    [SerializeField] Text speedText;

    private Rigidbody hullBody;
    private const float KNOTS_PER_METER = 1.94384f;
    private const string KNOT_SYMBOL_STR = "kn";

    private void Start() {
        hullBody = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {
        float horizontalInput = 0;
        float verticalInput = 0;
        Vector3 p = leftPaddle.transform.parent.eulerAngles;

        speedText.text = Mathf.Abs(hullBody.velocity.magnitude * KNOTS_PER_METER).ToString("F2") + KNOT_SYMBOL_STR;
        

        horizontalInput = Input.GetAxis("LeftX");
        verticalInput = Input.GetAxis("LeftY");
        leftPaddle.transform.eulerAngles = new Vector3(-range*horizontalInput+p.x, -range*verticalInput+p.y, p.z);
        
        
        horizontalInput = Input.GetAxis("RightX");
        verticalInput = Input.GetAxis("RightY");
        rightPaddle.transform.eulerAngles = new Vector3(-range*horizontalInput+p.x, range*verticalInput+p.y, p.z);
    }
}
