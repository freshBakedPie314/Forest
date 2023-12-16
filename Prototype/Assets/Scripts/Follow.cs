using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{

    public float shakeDuration = 0f;  
    public float shakeMagnitude = 0.7f;  

    private Vector3 initialPosition;  

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        // Camera shake logic
        if (shakeDuration > 0)
        {
            transform.position = initialPosition + Random.insideUnitSphere * shakeMagnitude;

            shakeDuration -= Time.deltaTime;
        }
        else
        {
            shakeDuration = 0f;
            transform.position = initialPosition;
        }
    }

    // Call this method to trigger camera shake
    public void ShakeCamera(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
    }
}
