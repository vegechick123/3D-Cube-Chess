using System.Collections.Generic;
using UnityEngine;

public class TempertureManager : Manager<TempertureManager>
{
    public int globalTempature;
    [HideInInspector]
    public List<CHeatSource> heatSources;
    public int GetTempatureAt(Vector2Int location)
    {
        int res = globalTempature;
        foreach(var t in heatSources)
        {
            res += t.GetTempatureAffect(location);
        }
        return res;
    }
}
