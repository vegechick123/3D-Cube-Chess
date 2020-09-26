using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RangeCommand : CommandTask
{
    // Start is called before the first frame update
    protected Vector2Int[] range;
    protected GameObject[] floorHUDs;
    public RangeCommand(Vector2Int[] _range, GActor obj, Delegate action,Func<int, object[],bool> _checker=null):base(obj, action,_checker)
    {
        range = _range;
    }
    public void CreateFloorHUD(Color color)
    {
        floorHUDs = UIManager.instance.CreateFloorHUD(range, color);
        eTaskEnd.AddListener(()=>
        {
            foreach (var t in floorHUDs)
            {
                GameObject.Destroy(t);
            }
        });
    }
    public void HideFloorHUD()
    {
        if (floorHUDs==null)
            return;
        foreach (var t in floorHUDs)
        {
            t.SetActive(false);
        }
    }
    public void ShowFloorHUD()
    {
        if (floorHUDs == null)
            return;
        foreach (var t in floorHUDs)
        {
            t.SetActive(true);
        }
    }
    protected override bool SetCondition<T1>(T1 pa)
    {
        if (!GridFunctionUtility.InRange(range, pa.location))
        {
            Debug.Log("OutRange");
            return false;
        }
        return true;
    }

}
