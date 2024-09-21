using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCameraController : MonoBehaviour
{
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;  // Take the main camera from the scene
    }

    private void LateUpdate()
    {
        // The Canvas will always look at the camera
        if (mainCamera != null)
        {
            transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
                             mainCamera.transform.rotation * Vector3.up);
        }
    }
}
