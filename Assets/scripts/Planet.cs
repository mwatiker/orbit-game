using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet
{
    private GameObject planetObject;
    private float mass;
    private string name;

    public Planet(GameObject planetObj, float m, string n)
    {
        planetObject = planetObj;
        mass = m;
        name = n;
    }


    public float GetMass()
    {
        return mass;
    }

    public string GetName()
    {
        return name;
    }

    public GameObject GetObject()
    {
        return planetObject;
    }


}
