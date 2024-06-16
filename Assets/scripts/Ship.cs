using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; // Add this for LINQ support


public class Ship : MonoBehaviour
{
    public float G = 6.674f; // Adjusted gravitational constant for the game


    private Rigidbody2D rb;


    private PlanetMaster planetMaster;

    private Planet[] planetInfo;

    public ArrowGenerator rotationalNavArrow;

    public CinemachineCameraZoom mapCameraZoom;

    private float planetMass;

    public float orbitRadius = 50f;

    public LineRenderer orbitRenderer; // Assign this in the editor or via script
    public int numPathPoints = 100; // Number of points in the projected ORBIT path


    private bool updatingOrbit = false;




    public GameObject colliderPrefab; // Prefab for the colliders

    public int numberOfColliders = 10; // Number of colliders to distribute along the path
    private GameObject[] colliders; // Array to hold the collider instances

    private float currentGravitationalForce;

    private InGameDebug ingameDebug;

    private float adjustedVelocity;

    public float gravityDebugMultiplier = 1000f;

    private int pathCollisionsNumber = 0;

    private int numPoints;

    public int flightPathSpeed = 1;

    private ShipCollisionDetection shipCollisionDetection;

    private bool pathInterrupted = false;







    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        planetMaster = GameObject.FindObjectOfType<PlanetMaster>();
        planetInfo = planetMaster.getPlanetInfo();
        orbitRenderer.positionCount = numPathPoints;
        ingameDebug = GetComponent<InGameDebug>();

        
        
        
        shipCollisionDetection = GetComponent<ShipCollisionDetection>();

    }
    void Update()
    {

        // if on a planet and not thrusting, lock the ship in place

        if (Input.GetKeyDown(KeyCode.R)) // Key to set orbit
        {
            GameObject planetGO = planetInfo[0].GetObject(); // Get the first planet GameObject
            SetOrbitAndTeleport(planetGO, orbitRadius); // Teleport to orbit
            updatingOrbit = true;
            pathInterrupted = false;
            for (int i = 0; i < numberOfColliders; i++)
            {
                colliders[i].SetActive(true);
            }
        }


        // if the velocity is reasonable, update the flight path projection
        adjustedVelocity = (Mathf.Round(rb.velocity.magnitude * 10));
        if (adjustedVelocity > 2f && rb.velocity.magnitude < 10000f)
        {

            //UpdateFlightPathProjection();

        }


        DebugDiagostics();




    }




    void FixedUpdate()
    {
        if (!shipCollisionDetection.GetOnPlanet())
        {
            ApplyGravity();
        }
        else
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0;
        }

    




    }

    private void DebugDiagostics()
    {
        ingameDebug.UpdateVelocityDebug(adjustedVelocity);
        ingameDebug.UpdateAngularVelocityDebug(rb.angularVelocity);
        ingameDebug.UpdateGravitationalForceDebug(currentGravitationalForce * gravityDebugMultiplier);


    }



    

    

    




    





    

    private void SetOrbitAndTeleport(GameObject planetGO, float desiredOrbitRadius)
    {
        Rigidbody2D planetRb = planetGO.GetComponent<Rigidbody2D>();
        // Calculate the position for the ship along the desired orbit radius
        Vector2 orbitPosition = new Vector2(desiredOrbitRadius, 0); // Choose an initial position, e.g., (radius, 0)
        orbitPosition = Quaternion.Euler(0, 0, Random.Range(0, 360)) * orbitPosition; // Rotate to a random angle around the planet
        orbitPosition += planetRb.position; // Adjust position relative to the planet's position

        // Calculate the orbital speed and direction
        float orbitSpeed = Mathf.Sqrt(G * planetInfo[0].GetMass() / desiredOrbitRadius);
        Vector2 orbitDirection = new Vector2(-orbitPosition.y, orbitPosition.x).normalized; // Perpendicular to the radius vector

        // Teleport the ship to the calculated position
        rb.position = orbitPosition;
        rb.velocity = orbitDirection * orbitSpeed; // Set velocity tangent to the orbit
    }


    private Vector2 GravitationalForce(Rigidbody2D planetRb, Rigidbody2D shipRb, float planetMass)
    {
        Vector2 distanceVector = planetRb.position - shipRb.position;
        float distance = distanceVector.magnitude;
        float forceMagnitude = (G * planetMass * shipRb.mass) / (distance * distance);
        Vector2 force = distanceVector.normalized * forceMagnitude;
        return force;
    }






    private void ApplyGravity()
    {

        foreach (Planet planet in planetInfo)
        {
            GameObject planetGO = planet.GetObject();
            Rigidbody2D planetRb = planetGO.GetComponent<Rigidbody2D>();

            if (planetRb != null && rb != null)
            {
                planetMass = planet.GetMass();
                Vector2 force = GravitationalForce(planetRb, rb, planetMass);
                currentGravitationalForce = force.magnitude;
                rb.AddForce(force);
            }
        }
    }



    

    

    


}
