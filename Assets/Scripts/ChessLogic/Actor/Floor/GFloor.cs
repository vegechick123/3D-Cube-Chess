using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;
using Ludiq;
public class GFloor : GActor
{
    public bool transitable=true;
    public virtual void OnChessEnter(GChess chess)
    {
        CustomEvent.Trigger(gameObject,"OnChessEnter", chess);
    }
}
