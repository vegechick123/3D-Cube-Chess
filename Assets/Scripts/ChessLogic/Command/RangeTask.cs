using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RangeTask : InputTask
{
    protected Func<Vector2Int[]> GetRange;
    protected FloorHUD floorHUD;
    public RangeTask(Func<Vector2Int[]> GetRange, Action<GActor[]> action, int cnt, Func<int, GActor, bool> _checker = null) : base(action, cnt, _checker)
    {
        this.GetRange = GetRange;
    }
    public void CreateFloorHUD(Color color)
    {
        floorHUD = new FloorHUD(GetRange, color);
        eTaskEnd.AddListener(CleanFloorHUD);
    }
    public void CleanFloorHUD()
    {
        if (floorHUD != null)
        {
            floorHUD.Release();
            floorHUD = null;
        }
    }
    public void HideFloorHUD()
    {
        if (floorHUD == null)
            return;
        floorHUD.Hide();
    }
    public void ShowFloorHUD()
    {
        if (floorHUD == null)
            return;
        floorHUD.Show();
    }
    protected override bool SetCondition(GActor pa)
    {
        if (!GridExtensions.InRange(GetRange(), pa.location))
        {
            Debug.Log("OutRange");
            return false;
        }
        return true;
    }
}
