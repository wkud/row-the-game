using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] private float flowSpeed;

    void FixedUpdate()
    {
        transform.position += Vector3.forward * flowSpeed;
    }
}
