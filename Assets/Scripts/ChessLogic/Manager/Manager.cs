using UnityEngine;
public class Manager<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T instance { get; protected set; }
    virtual protected void Awake()
    {
        if(instance!=null)
        {
            Debug.LogError("存在多个相同的Manager类：" + this.GetType());
            Destroy(this);
        }
        else
        {
            instance = this as T;
        }

    }
    virtual protected void OnDestroy()
    {
        instance = null;
    }
}