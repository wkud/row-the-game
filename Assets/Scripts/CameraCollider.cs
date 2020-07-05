using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollider : MonoBehaviour
{
    enum ColliderType
    {
        Unassigned,
        Front,
        Rear
    }
    [SerializeField] private ColliderType colliderType;

    private Camera camera;
    void Awake()
    {
        camera = FindObjectOfType<Camera>();
    }

    void OnTriggerEnter(Collider other)
    {
        switch(colliderType)
        {
            case ColliderType.Front:
                camera.JumpForward();
                break;
            case ColliderType.Rear:
                //todo game over
                break;
            case ColliderType.Unassigned:
                Debug.LogError("CameraCollider.colliderType is unassigned");
                break;
        }
    }
}
