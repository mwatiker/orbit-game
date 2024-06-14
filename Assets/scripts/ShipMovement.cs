using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    public float rotationSpeed = 100f;

    public float thrust = 5f;

    public float boostBonus = 20f;

    private Vector2 thrustDirection;

    private float currentThrust;

    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // called by input handler
    public void Rotate(float rotationInput)
    {
        rb.angularVelocity = -rotationInput * rotationSpeed;
    }

    // called by input handler
    public void ApplyThrust(float thrustInput, bool boostingInput)
    {
        thrustDirection = transform.up * -1; // The bottom of the ship
        currentThrust = thrust;
        if (boostingInput)
        {
            currentThrust += boostBonus; // Apply boost if space is pressed
        }
        rb.AddForce(thrustDirection * currentThrust * thrustInput);
    }
}
 