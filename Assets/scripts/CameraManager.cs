using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private bool mapOpen = true;
    private CinemachineCameraZoom mapCamera;
    // Start is called before the first frame update
    void Start()
    {
        mapCamera = FindObjectOfType<CinemachineCameraZoom>();
    }

    // called by input handler
    public void ToggleMap()
    {
        if (mapOpen)
        {
            Debug.Log("Closing map_Preface");
            mapOpen = false;
            mapCamera.LeaveMap();
        }
        else
        {
            Debug.Log("Opening map_Preface");
            mapCamera.OpenMap();
            mapOpen = true;
        }
    }


}
