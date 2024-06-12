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
        velocityText.text = velocity.ToString();
    }

    public void UpdateAngularVelocityDebug(float angularVelocity)
    {
        angularVelocityText.text = angularVelocity.ToString();
    }

    public void UpdateGravitationalForceDebug(float gravitationalForce)
    {
        gravitationalForceText.text = gravitationalForce.ToString();
    }


}
