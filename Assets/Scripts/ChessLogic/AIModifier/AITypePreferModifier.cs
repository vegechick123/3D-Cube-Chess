using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PreferType
{
    Chest,
    PlayerChess
}
public class AITypePreferModifier : AIPreferTargetModifier
{
    public PreferType preferType;
    public override List<GChess> GetPreferTarget()
    {
        List<GChess> res;
        switch (preferType)
        {
            case PreferType.Chest:
                res = new List<GChess>();
                res.AddRange(GridManager.instance.chests.FindAll((t) => t.complete));
                return res;
            case PreferType.PlayerChess:
                
                break;
        }
        return null;
    }
}