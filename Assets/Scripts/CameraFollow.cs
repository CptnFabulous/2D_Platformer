using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    public Transform referenceTransform;
    public float distance = 5;
    public float adjustSpeed = 5;

    Camera viewCamera;
    Vector3 cameraTransitionPosition;
    Quaternion cameraTransitionRotation;

    private void Awake()
    {
        viewCamera = GetComponent<Camera>();
    }
    private void OnEnable()
    {
        cameraTransitionPosition = referenceTransform.position;
        cameraTransitionRotation = referenceTransform.rotation;
    }
    private void LateUpdate()
    {
        cameraTransitionRotation = Quaternion.Lerp(cameraTransitionRotation, referenceTransform.rotation, Time.deltaTime * adjustSpeed);
        cameraTransitionPosition = Vector3.Lerp(cameraTransitionPosition, referenceTransform.position, Time.deltaTime * adjustSpeed);

        viewCamera.transform.rotation = cameraTransitionRotation;
        viewCamera.transform.position = cameraTransitionPosition + (transform.forward * -distance);
    }
}
