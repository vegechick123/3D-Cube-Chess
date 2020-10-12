using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpanwnFire", menuName = "Skills/SpanwnFire")]
public class SSpanwnFire: PlayerSkill
{
    public int length;
    public GameObject prefabtest;
    public void Cast(GFloor floor)
    {
        Debug.Log(floor);
        GridManager.instance.InstansiateChessAt(prefabtest, floor.location);
        Vector2Int[] dir = new Vector2Int[] { Vector2Int.down, Vector2Int.up, Vector2Int.left, Vector2Int.right };
        foreach(Vector2Int d in dir)
        {
            GChess t = GridManager.instance.GetChess(floor.location + d);
            if(t!=null)
            {
                t.ElementReaction(Element.Fire);
            }
        }
    }
    public override Vector2Int[] GetRange()
    {
        return GetRangeWithLength(length);
    }
    public override bool ConditionCheck(int index, object[] parameters)
    {
        if (index == 0)
        {
            GFloor floor = parameters[index] as GFloor;
            return GridManager.instance.GetChess(floor.location) == null;
        }
        else
        {
            return true;
        }
    }
}

