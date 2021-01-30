using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Component : MonoBehaviour
{
    [HideInInspector]
    public GActor actor;
    public Vector2Int location { get { return actor.location; }set { actor.location = value; } }

    virtual protected void Awake()
    {
        actor = GetComponent<GActor>();
    }
}
