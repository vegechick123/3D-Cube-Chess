using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public static class MyUniTaskExtensions
{
   class EventListener
    {
        public bool receive=false;
        //public 
    }
    async public static UniTask WaitUntilEvent(UnityEvent targetEvent)
    {
        Assert.IsNotNull(targetEvent);
        EventListener temp = new EventListener();
        targetEvent.AddListenerForOnce(()=>temp.receive = true);
        await UniTask.WaitUntil(() => temp.receive);
    }
}
