using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipEffects : MonoBehaviour
{
    private GameObject thrustEffectObject;
    private GameObject boostEffectObject;
    private GameObject landingEffectObject;
    private bool thrustEffect = false;
    private bool boostEffect = false;
    
    void Start()
    {
        thrustEffectObject = transform.Find("JetEffectParent").gameObject;
        boostEffectObject = transform.Find("BoostEffect").gameObject;
        landingEffectObject = transform.Find("LandingEffect").gameObject;
    }

    void Update()
    {
        HandleJetEffects();
    }

    private void HandleJetEffects()
    {
        float thrustInput = Input.GetAxis("Vertical");

        if (thrustEffect && boostEffect)
        {
            boostEffectObject.SetActive(true);
            thrustEffectObject.SetActive(true);
        }
        else if (thrustEffect)
        {
            thrustEffectObject.SetActive(true);
            boostEffectObject.SetActive(false);
        }
        else
        {
            boostEffectObject.SetActive(false);
            thrustEffectObject.SetActive(false);
        }
    }

    // called by input handler
    public void StartThrustEffect()
    {
        thrustEffect = true;
    }

    // called by input handler
    public void StopThrustEffect()
    {
        thrustEffect = false;
    }

    // called by input handler
    public void StartBoostEffect()
    {
        boostEffect = true;
    }

    // called by input handler
    public void StopBoostEffect()
    {
        boostEffect = false;
    }

    // called by ShipCollisionDetection
    public void PlayLandingEffect()
    {
        landingEffectObject.SetActive(true);
    }

}
