using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damp : Modifier
{
    public override void OnPassFloor(GFloor floor)
    {
        base.OnPassFloor(floor);
        ElementSystem.ApplyElementAtAsync(floor.location, Element.Water);
    }
}
