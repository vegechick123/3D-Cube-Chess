using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar:IChessUI
{
    // Start is called before the first frame update
    
    bool bReleased;
    public HealthBar(GChess chess)
    {
        int t =chess.health;
        //FloorHUD res = new FloorHUD();
    }
    public void Refresh()
    {
       
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
