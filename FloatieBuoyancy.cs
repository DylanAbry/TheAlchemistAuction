using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatieBuoyancy : MonoBehaviour
{
    public float waterHeight = 0f;
    public float floatStrength = 10f;      
    public float dampening = 0.1f;

    public float waterDrag = 2f;
    public float angularDragWater = 2f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float depth = waterHeight - transform.position.y;

        if (depth > 0f)
        {
            
            rb.AddForce(Vector3.up * floatStrength * depth, ForceMode.Acceleration);

            
            rb.AddForce(-rb.velocity * waterDrag, ForceMode.Acceleration);
            rb.AddTorque(-rb.angularVelocity * angularDragWater, ForceMode.Acceleration);
        }
    }
}
