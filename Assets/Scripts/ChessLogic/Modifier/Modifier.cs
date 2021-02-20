using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modifier : ScriptableObject
{
    [HideInInspector]
    public GChess chess;
    public void Init(GChess owner)
    {
        chess = owner;
    }
    public virtual void OnDeath()
    {

    }
}
