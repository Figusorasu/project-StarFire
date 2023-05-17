using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Transform target;
    [SerializeField] private float smoothing;

    [Header("Bounding")]
    public Vector2 maxPosition;
    public Vector2 minPosition;

    void FixedUpdate() {
        if(transform.position != target.position) {
            Vector3 targetPosition = new Vector3(target.position.x, target.position.y, -10f);
            targetPosition = SetCameraBounding(targetPosition, maxPosition, minPosition);
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing);
        }
    }

    private Vector3 SetCameraBounding(Vector3 cameraTargetPosition, Vector2 cameraMaxPos, Vector2 cameraMinPos) {
        cameraTargetPosition.x = Mathf.Clamp(cameraTargetPosition.x, cameraMinPos.x, cameraMaxPos.x);
        cameraTargetPosition.y = Mathf.Clamp(cameraTargetPosition.y, cameraMinPos.y, cameraMaxPos.y);
        return cameraTargetPosition;
    }
}
