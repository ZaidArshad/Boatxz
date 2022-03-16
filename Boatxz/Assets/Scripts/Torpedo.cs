using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torpedo : MonoBehaviour {
    [SerializeField] Rigidbody missle;

    float time = 0;

    private void Update() {
        missle.AddRelativeForce(new Vector3 (1, 0, 0));
        time += Time.deltaTime;
        if (time > 5) Destroy(gameObject);
    }
}
