using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricCameraController : MonoBehaviour
{
    [Header("Target")]
    [Tooltip("Target to follow")]
    [SerializeField] private Transform target;

    [Header("Follow Settings.")]
    [Tooltip("Offset from the targets position.")]
    [SerializeField] private Vector3 offset;
    [Tooltip("The smoothing speed of the camera follow.")]
    [SerializeField] private float smoothSpeed = 0.125f;

    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    private void LateUpdate()
    {
        if (target == null)
        {
#if UNITY_EDITOR
            Debug.LogWarning("Warning: No target assigned to the IsometricCameraController.", gameObject);
#endif
            return;
        }
        
        var desiredPosition = target.position + offset;
        var smoothedPosition = Vector3.Lerp(_transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
    
    private void OnDrawGizmosSelected()
    {
        if (!target)
        {
#if UNITY_EDITOR
            UnityEditor.Handles.color = new Color(1, 0.5f, 0, 1);
            UnityEditor.Handles.Label(transform.position, "No target assigned");
#endif
            return;
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(target.position + offset, 0.1f);
    }
}
