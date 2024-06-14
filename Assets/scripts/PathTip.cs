using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathTip : MonoBehaviour
{
    public Ship ship;
    private int colliderIndex;
    private int numPoint;
    


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Planet")
        {
            ship.HaltPathProjection(colliderIndex);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Planet")
        {
            ship.AllowPathProjection(colliderIndex);
        }
    }

    public void SetColliderIndex(int index)
    {
        colliderIndex = index;
    }

    public void SetNumPoint(int np)
    {
        numPoint = np;
    }

    public int GetNumPoint()
    {
        return numPoint;
    }
}
