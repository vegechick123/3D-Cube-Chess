using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;

[RequireComponent(typeof(GChess))]
public abstract class Skill : MonoBehaviour
{
    [HideInInspector]
    public GChess chess;
    public Sprite sprite;
    public void Awake()
    {
        chess = GetComponent<GChess>();
    }
    public abstract Vector2Int[] GetRange();
    public void CreateCommand()
    {
        ///////
        MethodInfo methodInfo = this.GetType().GetMethod("Cast");
        Type[] types= (from parameters in  methodInfo.GetParameters()
                      select parameters.ParameterType).ToArray();
        Type p = Expression.GetActionType(types);
        Delegate action = Delegate.CreateDelegate(typeof(Action<GChess>),this,methodInfo);
        //////
        Vector2Int[] range = GetRange();
        RangeCommand res = new RangeCommand(GetRange(), chess, action);
        res.CreateFloorHUD(new Color(1,0,0,0.5f));
        PlayerControlManager.instance.PreemptSkillCommand(res);
        return ;
    }
}
