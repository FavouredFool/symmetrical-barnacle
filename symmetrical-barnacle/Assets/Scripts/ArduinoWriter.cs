using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using UnityEngine.XR;

public class ArduinoWriter : MonoBehaviour
{
    SerialPort port = new SerialPort("COM5", 9600); // sets the port for the arduino-PC Connection
    const string Byte1 = "1";
    const string Byte2 = "2";
    const string Byte3 = "3";
    const string Byte4 = "4";
    const string stopByte = "0";

    void Start()
    {
        port.Open();
        port.ReadTimeout = 1;
    }


    /// <summary>
    /// 
    /// sign ist bei einer Bewegung nach rechts "1" -> Plattform fährt links hoch.
    /// sign ist bei Bewegung geradeaus "0" -> Plattform muss runterfahren / unten bleiben.
    /// sign ist bei einer Bewegung nach links "-1" -> PLattform fährt rechts hoch.
    /// 
    /// </summary>
    /// <param name="sign"></param>

    public void WriteToArduino(int sign)
    {
        //Debug.Log(port);
        //Debug.Log($"WriteToArduino: {sign}");

        if (port.IsOpen)
        {

            switch (sign)
            {
                case 0:
                    port.Write(Byte4);
                    break;
                case 1:
                    port.Write(Byte1);
                    break;
                case -1:
                    port.Write(Byte2);
                    break;
                default:
                    Debug.LogWarning("FEHLER");
                    break;
            }
        }
    }

    public void OnApplicationQuit()
    {
        port.Write(stopByte);
    }
}
