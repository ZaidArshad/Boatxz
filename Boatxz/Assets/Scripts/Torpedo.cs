using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the torpedo a player shoots
/// To be applied to a torpedo object
/// </summary>
public class Torpedo : MonoBehaviour {
    [SerializeField] Rigidbody missle;

    float time = 0;

    private void Update() {
        missle.AddRelativeForce(new Vector3 (5, 0, 0));
        time += Time.deltaTime;
        if (time > 5) Destroy(gameObject);
    }
}
