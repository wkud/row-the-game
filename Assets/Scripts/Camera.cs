using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public GameObject camera;
    [SerializeField] public float forwardSpeed = 0.01f;

    void FixedUpdate()
    {
        camera.transform.position += Vector3.forward * forwardSpeed;
    }

    public Vector3 GetVelocity()
    {
        return (Vector3.forward * forwardSpeed) / Time.fixedDeltaTime;
    }
}
