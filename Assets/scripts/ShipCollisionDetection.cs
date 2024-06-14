using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCollisionDetection : MonoBehaviour
{
    private ShipEffects shipEffects;
    private bool onPlanet = false;
    void Start()
    {
        shipEffects = GetComponent<ShipEffects>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // collision with a planet
        if (collision.gameObject.CompareTag("Planet"))
        {
            StartCoroutine(SetParentAfterFrame(collision.transform));
            shipEffects.PlayLandingEffect();
            onPlanet = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // leaving a planet
        if (collision.gameObject.CompareTag("Planet") && gameObject.activeInHierarchy)
        {
            StartCoroutine(SetParentAfterFrame(null));
            onPlanet = false;
        }

    }

    IEnumerator SetParentAfterFrame(Transform newParent)
    {
        yield return new WaitForEndOfFrame();
        transform.parent = newParent;
    }

    // called by input handler
    public bool GetOnPlanet()
    {
        return onPlanet;
    }
}
