using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateFloorHUDAtLocation : MonoBehaviour
{
    public GActor target;
    public Vector2Int targetLocation;
    public Color color=Color.yellow;
    private GameObject t;
    public void Create()
    {
        Vector2Int location;
        if (target != null)
            location = target.location;
        else
            location = targetLocation;
        t=UIManager.instance.CreateFloorHUD(location, color);
    }
    public void Release()
    {
        if(t!=null)
            Destroy(t);
        t = null;
    }
}
