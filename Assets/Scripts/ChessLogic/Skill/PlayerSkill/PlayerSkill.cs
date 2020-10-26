using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;

public abstract class PlayerSkill : Skill
{
    public Sprite icon;
    public string[] cursorHints;
    public void CreateCommand()
    {
       
        PlayerControlManager.instance.PreemptSkillCommand(this);
        return;
    }
    public Delegate GetCastAction()
    {
        //return null;
        MethodInfo methodInfo = this.GetType().GetMethod("Cast");

        if (methodInfo == null)
        {
            Debug.LogError(this.GetType().ToString() + "类没有实现Cast方法");
            return null;
        }

        Type[] types = (from parameters in methodInfo.GetParameters()
                        select parameters.ParameterType).ToArray();
        Type p = Expression.GetActionType(types);
        Delegate action = Delegate.CreateDelegate(p, this, methodInfo);
        return action;
    }
    public string GetCursorHint(int index)
    {
        if (index >= cursorHints.Length)
            return string.Empty;
        return cursorHints[index]; 
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
