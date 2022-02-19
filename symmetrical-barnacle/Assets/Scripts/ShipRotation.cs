using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipRotation : MonoBehaviour
{
    private Rigidbody rigidBody;

    //public float turnDampening = 10;
    public float rotationSpeed = 15;

    public float angleDampening = 200;
    public float angleMax = 20;
    

    public float angleSpeed = 10;
    private float angleGoal = 0;


    public void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    public void UpdateShipRotation(Quaternion shipRot, float shipsign)
    {
        float shipRotGoal = transform.rotation.eulerAngles.y + shipsign * rotationSpeed * Time.deltaTime;

        Quaternion yRotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, shipRotGoal, 0), float.PositiveInfinity);

        /*
        shipRotGoal = (360 + shipRotGoal) % 360;
        Quaternion yRotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, shipRotGoal, 0), Time.deltaTime * turnDampening);

        float dist = shipRotGoal - shipRot.eulerAngles.y;

        
        if (Mathf.Abs(dist) < 180)
        {
            if (dist < -10)
            {
                sign = -1;
            } else if (dist > 10)
            {
                sign = 1;
            } else
            {
                sign = 0;
            }
        } else
        {
            if (dist < -10)
            {
                sign = 1;
            }
            else if (dist > 10)
            {
                sign = -1;
            }
            else
            {
                sign = 0;
            }
        }
        */

        Quaternion zRotation = CalculateZRotation(shipsign);

        Vector3 fullEulerRotation = new Vector3(0, yRotation.eulerAngles.y, zRotation.eulerAngles.z);
        rigidBody.MoveRotation(Quaternion.Euler(fullEulerRotation));
    }

    public Quaternion CalculateZRotation(float sign)
    {
        float angleGoal = angleMax * sign;
        //Debug.Log($"AngleGoal: {angleGoal}");


        /*
        if (sign > 0)
        {
            angleGoal = Mathf.Min(transform.rotation.eulerAngles.z + sign * angleSpeed * Time.deltaTime, angleMax);
        } else if (sign < 0)
        {
            angleGoal = Mathf.Max(transform.rotation.eulerAngles.z + sign * angleSpeed * Time.deltaTime, -angleMax);
        } else
        {
            angleGoal = 0;
        }*/
        

        //return Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, -angleGoal, 0), float.PositiveInfinity);

        return Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, -angleGoal), angleDampening * Time.deltaTime);
    }
}
