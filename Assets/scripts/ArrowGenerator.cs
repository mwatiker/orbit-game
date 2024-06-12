using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class ArrowGenerator : MonoBehaviour
{
    public float stemLength;
    public float stemWidth;
    public float tipLength;
    public float tipWidth;

    private List<Vector3> verticesList;
    private List<int> trianglesList;

    Mesh mesh;

    public GameObject tipColliderObject;

    public float circleRadius = 0;

    public float speedometerOffset = 0f;

    private float currentVelocity;
    void Start()
    {
        //make sure Mesh Renderer has a material
        mesh = new Mesh();
        this.GetComponent<MeshFilter>().mesh = mesh;

    
    }

    void Update()
    {
        GenerateArrow();
    }


    public void SetDirectionVelocity(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.Euler(0, 0, angle);

        // Calculate the new origin based on the circle's radius and the angle
        Vector3 newOrigin = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0) * circleRadius;
        this.transform.localPosition = newOrigin;  // Move the GameObject relative to its parent

        currentVelocity = direction.magnitude;
    }

    public void SetDirectionRotation(float angle)
    {
        this.transform.rotation = Quaternion.Euler(0, 0, angle);
        // Calculate the new origin based on the circle's radius and the angle
        Vector3 newOrigin = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0) * circleRadius;
        this.transform.localPosition = newOrigin;  // Move the GameObject relative to its parent
    }

    //arrow is generated starting at Vector3.zero
    //arrow is generated facing right, towards radian 0.
    // Overloaded GenerateArrow to accept an origin point
    private void GenerateArrow()
    {
        verticesList = new List<Vector3>();
        trianglesList = new List<int>();

        // Stem setup at local origin (center of GameObject)
        Vector3 stemOrigin = Vector3.zero;  // Local origin
        float stemHalfWidth = stemWidth / 2f;

        // Stem points
        verticesList.Add(stemOrigin + (stemHalfWidth * Vector3.down));
        verticesList.Add(stemOrigin + (stemHalfWidth * Vector3.up));
        verticesList.Add(verticesList[0] + (stemLength * Vector3.right));
        verticesList.Add(verticesList[1] + (stemLength * Vector3.right));

        // Stem triangles
        trianglesList.Add(0);
        trianglesList.Add(1);
        trianglesList.Add(3);
        trianglesList.Add(0);
        trianglesList.Add(3);
        trianglesList.Add(2);

        // Tip setup
        Vector3 tipOrigin = stemOrigin + (stemLength * Vector3.right);
        float tipHalfWidth = tipWidth / 2;

        // Tip points
        verticesList.Add(tipOrigin + (tipHalfWidth * Vector3.up));
        verticesList.Add(tipOrigin + (tipHalfWidth * Vector3.down));
        verticesList.Add(tipOrigin + (tipLength * Vector3.right));

        // Tip triangle
        trianglesList.Add(4);
        trianglesList.Add(6);
        trianglesList.Add(5);

        // Assign lists to mesh
        mesh.vertices = verticesList.ToArray();
        mesh.triangles = trianglesList.ToArray();
    }



}