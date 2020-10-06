using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GFrozenFloor : GFloor
{
    protected override void OnRoundEnd()
    {
        base.OnRoundEnd();
        GChess chess= GridManager.instance.GetChess(location);
        if(chess!=null)
        {
            chess.ElementReaction(Element.Ice);
        }
    }

}
