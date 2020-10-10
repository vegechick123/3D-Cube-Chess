using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TempertureButton : MonoBehaviour
{
    public string showTemperture;
    public string hideTemperture;
    bool bShowingTemperture;
    private Text text;
    void Awake()
    {
        text = GetComponentInChildren<Text>();    
    }
    public void Change()
    {
        bShowingTemperture = !bShowingTemperture;
        if(bShowingTemperture)
        {
            text.text = hideTemperture;
            UIManager.instance.ShowTemperture();
        }
        else
        {
            text.text = showTemperture;
            UIManager.instance.HideTemperture();
        }

    }
}
