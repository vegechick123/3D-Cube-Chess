using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar:IChessUI
{
    // Start is called before the first frame update
    
    bool bReleased;
    GChess owner;
    GameObject[] blood;
    public HealthBar(GChess chess)
    {
        owner=chess;
        for (int i = 0; i < owner.health; i++)
        {
            //GameObject.Instantiate()
        }

       Refresh();
        //FloorHUD res = new FloorHUD();
    }
    public void Refresh()
    {
        int health = owner.curHealth;
        for (int i = 0; i < health; i++)
            blood[i].SetActive(false);
    }
    public void Release()
    {
    }
    public void Hide()
    {
    }
    public void Show()
    {

    }
}
