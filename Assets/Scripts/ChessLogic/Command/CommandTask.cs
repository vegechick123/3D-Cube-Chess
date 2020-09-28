using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

public class CommandTask : CommandTaskBase
{
    

    public CommandTask(GActor obj,Delegate action,Func<int,object[],bool> _checker=null)
    {
        task = action;
        castObject = obj;
        ParameterInfo[] t= action.GetMethodInfo().GetParameters();
        types = new Type[t.Length];
        for(int i=0;i<t.Length;i++)
        {
            types[i] = t[i].ParameterType;
        }
        //types =action.GetType().GetGenericArguments();
        parameters = new object[types.Length];        
        curID = 0;
        RefreshInputMode();
        checker = _checker;
    }
}