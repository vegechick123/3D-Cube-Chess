using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateFloorHUDAtLocation : MonoBehaviour
{
    public GActor target;
    public Vector2Int targetLocation;
    public Color color=Color.yellow;
    private GameObject t;
    private void OnEnable()
    {
        Create();
    }
    private void OnDisable()
    {
        Release();
    }
    public void Create()
    {
        Vector2Int location;
        if (target != null)
            location = target.location;
        else
            location = targetLocation;
        t=UIManager.instance.CreateArrowFloorHUD(location);
    }
    public void Release()
    {
        if(t!=null)
            Destroy(t);
        t = null;
    }
}
