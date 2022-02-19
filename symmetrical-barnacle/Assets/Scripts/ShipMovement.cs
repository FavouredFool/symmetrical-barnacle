using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    public int speed = 5;
    private Rigidbody rb;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void LateUpdate()
    {
        //transform.Translate(transform.forward * speed * Time.deltaTime);
        rb.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
    }

    public void UpdateShipMovement()
    {
        //transform.position += transform.forward * speed * Time.deltaTime;
    }
}


