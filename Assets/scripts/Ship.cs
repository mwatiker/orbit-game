using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour {
    public GameObject[] planets; // Array to hold all planets
    public float G = 6.674f; // Adjusted gravitational constant for the game
    public float thrust = 5f; // Thrust power of the spaceship

    private Rigidbody2D rb;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        ApplyGravity();
        ControlShip();
    }

    void ApplyGravity() {
        foreach (GameObject planet in planets) {
            Rigidbody2D planetRb = planet.GetComponent<Rigidbody2D>();

            if (planetRb != null && rb != null) {
                Vector2 force = GravitationalForce(planetRb, rb);
                rb.AddForce(force);
            }
        }
    }

    Vector2 GravitationalForce(Rigidbody2D planetRb, Rigidbody2D shipRb) {
        Vector2 distanceVector = planetRb.position - shipRb.position;
        float distance = distanceVector.magnitude;
        float forceMagnitude = (G * planetRb.mass * shipRb.mass) / (distance * distance);
        Vector2 force = distanceVector.normalized * forceMagnitude;
        return force;
    }

    void ControlShip() {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        rb.AddForce(movement * thrust);
    }
}
