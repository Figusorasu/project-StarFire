using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChanger : MonoBehaviour
{
    [HideInInspector] public Vector2 newMinPos;
    [HideInInspector] public Vector2 newMaxPos;

    private Camera cam;

    private void Start() {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")) {
            cam.GetComponent<CameraController>().minPosition = newMinPos;
            cam.GetComponent<CameraController>().maxPosition = newMaxPos;
        }
    }
}
