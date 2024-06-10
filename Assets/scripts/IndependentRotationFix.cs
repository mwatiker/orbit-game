using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndependentRotationFix : MonoBehaviour
{
    void Update()
    {
        // This will keep the rotation fixed to zero rotation relative to the world
        transform.rotation = Quaternion.identity;
    }
}
