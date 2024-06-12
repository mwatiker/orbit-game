using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathTip : MonoBehaviour
{
    public Ship ship;


    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Path hit something!" + collision.gameObject.tag);
        if (collision.gameObject.tag == "Planet")
        {
            Debug.Log("Path hit a planet!");
            ship.HaltPathProjection();
        }
    }
}
