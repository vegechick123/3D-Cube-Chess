using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorAttachment : MonoBehaviour
{
    public GFloor owner;
    public virtual void Begin()
    {
       
    }
    public virtual void End()
    {
        Destroy(gameObject, 5f);
    }
    public virtual void OnHitElement(Element element)
    {

    }
}
