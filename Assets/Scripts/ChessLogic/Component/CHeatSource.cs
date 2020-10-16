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
    }
}
