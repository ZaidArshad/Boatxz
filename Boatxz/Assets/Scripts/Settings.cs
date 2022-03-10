using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    private void Awake() {
        QualitySettings.vSyncCount = 2;  // VSync must be disabled
    }
}
