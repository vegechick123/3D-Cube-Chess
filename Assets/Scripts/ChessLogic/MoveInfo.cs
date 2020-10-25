using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveInfo
{
    public GChess owner;    
    public Vector2Int origin;
    public Vector2Int destination;
    public Quaternion originRotation;
    public Quaternion destinationRotation;
}
