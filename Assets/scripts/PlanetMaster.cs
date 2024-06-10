using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetMaster : MonoBehaviour
{
    public float bluePlanetMass;
    public float redPlanetMass;

    public float whitePlanetMass;
    public GameObject[] planets; // Array to hold all planet Gameobjects
    private Planet[] planetInfo; // Array to hold all planet information
    // Start is called before the first frame update
    void Start()
    {
        planetInfo = new Planet[1];
        planetInfo[0] = new Planet(planets[0],bluePlanetMass, "Blue Planet");
        // planetInfo[1] = new Planet(planets[1],redPlanetMass, "Red Planet");
        // planetInfo[2] = new Planet(planets[2], whitePlanetMass, "White Planet");
    }

    public Planet[] getPlanetInfo()
    {
        return planetInfo;
    }

    public GameObject[] getPlanetObjects()
    {
        return planets;
    }

}
