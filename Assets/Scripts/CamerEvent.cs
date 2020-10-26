using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class CamerEvent : MonoBehaviour
{
    public UnityEvent eCameraReady;
    public void OnReady()
    {
        //Tuple<int, int> a = new Tuple<int, int>(1,2);

        eCameraReady.Invoke();
    }
}
