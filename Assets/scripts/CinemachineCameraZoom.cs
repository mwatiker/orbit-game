using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineCameraZoom : MonoBehaviour
{
    public CinemachineVirtualCamera cinemachineCamera;
    public float zoomSpeed = 1.0f;
    public float minOrthoSize = 5.0f;
    public float maxOrthoSize = 20.0f;

    public bool positionChange = false;

    public float CinemachineBrainTransitionTime = 1.1f;

    // store the original ortho size and position
    private float originalOrthoSize;
    private Vector3 originalPosition;


    


    private void Start()
    {
        originalOrthoSize = cinemachineCamera.m_Lens.OrthographicSize;
        originalPosition = cinemachineCamera.transform.position;
    }

    void Update()
    {
        if (IsCameraActive())
        {
            ZoomCamera();
        }
    }

    public bool IsCameraActive()
    {
        CinemachineBrain brain = Camera.main.GetComponent<CinemachineBrain>();
        return brain != null && brain.ActiveVirtualCamera == cinemachineCamera;
    }

    private void ZoomCamera()
    {

        if (Input.mouseScrollDelta.y != 0)
        {
            float zoomAmount = Input.mouseScrollDelta.y * zoomSpeed;
            float newOrthoSize = Mathf.Clamp(cinemachineCamera.m_Lens.OrthographicSize - zoomAmount, minOrthoSize, maxOrthoSize);

            float zoomRatio = newOrthoSize / cinemachineCamera.m_Lens.OrthographicSize;
            cinemachineCamera.m_Lens.OrthographicSize = newOrthoSize;

            if (positionChange)
            {
                // Get the world position of the cursor
                Vector3 worldCursorPosition = cinemachineCamera.State.FinalPosition + cinemachineCamera.State.RawOrientation * (cinemachineCamera.transform.forward * cinemachineCamera.m_Lens.NearClipPlane);
                worldCursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * -cinemachineCamera.State.FinalPosition.z);

                Vector3 cameraPositionPreZoom = cinemachineCamera.transform.position;
                Vector3 directionFromCameraToCursor = worldCursorPosition - cameraPositionPreZoom;
                Vector3 staticnewCameraPosition = cameraPositionPreZoom + (1 - zoomRatio) * directionFromCameraToCursor;

                // Apply the new camera position
                cinemachineCamera.transform.position = new Vector3(staticnewCameraPosition.x, staticnewCameraPosition.y, cinemachineCamera.transform.position.z);
            }

        }
    }

    public void Reset()
    {
        cinemachineCamera.m_Lens.OrthographicSize = originalOrthoSize;
        cinemachineCamera.transform.position = originalPosition;
    }

    public void OpenMap()
    {
        cinemachineCamera.Priority = 30;
    }

    public void LeaveMap()
    {
        cinemachineCamera.Priority = 10;
        StartCoroutine(SafelyRepositionMap());

    }

    IEnumerator SafelyRepositionMap()
    {
        // debug that the map is closing
        Debug.Log("Closing map");
        // Wait for the specified number of seconds
        yield return new WaitForSeconds(CinemachineBrainTransitionTime);
        // Reset the map's position
        Reset();

    }


}
