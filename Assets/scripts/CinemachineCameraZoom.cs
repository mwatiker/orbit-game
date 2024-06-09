using UnityEngine;
using Cinemachine;

public class CinemachineCameraZoom : MonoBehaviour
{
    public CinemachineVirtualCamera cinemachineCamera;
    public float zoomSpeed = 1.0f;
    public float minOrthoSize = 5.0f;
    public float maxOrthoSize = 20.0f;

    private void Update()
    {
        ZoomCamera();
    }

    private void ZoomCamera()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            float zoomAmount = Input.mouseScrollDelta.y * zoomSpeed;
            float newOrthoSize = Mathf.Clamp(cinemachineCamera.m_Lens.OrthographicSize - zoomAmount, minOrthoSize, maxOrthoSize);

            float zoomRatio = newOrthoSize / cinemachineCamera.m_Lens.OrthographicSize;
            cinemachineCamera.m_Lens.OrthographicSize = newOrthoSize;

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
