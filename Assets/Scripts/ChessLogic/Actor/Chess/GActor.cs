using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.Events;
public abstract class GActor : MonoBehaviour, IGetInfo
{
    /// <summary>
    /// 表示Actor在网格中的位置
    /// 请勿直接更改
    /// 使用GChes中的MoveTo或MoveToDirectly来修改
    /// </summary>
    public Vector2Int location;
    [HideInInspector]
    public MeshRenderer render;
    public Animator animator { get { return render.GetComponent<Animator>(); } }
    [HideInInspector]
    public MeshFilter meshFilter;
    [HideInInspector]
    public Material originMaterial;
    [HideInInspector]
    public CElementComponent elementComponent;
    [HideInInspector]
    public UnityEvent<Element> eElementReaction = new EventWrapper<Element>();


    public string title;
    public string info;

    /// <summary>
    /// 禁用Awake
    /// </summary>
    virtual public void GAwake()
    {
        
        //Debug.Log($"GActor:{this} Begin");
    }
    protected virtual void Awake()
    {
        render = GetComponent<MeshRenderer>();
        if (render == null)
        {
            render = GetComponentInChildren<MeshRenderer>();

        }
        if (render != null)
        {
            meshFilter = render.GetComponent<MeshFilter>();
            originMaterial = render.sharedMaterial;
        }
        elementComponent = GetComponent<CElementComponent>();

        
    }
    virtual public void GEnd()
    {
        //Debug.Log($"GActor:{this} End");
    }
    public virtual void ElementReaction(Element element)
    {
        eElementReaction.Invoke(element);
    }
    public void CreateFloatTextOnHead(TextTag tag)
    {
        UIManager.instance.CreateFloatText(transform.position + Vector3.up, tag);
    }
    public void CreateFloatTextOnHead(string text, Color color)
    {
        UIManager.instance.CreateFloatText(transform.position + Vector3.up, text, color);
    }
    virtual protected void OnGameEnd()
    {

    }
    virtual public List<IGetInfo> GetInfos()
    {
        List<IGetInfo> result = new List<IGetInfo>();
        result.AddRange(GetComponents<CExtraInformation>());
        return result;
    }
    public virtual string GetTitle()
    {
        return title;
    }

    public virtual string GetInfo()
    {
        return info;
    }
    public void OnValidate()
    {
        if (GridManager.instance != null)
        {
            Vector3 position = GridManager.instance.GetFloorPosition3D(location);
            transform.position = new Vector3(position.x, transform.position.y, position.z);
        }
    }
}
