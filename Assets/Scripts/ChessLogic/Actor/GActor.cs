using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GActor : MonoBehaviour
{
    /// <summary>
    /// 表示Actor在网格中的位置
    /// 请勿直接更改
    /// 使用GChes中的MoveTo或MoveToDirectly来修改
    /// </summary>
    public Vector2Int location;
    [HideInInspector]
    public Renderer render;
    [HideInInspector]
    public Material originMaterial;
    [HideInInspector]
    protected CElementComponent elementComponent;
    virtual protected void Awake()
    {
        //注册事件
        GameManager.instance.eRoundStart.AddListener(OnGameStart);
        GameManager.instance.eRoundEnd.AddListener(OnGameEnd);
        GameManager.instance.eGameStart.AddListener(OnGameStart);
        GameManager.instance.eGameEnd.AddListener(OnGameEnd);
        render = GetComponent<Renderer>();
        if(render==null)
        {
            render = GetComponentInChildren<Renderer>();
        }
        elementComponent = GetComponent<CElementComponent>();
        originMaterial = render.material;
    }
    public virtual void ElementReaction(Element element)
    {
        if(elementComponent)
        {
            elementComponent.OnHitElement(element);
        }
    }
    virtual protected void OnRoundStart()
    {

    }
    virtual protected void OnRoundEnd()
    {

    }
    virtual protected void OnGameStart()
    {

    }
    virtual protected void OnGameEnd()
    {

    }
}
