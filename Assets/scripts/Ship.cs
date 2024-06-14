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

    public ArrowGenerator velocityNavArrow;

    public GameObject navMapVisual;

    public LineRenderer pathRenderer;
    public int pathLength = 100; // Number of points in the path
    public float pathPointInterval = 0.1f; // Time interval between points

    public CinemachineCameraZoom mapCameraZoom;

    private float planetMass;

    public float orbitRadius = 50f;

    public LineRenderer orbitRenderer; // Assign this in the editor or via script
    public int numPathPoints = 100; // Number of points in the projected path


    private bool updatingOrbit = false;

    public GameObject flightPath;

    public int startingPoint = 0;


    private bool pathInterrupted = false;

    private int interruptIndex = 0;  // Index where the path should be interrupted

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







    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        planetMaster = GameObject.FindObjectOfType<PlanetMaster>();
        planetInfo = planetMaster.getPlanetInfo();
        navMapVisual.SetActive(false);
        // pathRenderer.positionCount = pathLength;
        // pathRenderer.enabled = true;
        orbitRenderer.positionCount = numPathPoints;
        // Find the InGameDebug component that is attached to this GameObject
        ingameDebug = GetComponent<InGameDebug>();

        colliders = new GameObject[numberOfColliders];
        for (int i = 0; i < numberOfColliders; i++)
        {
            colliders[i] = Instantiate(colliderPrefab, Vector3.zero, Quaternion.identity);
            colliders[i].GetComponent<PathTip>().SetColliderIndex(i);
        }
        SetCollidersNumPoint();
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

            UpdateFlightPathProjection();

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



    public void UpdateFlightPathProjection()
    {
        if (pathInterrupted)
        {
            numPoints = colliders[interruptIndex].GetComponent<PathTip>().GetNumPoint();
        }
        else
        {
            numPoints = pathLength;
        }
        float timeStep = pathPointInterval;
        Vector2[] pathPoints = new Vector2[numPoints];
        Vector2 simulatedPosition = rb.position;
        Vector2 simulatedVelocity = rb.velocity;

        pathRenderer.positionCount = numPoints;

        for (int i = startingPoint; i < numPoints; i++)
        {
            Vector2 force = Vector2.zero;
            foreach (Planet planet in planetInfo)
            {
                GameObject planetGO = planet.GetObject();
                Rigidbody2D planetRb = planetGO.GetComponent<Rigidbody2D>();
                force += CalculateGravitationalForce(simulatedPosition, planetRb.position, planet.GetMass(), rb.mass);
            }

            Vector2 acceleration = force / rb.mass;
            simulatedVelocity += acceleration * timeStep;
            simulatedPosition += simulatedVelocity * timeStep;

            pathPoints[i] = simulatedPosition;
        }

        Vector3[] pathPositions = pathPoints.Select(p => new Vector3(p.x, p.y, 0)).ToArray();
        pathRenderer.SetPositions(pathPositions);

        UpdateCollidersPosition(pathPoints); // Adjust this method if needed to support partial path

        pathLength = pathLength + flightPathSpeed;
    }

    private void UpdateCollidersPosition(Vector2[] pathPoints)
    {
        int step = pathLength / numberOfColliders;
        for (int i = 0; i < numberOfColliders; i++)
        {
            int index = i * step;
            if (index < pathPoints.Length)
            {
                colliders[i].transform.position = new Vector3(pathPoints[index].x, pathPoints[index].y, 0);
            }
        }
    }

    private void SetCollidersNumPoint()
    {
        int step = pathLength / numberOfColliders;
        for (int i = 0; i < numberOfColliders; i++)
        {
            int index = i * step;
            colliders[i].GetComponent<PathTip>().SetNumPoint(index);
        }
    }




    





    Vector2 CalculateGravitationalForce(Vector2 shipPos, Vector2 planetPos, float planetMass, float shipMass)
    {
        Vector2 distanceVec = planetPos - shipPos;
        float distance = distanceVec.magnitude;
        float epsilon = 0.1f; // Prevent division by extremely small distances
        distance = Mathf.Max(distance, epsilon);
        float forceMagnitude = (G * planetMass * shipMass) / (distance * distance);
        return distanceVec.normalized * forceMagnitude;
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



    

    

    public void HaltPathProjection(int index)
    {
        pathInterrupted = true;
        interruptIndex = index;
    }

    public void AllowPathProjection(int index)
    {
        Debug.Log("Allowing path projection" + index);
        if (index == interruptIndex)
        {
            interruptIndex = index + 1;
        }
    }


}
