using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "CloseAttack", menuName = "Skills/AISkill/CloseAttack")]
public class SCloseAttack : AISkill
{
    protected Vector2Int direction; 
    public override Vector2Int[] GetRange()
    {
        return GetRangeWithLength(1);
    }
    public override void Decide(GChess target)
    {
        direction = target.location-owner.location;
    }
    public override void PreCast()
    {
        GChess chess = GridManager.instance.GetChess(owner.location + direction);
        if (chess != null)
        {
            chess.FreezeFoot();
            UnityAction a; UnityAction<Element> b;
            a = () =>
              {
                  chess.DeactiveFreezeFoot();
              };
            b = (element) =>
              {
                  if (element == Element.Fire)
                  {
                      chess.DeactiveFreezeFoot();
                  };
              };
            owner.eBeForceMove.AddListener(a);
            owner.eElementReaction.AddListener(b);
            chess.eFreezeFootBroken.AddListener(() =>
            {
                owner.eBeForceMove.RemoveListener(a);
                owner.eElementReaction.RemoveListener(b);
            });
            
        }
    }
    public override void Perform()
    {
        base.Perform();
        GChess chess = GridManager.instance.GetChess(owner.location + direction);
        if(chess!=null)
            chess.ElementReaction(Element.Ice);
    }
    
    public override Vector2Int[] GetAffectRange()
    {
        return new Vector2Int[] { direction + owner.location };
    }
}
