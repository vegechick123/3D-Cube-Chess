using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : FloorAttachment
{
    public bool burning = false;
    public override void OnHitElement(Element element)
    {
        switch (element)
        {
            case Element.Fire:
                break;
            case Element.Ice:
            case Element.Water:
                break;

        }
    }
    //public void EnterBuring()
    //{
    //}
    //public void EnterBuring()
    //{
    //}
}