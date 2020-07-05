using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public GameObject camera;
    public Transform player;
    [SerializeField] public float forwardSpeed = 0.01f;

    public bool skipUpdate { get; private set; } = true;

    public void SetSkipUpdate(bool newValue)
    {
        skipUpdate = newValue;
    }

    void FixedUpdate()
    {
        if (skipUpdate)
        {
            return;
        }

        camera.transform.position = new Vector3(player.position.x, camera.transform.position.y, camera.transform.position.z);
        camera.transform.position += Vector3.forward * forwardSpeed;
    }

    public Vector3 GetVelocity()
    {
        if (skipUpdate)
        {
            return Vector3.zero;
        }
        return (Vector3.forward * forwardSpeed) / Time.fixedDeltaTime;
    }
}
