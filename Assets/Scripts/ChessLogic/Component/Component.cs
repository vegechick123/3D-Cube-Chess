using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Component : MonoBehaviour
{
    [HideInInspector]
    public GActor actor;
    public Vector2Int location { get { return actor.location; }set { actor.location = value; } }
    protected int curMovement { get { return (actor as GChess).curMovement; } }
    virtual protected void Awake()
    {
        actor = GetComponent<GActor>();
    }
    virtual public void OnGameAwake()
    {
    }
    virtual public void OnGameStart()
    {
    }
}
