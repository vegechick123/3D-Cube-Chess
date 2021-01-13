using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Internal;
using System;

public class HealthBar:IChessUI
{
    // Start is called before the first frame update
   // bool bReleased;
    GChess owner;  
    public GameObject healthBarObject;
    public HealthBar(GChess chess)
    {
        
        owner =chess;
        healthBarObject = UIManager.instance.CreateHealthBar(chess);
        Refresh();
        //FloorHUD res = new FloorHUD();
    }
    public void Refresh()
    {
        healthBarObject.GetComponent<GetImage>().Refresh();
    }
    public void Release()
    {
        //eleased = true;
        GameObject.Destroy(healthBarObject);
    }
    public void Hide()
    {
        healthBarObject.SetActive(false);//maxBlood
    }
    public void Show()
    {
        healthBarObject.SetActive(true);//maxBlood
    }
}
