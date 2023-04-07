using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private float minRotationSpeed = 0f;
    [SerializeField] private float maxRotationSpeed;

    [SerializeField] private float rotateSpeed;

    private enum RotationType{Horizontal, Vertical, Round}
    [SerializeField] private RotationType rotationType; 
    private enum RotationDirection{Right, Left}
    [SerializeField] private RotationDirection rotationDirection;


    private void Update()
    {
        switch (rotationType)
        {
            case RotationType.Horizontal:
                if(rotationDirection == RotationDirection.Right) {
                    transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.Self);
                } else {
                    transform.Rotate(Vector3.up * -rotateSpeed * Time.deltaTime, Space.Self);
                }
            break;
            case RotationType.Vertical:
                if(rotationDirection == RotationDirection.Right) {
                    transform.Rotate(Vector3.right * rotateSpeed * Time.deltaTime, Space.Self);
                } else {
                    transform.Rotate(Vector3.right * -rotateSpeed * Time.deltaTime, Space.Self);
                }
            break;
            case RotationType.Round:
                if(rotationDirection == RotationDirection.Right) {
                    transform.Rotate(Vector3.forward * -rotateSpeed * Time.deltaTime, Space.Self);
                } else {
                    transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime, Space.Self);
                }
            break;
        }
    }
}
