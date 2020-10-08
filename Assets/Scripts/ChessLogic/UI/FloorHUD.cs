using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FloorHUD : IChessUI
{
    Func<Vector2Int[]> GetRange;
    GameObject[] rangeObject;
    Color color;
    bool bReleased;
    public FloorHUD (Func<Vector2Int[]> GetRange, Color color)
    {
        //FloorHUD res = new FloorHUD();
        this.GetRange = GetRange;
        this.color = color;
        UIManager.instance.eRefreshFloorHUD.AddListener(Refresh);
        Refresh();
    }
    public void Refresh()
    {
        if (bReleased)
            Debug.LogError("FloorHUD未正确释放");
        if (GetRange == null)
            return;
        if(rangeObject!=null)
            rangeObject.DestoryAll();
        rangeObject=UIManager.instance.CreateFloorHUD(GetRange(), color);
    }
    public void Release()
    {
        bReleased = true;
        if(UIManager.instance)
            UIManager.instance.eRefreshFloorHUD.RemoveListener(Refresh);
        rangeObject.DestoryAll();
    }
    public void Hide()
    {
        if (rangeObject == null)
            return;
        foreach (var t in rangeObject)
        {
            t.SetActive(false);
        }
    }
    public void Show()
    {
        if (rangeObject == null)
            return;
        foreach (var t in rangeObject)
        {
            t.SetActive(true);
        }
    }
}
