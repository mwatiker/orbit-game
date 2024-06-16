using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipFlightProjection : MonoBehaviour
{
    // public LineRenderer pathRenderer;
    // public int pathLength = 100; // Number of points in the path
    // public float pathPointInterval = 0.1f; // Time interval between points
    // private int interruptIndex = 0;  // Index where the path should be interrupted
    // private bool pathInterrupted = false;

    // void Start()
    // {
    //     colliders = new GameObject[numberOfColliders];
    //     for (int i = 0; i < numberOfColliders; i++)
    //     {
    //         colliders[i] = Instantiate(colliderPrefab, Vector3.zero, Quaternion.identity);
    //         colliders[i].GetComponent<PathTip>().SetColliderIndex(i);
    //     }
    //     SetCollidersNumPoint();
    // }
    

    // Vector2 CalculateGravitationalForce(Vector2 shipPos, Vector2 planetPos, float planetMass, float shipMass)
    // {
    //     Vector2 distanceVec = planetPos - shipPos;
    //     float distance = distanceVec.magnitude;
    //     float epsilon = 0.1f; // Prevent division by extremely small distances
    //     distance = Mathf.Max(distance, epsilon);
    //     float forceMagnitude = (G * planetMass * shipMass) / (distance * distance);
    //     return distanceVec.normalized * forceMagnitude;
    // }

    // public void UpdateFlightPathProjection()
    // {
    //     if (pathInterrupted)
    //     {
    //         numPoints = colliders[interruptIndex].GetComponent<PathTip>().GetNumPoint();
    //     }
    //     else
    //     {
    //         numPoints = pathLength;
    //     }
    //     float timeStep = pathPointInterval;
    //     Vector2[] pathPoints = new Vector2[numPoints];
    //     Vector2 simulatedPosition = rb.position;
    //     Vector2 simulatedVelocity = rb.velocity;

    //     pathRenderer.positionCount = numPoints;

    //     for (int i = 0; i < numPoints; i++)
    //     {
    //         Vector2 force = Vector2.zero;
    //         foreach (Planet planet in planetInfo)
    //         {
    //             GameObject planetGO = planet.GetObject();
    //             Rigidbody2D planetRb = planetGO.GetComponent<Rigidbody2D>();
    //             force += CalculateGravitationalForce(simulatedPosition, planetRb.position, planet.GetMass(), rb.mass);
    //         }

    //         Vector2 acceleration = force / rb.mass;
    //         simulatedVelocity += acceleration * timeStep;
    //         simulatedPosition += simulatedVelocity * timeStep;

    //         pathPoints[i] = simulatedPosition;
    //     }

    //     Vector3[] pathPositions = pathPoints.Select(p => new Vector3(p.x, p.y, 0)).ToArray();
    //     pathRenderer.SetPositions(pathPositions);

    //     UpdateCollidersPosition(pathPoints); // Adjust this method if needed to support partial path

    //     pathLength = pathLength + flightPathSpeed;
    // }

    // private void UpdateCollidersPosition(Vector2[] pathPoints)
    // {
    //     int step = pathLength / numberOfColliders;
    //     for (int i = 0; i < numberOfColliders; i++)
    //     {
    //         int index = i * step;
    //         if (index < pathPoints.Length)
    //         {
    //             colliders[i].transform.position = new Vector3(pathPoints[index].x, pathPoints[index].y, 0);
    //         }
    //     }
    // }

    // private void SetCollidersNumPoint()
    // {
    //     int step = pathLength / numberOfColliders;
    //     for (int i = 0; i < numberOfColliders; i++)
    //     {
    //         int index = i * step;
    //         colliders[i].GetComponent<PathTip>().SetNumPoint(index);
    //     }
    // }

    // public void HaltPathProjection(int index)
    // {
    //     pathInterrupted = true;
    //     interruptIndex = index;
    // }

    // public void AllowPathProjection(int index)
    // {
    //     Debug.Log("Allowing path projection" + index);
    //     if (index == interruptIndex)
    //     {
    //         interruptIndex = index + 1;
    //     }
    // }
}
