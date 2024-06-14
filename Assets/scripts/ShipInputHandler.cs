using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipInputHandler : MonoBehaviour
{

    
    private float rotationInput;
    private float thrustInput;
    private bool boostingInput;
    private ShipMovement shipMovement;
    private ShipEffects shipEffects;
    private ShipCollisionDetection shipCollisionDetection;
    private CameraManager cameraManager;

    

    
    void Start()
    {
        shipMovement = GetComponent<ShipMovement>();
        shipEffects = GetComponent<ShipEffects>();  
        shipCollisionDetection = GetComponent<ShipCollisionDetection>();
        cameraManager = FindObjectOfType<CameraManager>();
    }



    void Update()
    {
        ControlShip();
        HandleMapInput();

    }
    private void ControlShip()
    {
        rotationInput = Input.GetAxis("Horizontal");
        thrustInput = Input.GetAxis("Vertical");
        boostingInput = Input.GetKey(KeyCode.Space);

        // Rotate the ship based on horizontal input, if not on a planet
        if ((rotationInput > 0 || rotationInput < 0) && !shipCollisionDetection.GetOnPlanet())
        {
            shipMovement.Rotate(rotationInput);
        }


        // Apply thrust forward based on vertical input (up key)
        if (thrustInput > 0)
        {
            shipMovement.ApplyThrust(thrustInput, boostingInput);
            shipEffects.StartThrustEffect();
            if (boostingInput)
            {
                shipEffects.StartBoostEffect();
            }
            else
            {
                shipEffects.StopBoostEffect();
            }
        }
        else
        {
            shipEffects.StopThrustEffect();
            shipEffects.StopBoostEffect();
        }
    }

    private void HandleMapInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            cameraManager.ToggleMap();
        }
    }
}
