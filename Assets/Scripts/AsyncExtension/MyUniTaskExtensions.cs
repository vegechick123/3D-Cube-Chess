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
    class EventListener<T>
    {
        public bool receive = false;
        public T result=default(T); 
    }
    async public static UniTask WaitUntilEvent(UnityEvent targetEvent)
    {
        Assert.IsNotNull(targetEvent);
        EventListener temp = new EventListener();
        targetEvent.AddListenerForOnce(()=>temp.receive = true);
        await UniTask.WaitUntil(() => temp.receive);
    }
    async public static UniTask<T> WaitUntilEvent<T>(UnityEvent<T> targetEvent)
    {
        Assert.IsNotNull(targetEvent);
        EventListener<T> temp = new EventListener<T>();
        targetEvent.AddListenerForOnce((T res) => { temp.receive = true;temp.result = res; });
        await UniTask.WaitUntil(() => temp.receive);
        return temp.result;
    }
}
