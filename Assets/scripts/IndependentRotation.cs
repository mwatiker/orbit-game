using UnityEngine;

public class IndependentRotation : MonoBehaviour
{
    void Update()
    {
        // This will keep the rotation fixed to zero rotation relative to the world
        transform.rotation = Quaternion.identity;
    }
}
