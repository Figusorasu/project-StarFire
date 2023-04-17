using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRegionChanger : MonoBehaviour
{
    [Header("Camera Bounds A")]
        [SerializeField] private CameraChanger changerA;
        [SerializeField] private Vector2 camMinPosA;
        [SerializeField] private Vector2 camMaxPosA;
    [Header("Camera Bounds B")]
        [SerializeField] private CameraChanger changerB;
        [SerializeField] private Vector2 camMinPosB;
        [SerializeField] private Vector2 camMaxPosB;

    private void Start() {
        changerA.newMinPos = camMinPosA;
        changerA.newMaxPos = camMaxPosA;
        changerB.newMinPos = camMinPosB;
        changerB.newMaxPos = camMaxPosB;
    }
}