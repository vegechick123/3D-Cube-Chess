using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Modifier/DeathBurst")]
public class DeathBurst : Modifier
{
    public int affectRange = 1;
    public Element element;
    public override void OnDeath()
    {
        _=ElementSystem.ApplyElementAtAsync(GridManager.instance.GetCircleRange(chess.location, affectRange),element);
    }
}
