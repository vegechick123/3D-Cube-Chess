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
        switch (preferType)
        {
            case PreferType.Chest:
                List<GChess> t = new List<GChess>();
                t.AddRange(GridManager.instance.chests.FindAll((t) => t.complete));
                return t;
            case PreferType.PlayerChess:
                
                break;
        }
        return null;
    }
}