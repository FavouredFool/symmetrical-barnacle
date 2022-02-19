using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class SteeringWheelControl : MonoBehaviour
{
    // Right Hand
    public GameObject rightHand;
    private Transform rightHandOriginalParent;
    private bool rightHandOnWheel = false;

    // Right Hand
    public GameObject leftHand;
    private Transform leftHandOriginalParent;
    private bool leftHandOnWheel = false;

    public GameObject baseParent;

    public Transform[] snapPositions;

    public float currentSteeringWheelRotation = 0;

    public Transform directionalObject;

    public GameObject vehicle;
    

    private bool rightTriggerPressed = false;
    private bool leftTriggerPressed = false;

    private bool rightTriggerWasPressed = false;
    private bool leftTriggerWasPressed = false;

    private bool rightTriggerUp = false;
    private bool leftTriggerUp = false;

    private bool rightTriggerDown = false;
    private bool leftTriggerDown = false;

    InputDevice rightController;
    InputDevice leftController;

    public ShipRotation shipRotation;
    public ShipMovement shipMovement;
    

    private void Start()
    {
        TryInitialize();

    }

    public void TryInitialize()
    {
        List<InputDevice> rightDevices = new List<InputDevice>();
        List<InputDevice> leftDevices = new List<InputDevice>();

        InputDeviceCharacteristics rightControllerCharacteristics = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        InputDeviceCharacteristics leftControllerCharacteristics = InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;

        InputDevices.GetDevicesWithCharacteristics(rightControllerCharacteristics, rightDevices);
        InputDevices.GetDevicesWithCharacteristics(leftControllerCharacteristics, leftDevices);
        if (rightDevices.Count > 0)
        {
            rightController = rightDevices[0];
        }
        if (leftDevices.Count > 0)
        {
            leftController = leftDevices[0];
        }
    }

    private void Update()
    {
        if (!rightController.isValid || !leftController.isValid)
        {
            TryInitialize();
        }

        HandleInput();

        ReleaseHandsFromWheel();

        ConvertHandRotationToSteeringWheelRotation();

        TurnVehicle();

        currentSteeringWheelRotation = -transform.rotation.eulerAngles.z;
    }

    private void HandleInput()
    {
        rightTriggerWasPressed = rightTriggerPressed;
        leftTriggerWasPressed = leftTriggerPressed;

        rightController.TryGetFeatureValue(CommonUsages.trigger, out float rightTrigger);
        leftController.TryGetFeatureValue(CommonUsages.trigger, out float leftTrigger);

        if (rightTrigger > 0.1f)
        {
            rightTriggerPressed = true;
        } else
        {
            rightTriggerPressed = false;
        }

        if (leftTrigger > 0.1f)
        {
            leftTriggerPressed = true;
        }
        else
        {
            leftTriggerPressed = false;
        }

        if (!rightTriggerPressed && rightTriggerWasPressed)
        {
            rightTriggerUp = true;
        } else
        {
            rightTriggerUp = false;
        }

        if (!leftTriggerPressed && leftTriggerWasPressed)
        {
           leftTriggerUp = true;
        }
        else
        {
            leftTriggerUp = false;
        }


        if (rightTriggerPressed && !rightTriggerWasPressed)
        {
            rightTriggerDown = true;
        } else
        {
            rightTriggerDown = false;
        }

        if (leftTriggerPressed && !leftTriggerWasPressed)
        {
            leftTriggerDown = true;
        } else
        {
            leftTriggerDown = false;
        }

    }



    private void TurnVehicle()
    {
        
        var zTurn = transform.localRotation.eulerAngles.z;
        Debug.Log(zTurn);

        float dist = zTurn;

        int sign;

        if (Mathf.Abs(dist) < 180)
        {
            if (dist > 10)
            {
                sign = -1;
            }
            else
            {
                sign = 0;
            }
        }
        else
        {
            if (dist < 350)
            {
                sign = 1;
            }
            else
            {
                sign = 0;
            }
        }

        shipRotation.UpdateShipRotation(vehicle.transform.rotation, sign);

        shipMovement.UpdateShipMovement();

    }

    private void ConvertHandRotationToSteeringWheelRotation()
    {
        
        if (rightHandOnWheel && !leftHandOnWheel)
        {
            Quaternion newRot = Quaternion.Euler(0, vehicle.transform.rotation.eulerAngles.y, rightHandOriginalParent.transform.rotation.eulerAngles.z);
            //transform.RotateAround(directionalObject.transform.position, directionalObject.transform.forward, rightHandOriginalParent.transform.rotation.eulerAngles.z * Time.deltaTime);
            directionalObject.rotation = newRot;

            transform.parent = directionalObject;
        } else if (!rightHandOnWheel && leftHandOnWheel)
        {
            Quaternion newRot = Quaternion.Euler(0, vehicle.transform.rotation.eulerAngles.y, leftHandOriginalParent.transform.rotation.eulerAngles.z);
            directionalObject.rotation = newRot;
            transform.parent = directionalObject;
        } else if(rightHandOnWheel && leftHandOnWheel)
        {
            Quaternion newRotLeft = Quaternion.Euler(0, vehicle.transform.rotation.eulerAngles.y, leftHandOriginalParent.transform.rotation.eulerAngles.z);
            Quaternion newRotRight = Quaternion.Euler(0, vehicle.transform.rotation.eulerAngles.y, rightHandOriginalParent.transform.rotation.eulerAngles.z);
            Quaternion finalRot = Quaternion.Slerp(newRotLeft, newRotRight, 1.0f / 2.0f);
            directionalObject.rotation = finalRot;
            transform.parent = directionalObject;
        }
        
    }

    private void ReleaseHandsFromWheel()
    {
        if (rightHandOnWheel && rightTriggerUp)
        {
            rightHand.transform.parent = rightHandOriginalParent;
            rightHand.transform.position = rightHandOriginalParent.position;
            rightHand.transform.rotation = rightHandOriginalParent.rotation;
            rightHandOnWheel = false;
        }
        if (leftHandOnWheel && leftTriggerUp)
        {
            leftHand.transform.parent = leftHandOriginalParent;
            leftHand.transform.position = leftHandOriginalParent.position;
            leftHand.transform.rotation = leftHandOriginalParent.rotation;
            leftHandOnWheel = false;
        }

        if (!leftHandOnWheel && !rightHandOnWheel)
        {
            transform.parent = baseParent.transform;
        }
    }

    private void PlaceHandOnWheel(ref GameObject hand, ref Transform originalParent, ref bool handOnWheel)
    {
        var shortestDistance = Vector3.Distance(snapPositions[0].position, hand.transform.position);
        var bestSnap = snapPositions[0];

        foreach(var snapPosition in snapPositions)
        {
            if (snapPosition.childCount == 0)
            {
                var distance = Vector3.Distance(snapPosition.position, hand.transform.position);
                
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    bestSnap = snapPosition;
                }
            }
        }

        originalParent = hand.transform.parent;

        hand.transform.parent = bestSnap.transform;
        hand.transform.position = bestSnap.transform.position;

        handOnWheel = true;
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("PlayerHand"))
        {
            if (!rightHandOnWheel && rightTriggerDown) {
                PlaceHandOnWheel(ref rightHand, ref rightHandOriginalParent, ref rightHandOnWheel);
            }
            if (!leftHandOnWheel && leftTriggerDown)
            {
                PlaceHandOnWheel(ref leftHand, ref leftHandOriginalParent, ref leftHandOnWheel);
            }
        }
        
    }

}
