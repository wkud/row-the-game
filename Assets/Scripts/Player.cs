using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor.Rendering;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class Player : MonoBehaviour
{
    public float moveSpeed = 0.05f; // in (0, inf)
    public float moveSuppressionFactor = 0.95f; // in (0, 1), higher value results in smoother suppression
    public float rotationSpeed = 1.0f; // in (0, inf)
    public float rotationSuppressionFactor = 0.95f;  // in (0, 1), higher value results in smoother suppression

    Rigidbody m_Rigidbody;
    Vector3 m_Movement = Vector3.zero;
    Quaternion m_Rotation = Quaternion.identity;
    Quaternion m_RotationTowards = Quaternion.identity;
    bool m_RowPerformed = false;
    float m_RotationSuppression = 1.0f; // higher suppression when m_RotationSuppression is smaller

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");

        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        if (!hasHorizontalInput)
        {
            m_RowPerformed = false;
        }

        if (!m_RowPerformed && hasHorizontalInput)
        {
            m_Movement = transform.forward * moveSpeed;

            m_RotationTowards = (horizontal < 0) ? Quaternion.Euler(0, 45, 0) : Quaternion.Euler(0, -45, 0);
            m_RotationSuppression = 1.0f;

            m_RowPerformed = true;
        }

        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement);
        m_Movement *= moveSuppressionFactor;

        Vector3 desiredForward = Vector3.RotateTowards(
            transform.forward, m_RotationTowards * transform.forward,
            rotationSpeed * m_RotationSuppression * Time.fixedDeltaTime, 0f);
        m_Rotation = Quaternion.LookRotation(desiredForward);
        m_Rigidbody.MoveRotation(m_Rotation);
        m_RotationSuppression *= rotationSuppressionFactor;
    }
}
