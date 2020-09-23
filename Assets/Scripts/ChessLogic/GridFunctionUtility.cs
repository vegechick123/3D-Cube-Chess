using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

static class GridFunctionUtility
{
    public static bool InRange(Vector2Int[] range, Vector2Int location)
    {
        foreach(var p in range)
        {
            if (p == location)
                return true;
        }
        return false;
    }
}

