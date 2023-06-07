using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float distance = 10f;
    [SerializeField] private float minDistance = 5f;
    [SerializeField] private float maxDistance = 15f;
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float xSpeed = 250f;
    [SerializeField] private float ySpeed = 120f;
    [SerializeField] private float yMinLimit = -20f;
    [SerializeField] private float yMaxLimit = 80f;
    [SerializeField] private bool isFollowingMouse = true;
    [SerializeField] public Vector3 verticalOffset;

    private float x = 0f;
    private float y = 0f;

    private void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
    }

    private void LateUpdate()
    {
        if (target)
        {
            if (isFollowingMouse)
            {
                float capsuleHeight = target.GetComponent<CapsuleCollider>().height;
                Vector3 targetPosition = target.position + Vector3.up * capsuleHeight;

                x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
                y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

                y = ClampAngle(y, yMinLimit, yMaxLimit);

                Quaternion rotation = Quaternion.Euler(y, x, 0f);

                distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, minDistance, maxDistance);

                RaycastHit hit;
                if (Physics.Linecast(targetPosition, transform.position, out hit))
                {
                    distance -= hit.distance;
                }

                Vector3 negDistance = new Vector3(0f, 0f, -distance);
                Vector3 position = rotation * negDistance + targetPosition;

                transform.rotation = rotation;
                transform.position = position;
            }
        }
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f)
        {
            angle += 360f;
        }
        if (angle > 360f)
        {
            angle -= 360f;
        }

        return Mathf.Clamp(angle, min, max);
    }

    public void ToggleFollowingMouse()
    {
        isFollowingMouse = !isFollowingMouse;
    }
}