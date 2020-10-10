using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempertureMonitor : MonoBehaviour
{
    // Start is called before the first frame update
    Text text;
    private void Awake()
    {
        text = GetComponentInChildren<Text>();
    }
    public void Init(int value)
    {
        text.text =value.ToString();
    }
}
