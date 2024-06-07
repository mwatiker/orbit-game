using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public GameObject[] planets; // Array to hold all planets
    public float G = 6.674f; // Adjusted gravitational constant for the game
    public float thrust = 5f; // Thrust power of the spaceship
    public float rotationSpeed = 100f; // Speed at which the spaceship rotates

    private Rigidbody2D rb;

    public float planetMass = 10f;

    public float boostBonus = 20f;

    public GameObject jetEffect;

    private bool onPlanet = false;

    public GameObject landingEffect;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jetEffect.SetActive(false);
        landingEffect.SetActive(false);
    }

    void Update()
    {
        HandleJetEffect();
    }

    void FixedUpdate()
    {
        ApplyGravity();
        ControlShip();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Planet"))
        {
            StartCoroutine(SetParentAfterFrame(collision.transform));
            landingEffect.SetActive(true);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Planet") && gameObject.activeInHierarchy)
        {
            StartCoroutine(SetParentAfterFrame(null));
        }
    }

    IEnumerator SetParentAfterFrame(Transform newParent)
    {
        yield return new WaitForEndOfFrame();
        transform.parent = newParent;
    }

    void ApplyGravity()
    {
        foreach (GameObject planet in planets)
        {
            Rigidbody2D planetRb = planet.GetComponent<Rigidbody2D>();

            if (planetRb != null && rb != null)
            {
                Vector2 force = GravitationalForce(planetRb, rb);
                rb.AddForce(force);
            }
        }
    }

    Vector2 GravitationalForce(Rigidbody2D planetRb, Rigidbody2D shipRb)
    {
        Vector2 distanceVector = planetRb.position - shipRb.position;
        float distance = distanceVector.magnitude;
        float forceMagnitude = (G * planetMass * shipRb.mass) / (distance * distance);
        Vector2 force = distanceVector.normalized * forceMagnitude;
        return force;
    }

    void ControlShip()
    {
        float rotationInput = Input.GetAxis("Horizontal");
        float thrustInput = Input.GetAxis("Vertical");

        // Rotate the ship based on horizontal input, if not on a planet
        if (!onPlanet){
            rb.angularVelocity = -rotationInput * rotationSpeed;
        }
        

        // Apply thrust forward based on vertical input (up key)
        if (thrustInput > 0)
        {
            Vector2 thrustDirection = transform.up * -1; // The bottom of the ship
            float currentThrust = thrust;
            if (Input.GetKey(KeyCode.Space))
            {
                currentThrust += boostBonus; // Apply boost if space is pressed
            }
            rb.AddForce(thrustDirection * currentThrust * thrustInput);
        }
    }

    void HandleJetEffect()
    {
        float thrustInput = Input.GetAxis("Vertical");

        if (thrustInput > 0)
        {
            jetEffect.SetActive(true); // Show jet effect when thrusting
        }
        else
        {
            jetEffect.SetActive(false); // Hide jet effect when not thrusting
        }
    }
}
