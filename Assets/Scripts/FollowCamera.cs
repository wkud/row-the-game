using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private Camera camera;
    void Awake() => camera = FindObjectOfType<Camera>();
    void FixedUpdate() => transform.position += Vector3.forward * camera.forwardSpeed;
}
