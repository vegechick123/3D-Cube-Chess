using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RangeCommand : CommandTask
{
    protected Func<Vector2Int[]> GetRange;
    protected FloorHUD floorHUD;
    public RangeCommand(Func<Vector2Int[]> GetRange, GActor obj, Delegate action, Func<int, object[], bool> _checker = null) : base(obj, action, _checker)
    {
        this.GetRange=GetRange;
    }
    public void CreateFloorHUD(Color color)
    {
        floorHUD = new FloorHUD(GetRange, color);
        eTaskEnd.AddListener(CleanFloorHUD);
    }
    public void CleanFloorHUD()
    {
        if(floorHUD!=null)
        {
            floorHUD.Release();
            floorHUD = null;
        }
    }
    public void HideFloorHUD()
    {
        if (floorHUD==null)
            return;
        floorHUD.Hide();
    }
    public void ShowFloorHUD()
    {
        if (floorHUD == null)
            return;
        floorHUD.Show();
    }
    protected override bool SetCondition<T1>(T1 pa)
    {
        if (!GridExtensions.InRange(GetRange(), pa.location))
        {
            Debug.Log("OutRange");
            return false;
        }
        return true;
    }
    public override void Abort()
    {
        base.Abort();
    }

}
