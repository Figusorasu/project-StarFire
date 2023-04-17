using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRegionChanger : MonoBehaviour
{
    [Header("Camera Bounds A")]
        [SerializeField] private Vector2 camMinPosA;
        [SerializeField] private Vector2 camMaxPosA;
    [Header("Camera Bounds B")]
        [SerializeField] private Vector2 camMinPosB;
        [SerializeField] private Vector2 camMaxPosB;

/*
    [SerializeField] private Vector2[] cameraBoundsA = new Vector2[2];
    [SerializeField] private Vector2[] cameraBoundsB = new Vector2[2];

    [SerializeField] private List<Vector2> cameraBoundsAA = new List<Vector2>();
*/

    [SerializeField] private CameraChanger changerA;
    [SerializeField] private CameraChanger changerB;

    private void Start() {
        changerA.newMinPos = camMinPosA;
        changerA.newMaxPos = camMaxPosA;
        changerB.newMinPos = camMinPosB;
        changerB.newMaxPos = camMaxPosB;
    }

}