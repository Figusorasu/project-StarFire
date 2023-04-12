using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private float rotateSpeed;

    [Space]
    [SerializeField] private bool randomSpeed;
    [SerializeField] private float minRotationSpeed;
    [SerializeField] private float maxRotationSpeed;

    [Space]
    [SerializeField] private bool randomStartTime;

    [Header("Rotation Type and Direction")]
    [SerializeField] private RotationType rotationType;
    [SerializeField] private RotationDirection rotationDirection;

    private enum RotationType{Horizontal, Vertical, Round}
    private enum RotationDirection{Right, Left}

    private Vector3 vectorType;

    private bool startRotation = false;
  
    private void Start() {
        if(randomSpeed) {
            rotateSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
        }
        if(rotationDirection == RotationDirection.Left) {
            rotateSpeed = (-rotateSpeed);
        }

        vectorType = setRotationType(rotationType);

        if(randomStartTime) {
            StartCoroutine("waitForSeconds");
        } else {
            startRotation = true;
        }
    }

    private Vector3 setRotationType(RotationType type) {
        if(type == RotationType.Horizontal) {
            return new Vector3(0,1,0);
        } else if(type == RotationType.Vertical) {
            return new Vector3(1,0,0);
        } else {
            return new Vector3(0,0,1);
        }
    }

    private IEnumerator waitForSeconds() {
        float waitTime = Random.Range(0f, 2f);
        yield return new WaitForSeconds(waitTime);
        startRotation = true;
    }

    private void Update() {   
        if(startRotation) {
            transform.Rotate(vectorType * rotateSpeed * Time.deltaTime, Space.Self);
        }
    }
}