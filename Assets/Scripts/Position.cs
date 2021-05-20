using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Position : MonoBehaviour
{
    float wireSphereRadius = 1f;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, wireSphereRadius);
    }
}
