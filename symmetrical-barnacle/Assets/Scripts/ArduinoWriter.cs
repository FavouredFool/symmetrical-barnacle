using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class ArduinoWriter : MonoBehaviour
{

    SerialPort port = new SerialPort();
    

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
    }
}
