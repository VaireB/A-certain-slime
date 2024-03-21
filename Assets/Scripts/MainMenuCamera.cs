using UnityEngine;

public class MainMenuCameraPanner : MonoBehaviour
{
    public Transform target; // The prefab's transform to rotate around
    public float rotationSpeed = 10f; // Speed of rotation
    public float heightOffset = 5f; // Vertical offset from the target
    public float distance = 10f; // Distance from the target
    public float initialAngle = 0f; // Initial rotation angle around the target

    private float currentAngle = 0f; // Current rotation angle around the target

    private void Start()
    {
        currentAngle = initialAngle;
    }

    private void Update()
    {
        if (target != null)
        {
            // Calculate the desired position
            Vector3 desiredPosition = target.position - Quaternion.Euler(0f, currentAngle, 0f) * Vector3.forward * distance + Vector3.up * heightOffset;

            // Move the camera towards the desired position
            transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * 2f);

            // Rotate the camera around the target
            currentAngle += rotationSpeed * Time.deltaTime;
            transform.LookAt(target.position);
        }
    }
}
