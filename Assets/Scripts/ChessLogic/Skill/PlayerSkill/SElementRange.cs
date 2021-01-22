using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ElementRange", menuName = "Skills/ElementRange")]
public class SElementRange : PlayerSkill
{
    // Start is called before the first frame update
    public int length = 3;
    public int effectRange = 3;
    public Element element;
    public override Vector2Int[] GetRange()
    {
        return GetRangeWithLength(length);
    }
    public void Cast(GActor actor)
    {
        Vector2Int location = actor.location;
        List<GFloor> floors = GridManager.instance.GetFloorsInRange(GridManager.instance.GetCircleRange(location, effectRange));
        GChess[] chesses=GridManager.instance.GetChessesInRange(GridManager.instance.GetCircleRange(location,effectRange));
        foreach(GFloor floor in floors)
        {
            floor.ElementReaction(element);
        }
        foreach (GChess chess in chesses)
        {
            chess.ElementReaction(element);
        }
    }
}
