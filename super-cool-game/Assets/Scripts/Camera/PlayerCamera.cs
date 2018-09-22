using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

    // The object to follow
    public Transform target;
    
    // Between 0-1
    public float smoothSpeed = 0.125f;
    // Offet camera
    public Vector3 offset;

    private Vector3 velocity = Vector3.zero;

    void FixedUpdate() {
        Vector3 endPosition = target.position + offset;

        // SmoothDamp seems better than Lerp
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position,
            endPosition,
            ref velocity,
            smoothSpeed);

        transform.position = smoothedPosition;
    }
}
