using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TestSkill", menuName = "Skills/TestSkill")]
public class SLog: Skill
{
    public int length;
    public void Cast(GFloor floor)
    {
        Debug.Log(floor);
    }
    public override Vector2Int[] GetRange()
    {
        return GetRangeWithLength(length);
    }
}
