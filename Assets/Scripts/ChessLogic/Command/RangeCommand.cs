using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RangeCommand<T> : CommandTask<T>
{
    // Start is called before the first frame update
    protected Vector2Int[] range;
    public RangeCommand(Vector2Int[] _range, GActor obj, T action):base(obj, action)
    {
        range = _range;
    }
    protected override bool SetCondition<T1>(T1 pa)
    {
        if (!GridFunctionUtility.InRange(range, pa.location))
        {
            Debug.Log("OutRange");
            return false;
        }
        Debug.Log("InRange");
        return true;
    }

}
