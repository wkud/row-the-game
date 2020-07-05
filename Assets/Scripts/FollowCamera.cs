using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private Camera camera;
    void Awake() => camera = FindObjectOfType<Camera>();
    void FixedUpdate()
    {
        if (camera.skipUpdate)
            return;

        var cameraPosition = camera.transform.position;
        transform.position = new Vector3(cameraPosition.x, transform.position.y, cameraPosition.z);
    }
}
