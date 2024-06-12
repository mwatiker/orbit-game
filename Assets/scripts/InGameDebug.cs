using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameDebug : MonoBehaviour
{
    public TMPro.TextMeshProUGUI velocityText;
    public TMPro.TextMeshProUGUI angularVelocityText;
    public TMPro.TextMeshProUGUI gravitationalForceText;

    public void UpdateVelocityDebug(float velocity)
    {
        // keep to 2 decimal places
        velocity = (float)System.Math.Round(velocity, 2);
        velocityText.text = velocity.ToString();
    }

    public void UpdateAngularVelocityDebug(float angularVelocity)
    {
        // keep to 2 decimal places
        angularVelocity = (float)System.Math.Round(angularVelocity, 2);
        angularVelocityText.text = angularVelocity.ToString();
    }

    public void UpdateGravitationalForceDebug(float gravitationalForce)
    {
        // keep to 2 decimal places
        gravitationalForce = (float)System.Math.Round(gravitationalForce, 2);
        gravitationalForceText.text = gravitationalForce.ToString();
    }


}
