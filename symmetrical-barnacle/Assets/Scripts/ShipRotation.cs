using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipRotation : MonoBehaviour
{
    private Rigidbody rigidBody;

    public float turnDampening = 10;

    public float angleDampening = 200;
    public float angleMax = 20;

    private float angleGoal = 0;


    public void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    public void UpdateShipRotation(Quaternion shipRot, float shipRotGoal)
    {

        shipRotGoal = (360 + shipRotGoal) % 360;
        Quaternion yRotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, shipRotGoal, 0), Time.deltaTime * turnDampening);

        int sign;

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

        Quaternion zRotation = CalculateZRotation(sign, shipRotGoal);

        Vector3 fullEulerRotation = new Vector3(0, yRotation.eulerAngles.y, zRotation.eulerAngles.z);
        Debug.Log(fullEulerRotation);
        rigidBody.MoveRotation(Quaternion.Euler(fullEulerRotation));
    }

    public Quaternion CalculateZRotation(float sign, float shipRotGoal)
    {
        angleGoal = angleMax * sign;
        Debug.Log($"AngleGoal: {angleGoal}");

        return Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, -angleGoal), angleDampening * Time.deltaTime);
    }
}
