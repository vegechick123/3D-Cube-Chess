using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;

public abstract class Skill : ScriptableObject
{
    [HideInInspector]
    public GChess owner;
    public Sprite icon;
    public abstract Vector2Int[] GetRange();
    public void CreateCommand()
    {
        MethodInfo methodInfo = this.GetType().GetMethod("Cast");

        if (methodInfo == null)
        {
            Debug.LogError(this.GetType().ToString() + "类没有实现Cast方法");
            return;
        }

        Type[] types = (from parameters in methodInfo.GetParameters()
                        select parameters.ParameterType).ToArray();
        Type p = Expression.GetActionType(types);
        Delegate action = Delegate.CreateDelegate(p, this, methodInfo);

        Vector2Int[] range = GetRange();
        RangeCommand res = new RangeCommand(GetRange(), owner, action, ConditionCheck);
        res.CreateFloorHUD(new Color(1, 0, 0, 0.5f));
        PlayerControlManager.instance.PreemptSkillCommand(res);
        return;
    }

    protected Vector2Int[] GetRangeWithLength(int length)
    {
        Queue<Vector2Int> res = new Queue<Vector2Int>();
        Vector2Int[] dir = new Vector2Int[4];
        dir[0] = new Vector2Int(1, 1);
        dir[1] = new Vector2Int(1, -1);
        dir[2] = new Vector2Int(-1, 1);
        dir[3] = new Vector2Int(-1, -1);
        for (int x = 0; x <= length; x++)
            for (int y = 0; x + y <= length; y++)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (x == 0 && y == 0 && i >= 1)
                    {
                        break;
                    }
                    if (x == 0 && i >= 2)
                    {
                        break;
                    }
                    if (y == 0 && (i == 1|| i == 3))
                    {
                        continue;
                    }

                    Vector2Int loc = new Vector2Int(x, y) * dir[i] + owner.location;
                    if (GridManager.instance.InRange(loc))
                        res.Enqueue(loc);
                }
            }
        return res.ToArray();
    }
    /// <summary>
    /// 检测是否接受输入的参数
    /// </summary>
    /// <param name="index">表示第几个参数，下标从零开始</param>
    /// <param name="parameter">输入的参数对象</param>
    /// <returns></returns>
    public virtual bool ConditionCheck(int index, object[] parameters)
    {
        return true;
    }
}
