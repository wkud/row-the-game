using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    private Transform player;
    [SerializeField] public float forwardSpeed = 0.01f;
    [SerializeField] private float jumpDistance = 5;
    [SerializeField] private float movementSmothnessZ = 0.01f;
    [SerializeField] private float movementSmothnessX = 0.3f;

    public bool skipUpdate { get; private set; } = true;

    private Vector3 targetPosition;

    void Awake()
    {
        player = FindObjectOfType<Player>().transform;
        targetPosition = transform.position;
    }

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

        targetPosition.x = player.position.x;
        targetPosition.z += forwardSpeed;

        transform.position = Vector3.forward * Mathf.Lerp(transform.position.z, targetPosition.z, movementSmothnessZ)
            + Vector3.right * Mathf.Lerp(transform.position.x, targetPosition.x, movementSmothnessX)
            + Vector3.up * transform.position.y;
    }

    public Vector3 GetVelocity()
    {
        if (skipUpdate)
        {
            return Vector3.zero;
        }
        return (Vector3.forward * forwardSpeed) / Time.fixedDeltaTime;
    }

    public void JumpForward() => targetPosition.z += jumpDistance;
}
