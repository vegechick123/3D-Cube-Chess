using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.Events;
using Ludiq;
using Bolt;
public enum Element
{
    Fire,
    Ice,
    Water,
    Thunder,
    None
}
public class CElementComponent : Component
{

    // Start is called before the first frame update    

    public bool conductive = false;
    public virtual void OnHitElement(Element element)
    {
        switch (element)
        {
            case Element.Thunder:
                if (!conductive)
                {
                    CustomEvent.Trigger(gameObject, "OnHitElement", element);
                }
                else
                {
                    List<Vector2Int> range = GridExtensions.BFS(location,
                        (Vector2Int loc) =>
                        {
                            GChess chess = GridManager.instance.GetChess(loc);
                            GFloor floor = GridManager.instance.GetFloor(loc);
                            return floor?.GetComponent<CElementComponent>().conductive == true || chess?.GetComponent<CElementComponent>().conductive == true;
                        }
                    , out _);
                    foreach (Vector2Int loc in range)
                    {
                        GFloor floor = GridManager.instance.GetFloor(loc);
                        if (floor != null)
                            CustomEvent.Trigger(floor.gameObject, "OnHitElement", element);
                    }
                }
                break;
            default:
                CustomEvent.Trigger(gameObject, "OnHitElement", element);
                break;
        }
    }
}
