using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ArduinoWriter : MonoBehaviour
{
    
    /// <summary>
    /// 
    /// sign ist bei einer Bewegung nach rechts "1" -> Plattform f�hrt links hoch.
    /// sign ist bei Bewegung geradeaus "0" -> Plattform muss runterfahren / unten bleiben.
    /// sign ist bei einer Bewegung nach links "-1" -> PLattform f�hrt rechts hoch.
    /// 
    /// </summary>
    /// <param name="sign"></param>

    public void WriteToArduino(int sign)
    {

    }
}
