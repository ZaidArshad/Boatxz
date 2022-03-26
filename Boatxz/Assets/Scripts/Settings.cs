using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls settings
/// </summary>
public class Settings : MonoBehaviour
{
    private void Awake() {
        QualitySettings.vSyncCount = 2;  // VSync must be disabled
    }
}
