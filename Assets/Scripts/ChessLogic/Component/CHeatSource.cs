using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHeatSource : Component
{
    public int coreTempature;

    public int globalTemperture = 0;
    protected override void Awake()
    {
        base.Awake();
        TempertureManager.instance.heatSources.Add(this);
        (actor as GChess).eLocationChange.AddListener(UIManager.instance.RefreshTemperture);
    }
    public int GetTempatureAffect(Vector2Int location)
    {
        int res = Mathf.Max(coreTempature - actor.location.ManhattonDistance(location), 0) + globalTemperture;
        return res;
    }
    void OnDestroy()
    {
        if(TempertureManager.instance!=null)
            TempertureManager.instance.heatSources.Remove(this);
        if(UIManager.instance!=null)
            UIManager.instance.RefreshTemperture();
    }
}
