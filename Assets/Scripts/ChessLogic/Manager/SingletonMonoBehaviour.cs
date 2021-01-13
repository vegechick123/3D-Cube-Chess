using System;
using System.Collections;
using UnityEngine;
public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    static bool debugLog = false;
    public static T instance { get; protected set; }
    [Obsolete]
    protected IEnumerator ObsoleteCoroutine;//用于异步的协程
    virtual protected void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("存在多个相同的Manager类：" + this.GetType());
            Destroy(this);
        }
        else
        {
            instance = this as T;
            if(debugLog)
                Debug.Log("ManagerCreate:" + this.GetType());
        }

    }
    virtual protected void OnDestroy()
    {
        if (debugLog)
            Debug.Log("ManagerDestory:" + this.GetType());
        instance = null;
    }
}