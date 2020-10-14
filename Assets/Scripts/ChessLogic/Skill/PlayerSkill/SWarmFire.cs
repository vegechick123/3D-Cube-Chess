using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WarmFire", menuName = "Skills/WarmFire")]
public class SWarmFire: PlayerSkill
{
    // Start is called before the first frame update
    public int length = 3;
    public override Vector2Int[] GetRange()
    {
        return GridManager.instance.GetFourRayRange(owner.location,length);
    }
    public void Cast(GChess chess)
    {
        TakeEffect(() =>
        {
            chess.ElementReaction(Element.Fire);
            chess.Warm();
        },
        owner.location, chess.location);        
    }
}
