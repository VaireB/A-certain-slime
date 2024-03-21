using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target; // Reference to the target to follow (slime)
    public float distance = 5.0f; // Distance from the target
    public float height = 3.0f; // Height of the camera
    public float rotationDamping = 2.0f; // Damping for camera rotation
    public float heightDamping = 3.0f; // Damping for camera height adjustment
    public float rotationSpeed = 1.0f; // Speed of camera rotation around the target

    private void LateUpdate()
    {
        if (target != null)
        {
            // Calculate the desired rotation angle based on the target's rotation
            float wantedRotationAngle = target.eulerAngles.y;
            float wantedHeight = target.position.y + height;

            // Calculate the current rotation angle and height
            float currentRotationAngle = transform.eulerAngles.y;
            float currentHeight = transform.position.y;

            // Smoothly adjust the rotation angle and height
            currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
            currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

            // Convert the angle into a rotation
            Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

            // Set the position of the camera based on the target's position, rotation, and distance
            transform.position = target.position - currentRotation * Vector3.forward * distance;
            transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

            // Rotate the camera to look at the target
            transform.LookAt(target);
        }
    }
}
