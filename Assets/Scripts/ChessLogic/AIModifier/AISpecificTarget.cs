using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpecificTarget : AIPreferTargetModifier
{
    public int effectiveRoundCount = -1;
    public List<int> speceficTargetID;
    public List<Vector2Int> speceficLocationOffset;
    public override List<GChess> GetPreferTarget()
    {
        GChess[] result = new GChess[speceficTargetID.Count];
        for (int i = 0; i < result.Length; i++)
            result[i] = AIManager.instance.GetSpeceficTarget(speceficTargetID[i]);
        return new List<GChess>(result);
    }
    public override Vector2Int[] GetPreferLocation(GChess target)
    {
        Vector2Int[] result = new Vector2Int[speceficLocationOffset.Count];
        for (int i = 0; i < result.Length; i++)
            result[i] = target.location+speceficLocationOffset[i];
        return result;
    }
}
