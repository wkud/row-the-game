using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] public float forwardSpeed;

    void FixedUpdate()
    {
        transform.position += Vector3.forward * forwardSpeed;
    }
}
