using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour {
    [SerializeField] GameObject[] point;
    int currentIndex = 0;

    void Update() {
        if (Vector3.Distance(transform.position, point[currentIndex].transform.position) > 0.1f) {
            transform.position = Vector3.MoveTowards(transform.position, point[currentIndex].transform.position, 8*Time.deltaTime);
        }
        else {
            currentIndex++;
            if (currentIndex >= point.Length) currentIndex = 0;
        }
    }
}
