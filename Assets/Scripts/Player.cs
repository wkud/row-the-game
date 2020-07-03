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
    
    public AudioSource crashAudio;
    public AudioSource swingAudio;

    Animator m_Animator;
    Rigidbody m_Rigidbody;
    Vector3 m_Movement = Vector3.zero;
    Quaternion m_Rotation = Quaternion.identity;
    Quaternion m_RotationTowards = Quaternion.identity;
    bool m_RowPerformed = false;
    float m_RotationSuppression = 1.0f; // higher suppression when m_RotationSuppression is smaller

    Vector3 m_CurrentPosition;
    Vector3 m_LastPosition;

    bool hasCrashed = false;
    bool skipUpdate = true;

    void OnCollisionEnter(Collision other)
    {
        m_Animator.SetBool("crashed", true);
        m_Animator.SetTrigger("crash");
        hasCrashed = true;
        crashAudio.Play();
    }

    public bool CrashOccured()
    {
        return hasCrashed;
    }

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Animator = gameObject.GetComponent<Animator>();

        m_LastPosition = transform.position;
        m_CurrentPosition = transform.position;
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
            if (horizontal > 0)
            {
                m_Animator.SetTrigger("swingRight");
            } else
            {
                m_Animator.SetTrigger("swingLeft");
            }
            swingAudio.Play();
        }

        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement);
        m_Movement *= moveSuppressionFactor;

        Vector3 desiredForward = Vector3.RotateTowards(
            transform.forward, m_RotationTowards * transform.forward,
            rotationSpeed * m_RotationSuppression * Time.fixedDeltaTime, 0f);
        m_Rotation = Quaternion.LookRotation(desiredForward);
        m_Rigidbody.MoveRotation(m_Rotation);
        m_RotationSuppression *= rotationSuppressionFactor;

        m_LastPosition = m_CurrentPosition;
        m_CurrentPosition = transform.position;
    }

    public Vector3 GetVelocity()
    {
        return (m_CurrentPosition - m_LastPosition) / Time.fixedDeltaTime;
    }
}
