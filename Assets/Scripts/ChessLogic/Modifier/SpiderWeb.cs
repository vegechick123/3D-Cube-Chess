using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Modifier/SpiderWeb")]
public class SpiderWeb : Modifier
{
    public override void InitByOwner(GChess owner)
    {
        base.InitByOwner(owner);
        owner.eLocationChange.AddListener(DisConnect);
        caster.eLocationChange.AddListener(DisConnect);
        owner.eDie.AddListener(DisConnect);
        caster.eDie.AddListener(DisConnect);
        Connect();        
    }
    public void Connect()
    {
        Debug.Log("Connect");
        owner.unableTomove.value++;
    }
    public void DisConnect()
    {
        Debug.Log("DisConnect");
        owner.unableTomove.value--;
        EndModifier();
    }
    public override void OnPlayerTurn()
    {
        base.OnPlayerTurn();
        EndModifier();
    }
}
