using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TestSkill", menuName = "Skills/TestSkill")]
public class SSpanwnFire: PlayerSkill
{
    public int length;
    public GameObject prefabtest;
    public void Cast(GFloor floor)
    {
        Debug.Log(floor);
        GridManager.instance.InstansiateChessAt(prefabtest, floor.location);

    }
    public override Vector2Int[] GetRange()
    {
        return GetRangeWithLength(length);
    }
}
