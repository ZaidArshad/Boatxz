using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

/// <summary>
/// Controls the rotations and positions of the paddles
/// To be applied to the hull
/// </summary>
public class PaddleMovement : MonoBehaviour {
    [SerializeField] GameObject leftPaddle, rightPaddle;
    [SerializeField] float range = 40;
    [SerializeField] Text speedText;

    private Rigidbody hullBody;
    private const float KNOTS_PER_METER = 1.94384f;
    private const string KNOT_SYMBOL_STR = "kn";
    private float leftX, leftY, rightX, rightY;

    void Start() {
        hullBody = gameObject.GetComponent<Rigidbody>();
    }

    void Update() {
        paddle();
    }

    public void OnLeftX(InputAction.CallbackContext context) {
        leftX = context.ReadValue<float>();
    }
    public void OnLeftY(InputAction.CallbackContext context) {
        leftY = -context.ReadValue<float>();
    }
    public void OnRightX(InputAction.CallbackContext context) {
        rightX = context.ReadValue<float>();
    }
    public void OnRightY(InputAction.CallbackContext context) {
        rightY = -context.ReadValue<float>();
    }

    private void paddle() {
        Vector3 p = leftPaddle.transform.parent.eulerAngles;
        speedText.text = Mathf.Abs(hullBody.velocity.magnitude * KNOTS_PER_METER).ToString("F2") + KNOT_SYMBOL_STR;
        leftPaddle.transform.eulerAngles = new Vector3(-range*leftX+p.x, -range*leftY+p.y, p.z);
        rightPaddle.transform.eulerAngles = new Vector3(-range*rightX+p.x, range*rightY+p.y, p.z);
    }
}
