using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathTip : MonoBehaviour
{
    public Ship ship;


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Planet")
        {
            ship.HaltPathProjection();
        }
    }
}
