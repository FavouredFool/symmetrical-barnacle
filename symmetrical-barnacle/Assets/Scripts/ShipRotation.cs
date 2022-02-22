using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipRotation : MonoBehaviour
{
    private Rigidbody rigidBody;
    private ArduinoWriter arduinoWriter;

    //public float turnDampening = 10;
    public float rotationSpeed = 15;

    public float angleDampening = 200;
    public float angleMax = 20;


    public void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        arduinoWriter = GetComponent<ArduinoWriter>();
    }

    public void UpdateShipRotation(Quaternion shipRot, int shipsign)
    {
        float shipRotGoal = transform.rotation.eulerAngles.y + shipsign * rotationSpeed * Time.deltaTime;

        Quaternion yRotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, shipRotGoal, 0), float.PositiveInfinity);

        Quaternion zRotation = CalculateZRotation(shipsign);

        Vector3 fullEulerRotation = new Vector3(0, yRotation.eulerAngles.y, zRotation.eulerAngles.z);
        rigidBody.MoveRotation(Quaternion.Euler(fullEulerRotation));

        arduinoWriter.WriteToArduino(shipsign);


    }

    public Quaternion CalculateZRotation(float sign)
    {
        float angleGoal = angleMax * sign;

        return Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, -angleGoal), angleDampening * Time.deltaTime);
    }
}
