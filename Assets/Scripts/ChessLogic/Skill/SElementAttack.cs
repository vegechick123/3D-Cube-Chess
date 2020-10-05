using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ElementAttack", menuName = "Skills/ElementAttack")]
public class SElementAttack : PlayerSkill
{
    // Start is called before the first frame update
    public int length = 3;
    public Element element;
    public override Vector2Int[] GetRange()
    {
        return GetRangeWithLength(length);
    }
    public void Cast(GChess chess)
    {
        chess.ElementReaction(element);
    }
}
