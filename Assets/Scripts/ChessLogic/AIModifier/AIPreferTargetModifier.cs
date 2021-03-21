using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIPreferTargetModifier : MonoBehaviour
{
    public bool enable=true; 
    public virtual List<GChess> GetPreferTarget()
    {
        return new List<GChess>();
    }
    public virtual Vector2Int[] GetPreferLocation(GChess target)
    {
        return new Vector2Int[0];
    }
}
